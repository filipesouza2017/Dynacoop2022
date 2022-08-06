using SharedProject.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk.Messages;

namespace Dynacoop.MyFirstPlugin
{
    public class AccountManager : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext executionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(executionContext.UserId);
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            Entity conta = executionContext.Stage == 10 ? (Entity)executionContext.InputParameters["Target"] : (Entity)executionContext.PostEntityImages["PostImage"];
            
            //10 = Pre-Validation
            //20 - Pre-Operation
            //30 - Post-Operation

            if (executionContext.Stage == (int)SharedProject.Models.Enumerator.PluginStages.PreValidation)
            {
                string cnpj = conta["dyp_cnpj"].ToString();

                QueryExpression recuperarContaComCnpj = new QueryExpression("account");
                recuperarContaComCnpj.Criteria.AddCondition("dyp_cnpj", ConditionOperator.Equal, cnpj);
                EntityCollection contas = service.RetrieveMultiple(recuperarContaComCnpj);

                if (contas.Entities.Count() > 0)
                {
                    throw new InvalidPluginExecutionException("Já existe uma conta com este CNPJ");
                }
            }
            else
            {
                if ((bool)conta["dyp_convidarcontatos"])
                {
                    Contato contato = new Contato(service);
                    EntityCollection contatosDaConta =
                        contato.RecuperarContatosPorIdDaConta(
                            conta.Id,
                            new string[] {
                            "contactid"
                            });

                    ExecuteMultipleRequest executeMultipleRequest = new ExecuteMultipleRequest()
                    {
                        Requests = new OrganizationRequestCollection(),
                        Settings = new ExecuteMultipleSettings()
                        {
                            ContinueOnError = false,
                            ReturnResponses = true
                        }
                    };

                    foreach (Entity contatoEntity in contatosDaConta.Entities)
                    {
                        Entity conviteDoEvento = new Entity("dyp_convite_evento");
                        conviteDoEvento["dyp_contato"] = contatoEntity.ToEntityReference();
                        conviteDoEvento["dyp_evento"] = conta["dyp_proximoevento"];

                        CreateRequest createRequest = new CreateRequest()
                        {
                            Target = conviteDoEvento
                        };

                        executeMultipleRequest.Requests.Add(createRequest);
                    }

                    ExecuteMultipleResponse multipleResponse = (ExecuteMultipleResponse)service.Execute(executeMultipleRequest);

                    foreach (var response in multipleResponse.Responses)
                    {
                        if (response.Fault != null)
                        {
                            throw new InvalidPluginExecutionException("Erro ao cadastrar os convites para esse contato");
                        }
                    }
                }
            }
        }
    }
}
