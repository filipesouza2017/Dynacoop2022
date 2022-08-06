using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedProject.Models
{
    public class Contato
    {
        public IOrganizationService Service { get; set; }

        public Contato(IOrganizationService service)
        {
            this.Service = service;
        }

        private void Create(Guid accountId)
        {
            Entity contato = new Entity("contact");
            contato["firstname"] = "Filipe";
            contato["lastname"] = "Souza";
            contato["jobtitle"] = "Arquiteto Dynamics CE";
            contato["parentcustomerid"] = new EntityReference("account", accountId);
            this.Service.Create(contato);
        }

        public EntityCollection RecuperarContatosPorIdDaConta(Guid accountId, string[] columns)
        {
            QueryExpression recuperarContatos = new QueryExpression("contact");
            recuperarContatos.Criteria.AddCondition("parentcustomerid", ConditionOperator.Equal, accountId);
            recuperarContatos.ColumnSet.AddColumns(columns);
            EntityCollection todosContatos = this.Service.RetrieveMultiple(recuperarContatos);
            return todosContatos;
        }
    }
}
