using Microservices._2PhaseCommit.Coordinator.Models;
using Microservices._2PhaseCommit.Coordinator.Models.Context;
using Microservices._2PhaseCommit.Coordinator.Services.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Microservices._2PhaseCommit.Coordinator.Services
{
    public class TransactionService : ITransactionService
    {
        TwoPhaseCommitContext _context;
        IHttpClientFactory _httpClientFactory;
        HttpClient _orderHttpClient;
        HttpClient _stockHttpClient;
        HttpClient _paymentHttpClient;
        public TransactionService(TwoPhaseCommitContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
           _orderHttpClient = _httpClientFactory.CreateClient("OrderAPI");
           _stockHttpClient = _httpClientFactory.CreateClient("StockAPI");
           _paymentHttpClient = _httpClientFactory.CreateClient("PaymentAPI");
        }
        
        public Task<bool> CheckReadyServicesAsync(Guid transactionId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckTransactionStateServiceAsync(Guid transactionId)
        {
            throw new NotImplementedException();
        }

        public Task CommitAsync(Guid transactionId)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> CreateTransactionAsync()
        {
            Guid transactionID = Guid.NewGuid();
            var nodes = await _context.Nodes.ToListAsync();
            nodes.ForEach(node => node.NodeStates = new List<NodeState>()
            {
                new(transactionID)
                {
                   Ready = Enums.ReadyType.Pending,
                   TransactionState = Enums.TransactionState.Pending
                }
            });
            await _context.SaveChangesAsync();
            return transactionID;
        }

        public async Task PrepareServicesAsync(Guid transactionId)
        {
            var transactionNodes = await _context.NodeStates
                .Include(x => x.Node)
                .Where(x => x.TransactionId == transactionId)
                .ToListAsync();
            foreach (var transactionNode in transactionNodes)
            {
                try
                {
                    var response = await (transactionNode.Node.Name switch
                    {
                        "Order.API" => _orderHttpClient.GetAsync("ready"),
                        "Stock.API" => _stockHttpClient.GetAsync("ready"),
                        "Payment.API" => _paymentHttpClient.GetAsync("ready"),
                    });
                    var result = bool.Parse(await response.Content.ReadAsStringAsync());
                    transactionNode.Ready = result ? Enums.ReadyType.Ready : Enums.ReadyType.Unready;
                }
                catch (Exception)
                {
                    transactionNode.Ready = Enums.ReadyType.Unready;
                }
            }
        }

        public Task RollBackAsync(Guid transactionId)
        {
            throw new NotImplementedException();
        }
    }
}
