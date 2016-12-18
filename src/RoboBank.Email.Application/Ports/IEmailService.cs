using System.Threading.Tasks;

namespace RoboBank.Email.Application.Ports
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string message);
    }
}
