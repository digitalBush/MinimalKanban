using System;

namespace Domain
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
        }

        [StringFormatMethod("message")]
        public DomainException(string message,params object[] arguments)
            : base(message)
        {
            
        }
    }
}