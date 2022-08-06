using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDeIntegracao
{
    public class ConnectionFactory
    {
        public static IOrganizationService GetService()
        {
            string connectionString =
                "AuthType=OAuth;" +
                "Username=admin@dynacoop2022.onmicrosoft.com;" +
                "Password=P@ssw0rd;" +
                "Url=https://dynacoop2022.crm2.dynamics.com/;" +
                "AppId=934b952a-e47d-4970-ab98-e28e212ae48a;" +
                "RedirectUri=app://58145B91-0C36-4500-8554-080854F2AC97;";

            CrmServiceClient crmServiceClient = new CrmServiceClient(connectionString);
            return crmServiceClient.OrganizationWebProxyClient;
        }
    }
}
