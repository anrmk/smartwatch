using System;
using System.Reflection;

namespace Core.Dto {
    public class RequestDto {
        /// <summary>
        /// Информация по устройству
        /// </summary>
        public DeviceDto Device { get; set; }

        public DeviceLocationDto Location { get; set; }
    }

    public class ResponseDto {
        /// <summary>
        /// Версия пакета
        /// </summary>
        public string PacketVersion { get; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>
        /// Динамические параметры
        /// </summary>
        public dynamic Param { get; set; }

        /// <summary>
        /// Время выполнения задания
        /// </summary>
        public TimeSpan LeadTime { get; set; } = new TimeSpan();

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }
    }
}
