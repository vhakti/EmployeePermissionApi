namespace EmployeePermissionApi.Application.brokers
{
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; }
        public string Topic {  get; set; }
    }
}
