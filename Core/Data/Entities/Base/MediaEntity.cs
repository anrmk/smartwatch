namespace Core.Entities.Base {
    public abstract class MediaEntity: AuditableEntity<long> {
        public byte[] Source { get; set; }
        public string ContentType { get; set; } = "image/jpg";
    }
}
