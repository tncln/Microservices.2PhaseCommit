using Microservices._2PhaseCommit.Coordinator.Services.Abstraction;

namespace Microservices._2PhaseCommit.Coordinator.Services
{
    public class TransactionService : ITransactionService
    {
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

        public Task<Guid> CreateTransactionAsync()
        {
            throw new NotImplementedException();
        }

        public Task PrepareServicesAsync(Guid transactionId)
        {
            throw new NotImplementedException();
        }

        public Task RollBackAsync(Guid transactionId)
        {
            throw new NotImplementedException();
        }
    }
}
