using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dto;
using Npgsql;

namespace Core.Managers {
    public interface IIndicatorManager {
        Task<ChartSeriesDto<double>> GetRegisteredDevices(DateTime start, DateTime end);
        Task<ChartSeriesDto<object[]>> GetDeviceAccuracyInfo(long id, DateTime start, DateTime end);
    }

    public class IndicatorManager: IIndicatorManager {
        private readonly string CONNECTION_STRING = "Host=localhost;Database=smartwatch;Username=postgres;Password=postgres";

        public async Task<ChartSeriesDto<double>> GetRegisteredDevices(DateTime start, DateTime end) {
            var query = "SELECT * FROM " +
                "(SELECT COUNT(\"Id\") as RegisteredDevice FROM \"Devices\" WHERE \"ProfileCardEntityId\" IS NOT NULL AND \"CreatedDate\" BETWEEN @startDate and @endDate) A " +
                "CROSS JOIN" +
                "(SELECT COUNT(\"Id\") as UnregisteredDevice FROM \"Devices\" WHERE \"ProfileCardEntityId\" IS NULL AND \"CreatedDate\" BETWEEN @startDate and @endDate) B";

            using(var connection = new NpgsqlConnection(CONNECTION_STRING)) {
                await connection.OpenAsync();
                using(var command = new NpgsqlCommand(query, connection)) {
                    command.Parameters.AddWithValue("@startDate", NpgsqlTypes.NpgsqlDbType.Timestamp, start);
                    command.Parameters.AddWithValue("@endDate", NpgsqlTypes.NpgsqlDbType.Timestamp, end);

                    using(var reader = await command.ExecuteReaderAsync()) {
                        while(await reader.ReadAsync()) {
                            int? registeredDeviceCount = null, unregisteredDeviceCount = null;
                            if(!reader.IsDBNull(0))
                                registeredDeviceCount = reader.GetInt32(0);
                            if(!reader.IsDBNull(1))
                                unregisteredDeviceCount = reader.GetInt32(1);

                            return new ChartSeriesDto<double>() {
                                Series = new List<ChartDataSeriesDto<double>>() {
                                    new ChartDataSeriesDto<double>() {
                                        Name = "Зарегистрированные",
                                        Data = new double[]{ registeredDeviceCount ?? 0}
                                    },
                                    new ChartDataSeriesDto<double>() {
                                        Name = "Незарегистрированные",
                                        Data = new double[]{ unregisteredDeviceCount ?? 0}
                                    }
                                }
                            };
                        }
                    }
                }
            }

            return new ChartSeriesDto<double>();
        }

        public async Task<ChartSeriesDto<object[]>> GetDeviceAccuracyInfo(long Id, DateTime start, DateTime end) {
            var query = "SELECT \"dl\".\"Timestamp\", \"dl\".\"Accuracy\", \"dl\".\"Speed\", \"dl\".\"DeviceEntity_Id\", \"d\".\"Name\", \"d\".\"Imei\", \"d\".\"Model\", \"d\".\"Manufacturer\", \"d\".\"Platform\" from \"DeviceLocations\" as dl " +
                        "LEFT JOIN \"Devices\" AS d " +
                        "ON \"d\".\"Id\" = \"dl\".\"DeviceEntity_Id\" " +
                        "WHERE \"dl\".\"Timestamp\" BETWEEN @startDate AND @endDate " +
                        "AND \"dl\".\"DeviceEntity_Id\" = @id " +
                        "ORDER BY \"dl\".\"Timestamp\"";

            using(var connection = new NpgsqlConnection(CONNECTION_STRING)) {
                await connection.OpenAsync();
                using(var command = new NpgsqlCommand(query, connection)) {
                    command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Bigint, Id);
                    command.Parameters.AddWithValue("@startDate", NpgsqlTypes.NpgsqlDbType.Timestamp, start);
                    command.Parameters.AddWithValue("@endDate", NpgsqlTypes.NpgsqlDbType.Timestamp, end);

                    using(var reader = await command.ExecuteReaderAsync()) {
                        var chartSerier = new ChartSeriesDto<object[]>() {
                            Categories = new List<string>(),
                            Series = new List<ChartDataSeriesDto<object[]>>()
                        };
                        var datas = new List<object[]>();

                        while(await reader.ReadAsync()) {
                            var timestamp = (DateTime)reader["Timestamp"];
                            var accuracy = (double)reader["Accuracy"];
                            chartSerier.Categories.Add(timestamp.ToString("hh:MM:ss"));
                            datas.Add(new object[] { timestamp.ToString("hh:MM:ss"), accuracy });
                        }
                        chartSerier.Series.Add(new ChartDataSeriesDto<object[]> {
                            Data = datas.ToArray()
                        });
                        return chartSerier;

                    }
                }
            }

            return null;
        }

        public async Task<ChartSeriesDto<double>> GetDbSize() {
            var query = "SELECT pg_size_pretty(pg_database_size(current_database()))";
            using(var connection = new NpgsqlConnection(CONNECTION_STRING)) {
                await connection.OpenAsync();
                using(var command = new NpgsqlCommand(query, connection)) {

                    using(var reader = await command.ExecuteReaderAsync()) {
                        while(await reader.ReadAsync()) {
                            int? registeredDeviceCount = null, unregisteredDeviceCount = null;
                            if(!reader.IsDBNull(0))
                                registeredDeviceCount = reader.GetInt32(0);

                            return new ChartSeriesDto<double>() {
                                Series = new List<ChartDataSeriesDto<double>>() {
                                    new ChartDataSeriesDto<double>() {
                                        Name = "Зарегистрированные",
                                        Data = new double[]{ registeredDeviceCount ?? 0}
                                    },
                                    new ChartDataSeriesDto<double>() {
                                        Name = "Незарегистрированные",
                                        Data = new double[]{ unregisteredDeviceCount ?? 0}
                                    }
                                }
                            };
                        }
                    }
                }
            }

            return null;
        }
    }
}
