using Microservices._2PhaseCommit.Coordinator.Enums;

namespace Microservices._2PhaseCommit.Coordinator.Models
{
    public record NodeState(Guid TransactionId)
    {
        public Guid Id { get; set; }
        public ReadyType Ready { get; set; }
        public TransactionState TransactionState { get; set; }
        public Node Node { get; set; }
    }
}
