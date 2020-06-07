using System;
using System.Diagnostics.CodeAnalysis;
using NimbleConfig.Core.Logging;
using Serilog;

namespace andymac4182.Reference.Infrastructure.Logging
{
    [SuppressMessage("ReSharper", "Serilog004")]
    public class NimbleConfigLogger: IConfigLogger
    {
        private readonly ILogger _logger;

        public NimbleConfigLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void Trace(string message)
        {
            _logger.Verbose(message);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Info(string message)
        {
            _logger.Information(message);
        }

        public void Warning(string message)
        {
            _logger.Warning(message);
        }

        public void Error(string message, Exception ex)
        {
            _logger.Error(ex, message);
        }
    }
}