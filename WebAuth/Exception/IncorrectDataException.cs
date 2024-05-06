namespace university.Server.Exception.ExceptionsModels;
[Serializable]

public class IncorrectDataException:System.Exception
{    
    public IncorrectDataException() { }

    public IncorrectDataException(string message) : base(message) { }

    public IncorrectDataException(string message, System.Exception innerException) : base(message, innerException) { }
    
}