namespace Microservices._2PhaseCommit.Coordinator.Services.Abstraction
{
    public interface ITransactionService
    {
        Task<Guid> CreateTransactionAsync();
        Task PrepareServicesAsync(Guid transactionId);
        Task<bool> CheckReadyServicesAsync(Guid transactionId);
        Task CommitAsync(Guid transactionId);
        Task<bool> CheckTransactionStateServiceAsync(Guid transactionId);
        Task RollBackAsync(Guid transactionId);
    }
}
