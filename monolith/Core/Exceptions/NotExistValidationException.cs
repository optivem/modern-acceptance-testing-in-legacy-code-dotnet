namespace Optivem.AtddAccelerator.EShop.Monolith.Core.Exceptions;

public class NotExistValidationException : ValidationException
{
    public NotExistValidationException(string message) : base(message)
    {
    }
}
