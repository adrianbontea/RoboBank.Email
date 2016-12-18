namespace RoboBank.Email.Application
{
    public class AccountEventInfo
    {
        public string AccountId { get; set; }

        public string CustomerId { get; set; }

        public string Type { get; set; }

        public decimal Amount { get; set; }
    }
}
