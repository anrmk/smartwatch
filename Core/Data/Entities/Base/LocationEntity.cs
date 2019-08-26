using System;

namespace Core.Entities.Base {
    /// <summary>
    /// Координаты устройств
    /// </summary>
    public class LocationEntity : Entity<long> {
        /// <summary>
        /// Широта
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Долгота
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Точность
        /// </summary>
        public double Accuracy { get; set; }

        /// <summary>
        /// Высота над уровнем моря
        /// </summary>
        public double Altitude { get; set; }

        /// <summary>
        /// Признак получения координат от провайдера
        /// </summary>
        public bool IsFromMockProvider { get; set; }

        /// <summary>
        /// Скорость
        /// </summary>
        public double Speed { get; set; }

        /// <summary>
        /// Направление
        /// </summary>
        public double Direction { get; set; }

        /// <summary>
        /// Время регистрации
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }
    }
}
