using System.Diagnostics;
using System.Security.Permissions;
using XOracle.Infrastructure.Core;

namespace XOracle.Infrastructure
{
    public class TraceLogger : ILogger
    {
        private static readonly TraceSource _trace = new TraceSource("TraceSource", SourceLevels.Information);

        public TraceLogger() { }

        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        public TraceLogger(SourceLevels sourceLevels)
        {
            _trace.Switch.Level = sourceLevels;
        }

        public void LogInfo(string message, params object[] args)
        {
            _trace.TraceEvent(TraceEventType.Information, 0, message, args);
            
        }

        public void LogWarning(string message, params object[] args)
        {
            _trace.TraceEvent(TraceEventType.Warning, 0, message, args);
        }

        public void LogError(string message, params object[] args)
        {
            _trace.TraceEvent(TraceEventType.Error, 0, message, args);
        }

        public void LogVerbose(string message, params object[] args)
        {
            _trace.TraceEvent(TraceEventType.Verbose, 0, message, args);
        }
    }
}
