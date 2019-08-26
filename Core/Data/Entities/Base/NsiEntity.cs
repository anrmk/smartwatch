
namespace Core.Entities.Base {
    public abstract class NsiEntity: Entity<long> {
        public int SortIndex { get; set; } = 0;
        public bool Visibility { get; set; } = true;
    }
}
