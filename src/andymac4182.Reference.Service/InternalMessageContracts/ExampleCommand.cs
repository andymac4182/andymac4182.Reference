namespace andymac4182.Reference.Service.InternalMessageContracts
{
    public class ExampleCommand
    {
        public ExampleCommand(string message)
        {
            Message = message;
        }
        
        public string Message { get; }
    }
}