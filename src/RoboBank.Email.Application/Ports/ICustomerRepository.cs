using System.Threading.Tasks;
using RoboBank.Email.Domain;

namespace RoboBank.Email.Application.Ports
{
    public interface ICustomerRepository
    {
        Task<Customer> GetByIdAsync(string id);
    }
}
