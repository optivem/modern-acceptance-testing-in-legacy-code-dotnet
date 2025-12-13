namespace Optivem.Lang;

public class Error
{
    public string Message { get; }
    public IReadOnlyList<FieldError>? Fields { get; }

    private Error(string message, IReadOnlyList<FieldError>? fields = null)
    {
        Message = message;
        Fields = fields;
    }

    public static Error Of(string message)
    {
        return new Error(message);
    }

    public static Error Of(string message, params FieldError[] fieldErrors)
    {
        return new Error(message, fieldErrors.ToList().AsReadOnly());
    }

    public static Error Of(string message, IReadOnlyList<FieldError> fieldErrors)
    {
        return new Error(message, fieldErrors);
    }

    public class FieldError
    {
        public string Field { get; }
        public string Message { get; }
        public string? Code { get; }

        public FieldError(string field, string message, string? code = null)
        {
            Field = field;
            Message = message;
            Code = code;
        }
    }
}
