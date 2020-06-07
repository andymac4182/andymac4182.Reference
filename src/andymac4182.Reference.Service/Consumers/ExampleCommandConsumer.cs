using System.Threading.Tasks;
using andymac4182.Reference.Service.InternalMessageContracts;
using MassTransit;
using Serilog;

namespace andymac4182.Reference.Service.Consumers
{
    public class ExampleCommandConsumer : IConsumer<ExampleCommand>
    {
        private readonly ILogger _logger;

        public ExampleCommandConsumer(ILogger logger)
        {
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<ExampleCommand> context)
        {
            _logger.Debug("Received {@Message}", context.Message);
            await Task.CompletedTask;
        }
    }
}