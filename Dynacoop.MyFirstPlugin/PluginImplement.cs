using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynacoop.MyFirstPlugin
{
    public abstract class PluginImplement : IPlugin
    {
        public IPluginExecutionContext Context { get; set; }
        public IOrganizationServiceFactory ServiceFactory { get; set; }
        public IOrganizationService Service { get; set; }
        public ITracingService TracingService { get; set; }

        public void Execute(IServiceProvider serviceProvider)
        {
            Context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            ServiceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            Service = ServiceFactory.CreateOrganizationService(Context.UserId);
            TracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            ExecutePlugin(serviceProvider);
        }

        public abstract void ExecutePlugin(IServiceProvider serviceProvider);
    }
}
