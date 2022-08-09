using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;

namespace Dynacoop.MyFirstPlugin.ActionExample
{
    public class ExampleCode : CodeActivity
    {
        [Input("OpportunityId")]
        public InArgument<string> OpportunityId { get; set; }

        [Output("OutId")]
        public OutArgument<string> OutId { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // Getting OrganizationService from Context
            var workflowContext = context.GetExtension<IWorkflowContext>();
            var serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
            var orgService = serviceFactory.CreateOrganizationService(workflowContext.UserId);

            //OutId.Set(context, OpportunityId.Get(context));
            OutId.Set(context, OpportunityId.Get(context));
        }
    }
}
