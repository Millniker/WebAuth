namespace university.Server.Exception.ExceptionsModels;
[Serializable]

public class ItemNotFoundException:System.Exception
{
        public ItemNotFoundException() { }

        public ItemNotFoundException(string message) : base(message) { }

        public ItemNotFoundException(string message, System.Exception innerException) : base(message, innerException) { }
}