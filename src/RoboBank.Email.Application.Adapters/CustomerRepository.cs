using System.Threading.Tasks;
using Newtonsoft.Json;
using RoboBank.Email.Application.Ports;
using RoboBank.Email.Domain;
using StackExchange.Redis;

namespace RoboBank.Email.Application.Adapters
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;

        public CustomerRepository(ConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task<Customer> GetByIdAsync(string id)
        {
            var database = _connectionMultiplexer.GetDatabase();
            var customerData = await database.StringGetAsync(id);
            return JsonConvert.DeserializeObject<Customer>(customerData);
        }
    }
}
