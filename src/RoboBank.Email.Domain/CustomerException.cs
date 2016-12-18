using System;

namespace RoboBank.Email.Domain
{
    public class CustomerException : Exception
    {
        public CustomerException(string message) : base(message)
        {
        }
    }
}
