using MyFinance.Application.Properties;

namespace MyFinance.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base(string.Format(Resources.EntityNotFound, name, key)) { }
    }
}
