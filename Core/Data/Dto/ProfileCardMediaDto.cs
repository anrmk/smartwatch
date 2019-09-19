using System;

namespace Core.Dto {
    public class ProfileCardMediaDto {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Source { get; set; }
        public string Src => $"data:{ContentType};base64,{Convert.ToBase64String(Source)}";
        public byte[] Thumbnail { get; set; }
        public string ThumbnailSrc => $"data:{ContentType};base64,{Convert.ToBase64String(Thumbnail)}";
        public int Size => Source?.Length ?? 0;
        public long? ProfileCardId { get; set; }
    }
}
