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
        
        public async Task<bool> CheckReadyServicesAsync(Guid transactionId) => (await _context.NodeStates.Where(x => x.TransactionId == transactionId).ToListAsync()).TrueForAll(x=>x.Ready == Enums.ReadyType.Ready);


        public async Task<bool> CheckTransactionStateServiceAsync(Guid transactionId) =>
            (await _context.NodeStates
            .Where(x => x.TransactionId == transactionId).ToListAsync())
            .TrueForAll(x => x.TransactionState == Enums.TransactionState.Done);

        public async Task CommitAsync(Guid transactionId)
        {
           var transactionNodes =await _context.NodeStates
                .Where(x=>x.TransactionId == transactionId)
                .Include(x => x.TransactionId)
                .ToListAsync();

            foreach (var transactionNode in transactionNodes)
            {
                try
                {
                    var response =await (transactionNode.Node.Name switch
                    {
                        "Order.API" => _orderHttpClient.GetAsync("commit"),
                        "Stock.API" => _stockHttpClient.GetAsync("commit"),
                        "Payment.API" => _paymentHttpClient.GetAsync("commit")
                    });
                    var result = bool.Parse(await response.Content.ReadAsStringAsync());
                    transactionNode.TransactionState = result ? Enums.TransactionState.Done : Enums.TransactionState.Abort;
                }
                catch (Exception)
                {
                    transactionNode.TransactionState = Enums.TransactionState.Abort;
                }
            }
            await _context.SaveChangesAsync();
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
            await _context.SaveChangesAsync();
        }

        public async Task RollBackAsync(Guid transactionId)
        {
            var transactionNodes = await _context.NodeStates
                .Where(x => x.TransactionId == transactionId)
                .Include(x => x.Node).ToListAsync();

            foreach (var transactionNode in transactionNodes)
            {
                try
                {
                    if(transactionNode.TransactionState == Enums.TransactionState.Done)
                        _ = await (transactionNode.Node.Name switch
                        {
                            "Order.API" => _orderHttpClient.GetAsync("RoolBack"),
                            "Stock.API" => _stockHttpClient.GetAsync("RoolBack"),
                            "Payment.API" => _paymentHttpClient.GetAsync("RoolBack")
                        });
                    transactionNode.TransactionState = Enums.TransactionState.Abort;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
