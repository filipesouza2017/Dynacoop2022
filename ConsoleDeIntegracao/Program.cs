using ConsoleDeIntegracao.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDeIntegracao
{
    class Program
    {
        static void Main(string[] args)
        {
            IOrganizationService service = ConnectionFactory.GetService();

            Console.WriteLine("Digite o nome da conta");
            var nomeDaConta = Console.ReadLine();

            string fetchXML = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='account'>
                                    <attribute name='name' />
                                    <attribute name='primarycontactid' />
                                    <attribute name='telephone1' />
                                    <attribute name='accountid' />
                                    <order attribute='name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='name' operator='eq' value='{nomeDaConta}' />
                                    </filter>
                                    <link-entity name='contact' from='contactid' to='primarycontactid' visible='false' link-type='outer' alias='contato'>
                                      <attribute name='emailaddress1' />
                                    </link-entity>
                                  </entity>
                                </fetch>";

            EntityCollection contas = service.RetrieveMultiple(new FetchExpression(fetchXML));

            foreach(Entity conta in contas.Entities)
            {
                Console.WriteLine(conta["telephone1"]);
            }

            Console.ReadKey();
        }

        private static void RecuperarContatosComNomeDaConta(IOrganizationService service, string nomeDaConta)
        {
            EntityCollection contas = RecuperarPorNome(service, nomeDaConta);

            QueryExpression recuperarContatos = new QueryExpression("contact");
            recuperarContatos.ColumnSet.AddColumns("fullname", "contactid");
            recuperarContatos.TopCount = 10;
            recuperarContatos.AddOrder("fullname", OrderType.Ascending);
            recuperarContatos.Criteria.AddCondition("parentcustomerid", ConditionOperator.Equal, contas.Entities.FirstOrDefault().Id);
            EntityCollection contatos = service.RetrieveMultiple(recuperarContatos);

            foreach (Entity contato in contatos.Entities)
            {
                Console.WriteLine(contato["fullname"]);
            }
        }

        private static void RecuperarConta(IOrganizationService service, string nomeDaConta)
        {
            EntityCollection contas = RecuperarPorNome(service, nomeDaConta);

            foreach (Entity conta in contas.Entities)
            {
                Console.WriteLine(((AliasedValue)conta["contato.emailaddress1"]).Value);

                RecuperarDadosDaConta(conta);
            }
        }

        private static EntityCollection RecuperarPorNome(IOrganizationService service, string nomeDaConta)
        {
            QueryExpression recuperarConta = new QueryExpression("account");
            recuperarConta.ColumnSet.AddColumns("name", "accountid", "primarycontactid", "transactioncurrencyid", "dyp_totaldeoportunidades", "dyp_totaldeprodutos", "dyp_portedaempresa");
            recuperarConta.Criteria.AddCondition("name", ConditionOperator.Equal, nomeDaConta);

            recuperarConta.AddLink("contact", "primarycontactid", "contactid");
            recuperarConta.LinkEntities[0].Columns.AddColumns("emailaddress1");
            recuperarConta.LinkEntities[0].EntityAlias = "contato";

            EntityCollection contas = service.RetrieveMultiple(recuperarConta);
            return contas;
        }

        private static void RecuperarDadosDaConta(Entity conta)
        {
            Console.WriteLine(((EntityReference)conta["primarycontactid"]).Id);
            //Console.WriteLine(((EntityReference)conta["primarycontactid"]).Name);

            //Console.WriteLine(conta["primarycontactidname"]);

            if (conta.Contains("dyp_totaldeoportunidades"))
                Console.WriteLine(((Money)conta["dyp_totaldeoportunidades"]).Value);

            Console.WriteLine(((OptionSetValue)conta["dyp_portedaempresa"]).Value);

            Console.WriteLine((decimal)conta["dyp_totaldeprodutos"]);

            //Console.WriteLine(conta.Id);
        }

    }
}
