using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject1.Models
{
    public class Oportunidade
    {
        public IOrganizationService Service { get; set; }
        public Oportunidade(IOrganizationService service)
        {
            this.Service = service;
        }

        public EntityCollection GetOpportunities(Guid accountId, int statusCode = -1)
        {
            QueryExpression getOpportunities = new QueryExpression("opportunity");
            getOpportunities.ColumnSet.AddColumns("totalamount");
            getOpportunities.Criteria.AddCondition("parentaccountid", ConditionOperator.Equal, accountId);

            if(statusCode != -1)
            {
                getOpportunities.Criteria.AddCondition("statuscode", ConditionOperator.Equal, statusCode);
            }

            return this.Service.RetrieveMultiple(getOpportunities);
        }


        public decimal SumTotalValue(EntityCollection opportunities)
        {
            decimal totalValue = 0;

            foreach (Entity opp in opportunities.Entities)
            {
                totalValue += ((Money)opp["totalamount"]).Value;
            }

            return totalValue;
        }
    }
}
