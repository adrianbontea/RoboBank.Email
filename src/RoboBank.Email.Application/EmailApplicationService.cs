using System.Threading.Tasks;
using RoboBank.Email.Application.Ports;
using RoboBank.Email.Domain;

namespace RoboBank.Email.Application
{
    public class EmailApplicationService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmailService _emailService;

        public EmailApplicationService(ICustomerRepository customerRepository, IEmailService emailService)
        {
            _customerRepository = customerRepository;
            _emailService = emailService;
        }

        public async Task SendEmailForEventAsync(AccountEventInfo accountEventInfo)
        {
            var customer = await _customerRepository.GetByIdAsync(accountEventInfo.CustomerId);

            if (customer == null)
            {
                throw  new CustomerException("Customer not found.");
            }

            var actionName = accountEventInfo.Type.ToLower();
            var message =
                $"Hi, There has been a {actionName} on your account with IBAN {accountEventInfo.AccountId} with amount {accountEventInfo.Amount}";
            string subject = $"Activity on account with IBAN {accountEventInfo.AccountId}";

            await _emailService.SendAsync(customer.Email, subject, message);
        }
    }
}
