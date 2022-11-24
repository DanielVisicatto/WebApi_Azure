using Azure.Data.Tables;
using AzureTableWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace AzureTableWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly string _tableName;

        public EmployeeController(IConfiguration configuration)
        {
            //nosssa variáveis de configuração que buscam os dados no json...
            _connectionString = configuration.GetValue<string>("SAConnectionString");
            _tableName = configuration.GetValue<string>("AzureTableName");
            // criação da nossa model contact e instalação do do package Azure.Data.Tables via
            //comand line dotnet add package Azure.Data.Tables.
        }

        private TableClient GetTableClient()
        {
            var serviceClient = new TableServiceClient(_connectionString); //inicia o serviço
            var tableClient = serviceClient.GetTableClient(_tableName);    //pega a tabela

            tableClient.CreateIfNotExists();
            return tableClient;
        }

        [HttpPost]
        public IActionResult CreateTable([FromBody]Employee contact)
        {
            var tableClient = GetTableClient();           //chamando o método para reaproveitar código
                        
            contact.RowKey = Guid.NewGuid().ToString();   //identficador único global
            contact.PartitionKey = contact.RowKey;        //somente para exemplo nesse caso

            tableClient.UpsertEntity(contact);

            return Ok(contact);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTable(string id, Employee contact)
        {
            var tableClient = GetTableClient();
            var contactTable = tableClient.GetEntity<Employee>(id, id).Value;

            contactTable.Name = contact.Name;
            contactTable.Email = contact.Email;
            contactTable.Salary = contact.Salary;

            tableClient.UpsertEntity(contactTable);
            return Ok();
        }

        [HttpGet("ListAll")]
        public IActionResult GetAll()
        {
            var tableClient = GetTableClient();
            var contacts = tableClient.Query<Employee>().ToList(); //posso colocar uma condição aqui.

            return Ok(contacts);
        }

        [HttpGet("GetByName/{name}")]
        public IActionResult GetByName(string name)
        {
            var tableClient = GetTableClient();
            var contacts = tableClient.Query<Employee>(x => x.Name == name).ToList();

            return Ok(contacts);
        }

        [HttpDelete("id")]
        public IActionResult Delete(string id)
        {
            var tableClient = GetTableClient();
            tableClient.DeleteEntity(id, id);
            
            return NoContent();
        }
    }
}
