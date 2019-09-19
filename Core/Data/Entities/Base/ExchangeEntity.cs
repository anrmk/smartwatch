using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities.Base;

namespace Core.Base {
    /// <summary>
    /// Сушность с метками аудита
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    public abstract class ExchangeEntity<T>: Entity<T>, IAuditableEntity {
        /// <summary>
        /// Расширение файла (xls, jpg и тд)
        /// </summary>
        public string Source { get; set; }

        [Column(TypeName = "text")]
        public string Data { get; set; }
        public string Type { get; set; }
        public bool IsImport { get; set; }
        public bool Processed { get; set; }
        public string Error { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        [ScaffoldColumn(false)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Пользователь-создатель
        /// </summary>
        [MaxLength(256)]
        [ScaffoldColumn(false)]
        public string CreatedBy { get; set; } = "system";

        /// <summary>
        /// Дата обновления
        /// </summary>
        [ScaffoldColumn(false)]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Пользователь, обновивший запись
        /// </summary>
        [MaxLength(256)]
        [ScaffoldColumn(false)]
        public string UpdatedBy { get; set; } = "system";
    }
}