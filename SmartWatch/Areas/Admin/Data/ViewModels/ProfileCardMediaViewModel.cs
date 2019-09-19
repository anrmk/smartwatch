
namespace SmartWatch.Areas.Admin.Data.ViewModels {
    public class ProfileCardMediaViewModel {
        public long Id { get; set; }
        public string Name { get; set; }
        public byte[] Source { get; set; }
        public string Src { get; set; }
        public int Size { get; set; }
        public long? ProfileCardId { get; set; }
    }
}
