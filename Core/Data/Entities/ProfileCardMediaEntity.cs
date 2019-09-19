using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities.Base;

namespace Core.Entities {
    [Table(name: "ProfileCardMedias")]
    public class ProfileCardMediaEntity: MediaEntity {

        [ForeignKey("ProfileCard")]
        [Column("ProfileCardEntity_Id")]
        public long? ProfileCardId { get; set; }
        public virtual ProfileCardEntity ProfileCard { get; set; }

        /// <summary>
        /// Миниатюра изображения в массиве байтов
        /// </summary>
        public byte[] Thumbnail { get; set; }
    }
}
