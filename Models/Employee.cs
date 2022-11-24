using Azure;
using Azure.Data.Tables;

namespace AzureTableWebApi.Models
{
    public class Employee : ITableEntity
    {
        //minhas propriedades do contato
        public string Name { get; set; }
        public double Salary { get; set; }
        public string Email { get; set; }

        //propriedades obrigatórias do Azure Table
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
