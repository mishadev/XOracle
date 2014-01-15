using Microsoft.WindowsAzure.ServiceRuntime;
using System.Linq;

namespace XOracle.Azure.Web.Front
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            RoleEnvironment.Changing += RoleEnvironmentChanging;
            RoleEnvironment.Changed += RoleEnvironmentChanged;

            return base.OnStart();
        }

        private static void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            // for any configuration setting change except TraceEventTypeFilter
            if (e.Changes.OfType<RoleEnvironmentConfigurationSettingChange>().Any(change => change.ConfigurationSettingName != "TraceEventTypeFilter"))
            {
                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }

        private static void RoleEnvironmentChanged(object sender, RoleEnvironmentChangedEventArgs e)
        {
            // configure trace listener for any changes to EnableTableStorageTraceListener 
            if (e.Changes.OfType<RoleEnvironmentConfigurationSettingChange>().Any(change => change.ConfigurationSettingName == "TraceEventTypeFilter"))
            {
                //ConfigureTraceListener(RoleEnvironment.GetConfigurationSettingValue("TraceEventTypeFilter"));
            }
        }
    }
}
