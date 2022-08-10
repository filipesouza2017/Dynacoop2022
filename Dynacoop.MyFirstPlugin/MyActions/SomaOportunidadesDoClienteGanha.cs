using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using SharedProject1.Models;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynacoop.MyFirstPlugin.MyActions
{
    public class SomaOportunidadesDoClienteGanha : ActionImplement
    {
        [Input("StatusCode")]
        public InArgument<int> StatusCode { get; set; }

        [Output("Sucesso")]
        public OutArgument<bool> Sucesso { get; set; }

        public override void ExecuteAction(CodeActivityContext context)
        {
            Guid accountId = this.WorkflowContext.PrimaryEntityId;
            Oportunidade oportunidade = new Oportunidade(this.Service);
            EntityCollection opportunities = oportunidade.GetOpportunities(accountId, StatusCode.Get(context));
            decimal totalValue =  oportunidade.SumTotalValue(opportunities);

            Entity accountToUpdate = new Entity("account", accountId);
            accountToUpdate["dyp_oportunidades_ganhas"] = new Money(totalValue);
            this.Service.Update(accountToUpdate);

            Sucesso.Set(context, true);
        }
    }
}
