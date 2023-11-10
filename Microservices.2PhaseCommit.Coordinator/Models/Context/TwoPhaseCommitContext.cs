using Microsoft.EntityFrameworkCore;

namespace Microservices._2PhaseCommit.Coordinator.Models.Context
{
    public class TwoPhaseCommitContext : DbContext
    {
        public TwoPhaseCommitContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Node> Nodes { get; set; }
        public DbSet<NodeState> NodeStates { get; set; }
    }
}
