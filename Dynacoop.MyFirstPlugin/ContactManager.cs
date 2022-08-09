using Microsoft.Xrm.Sdk;
using SharedProject1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynacoop.MyFirstPlugin
{
    public class ContactManager : PluginImplement
    {
        public override void ExecutePlugin(IServiceProvider serviceProvider)
        {
            Entity contact = this.Context.MessageName == "Create" ? (Entity)this.Context.InputParameters["Target"] : (Entity)this.Context.PostEntityImages["PostImage"];

            if (contact.Contains("dyp_convidarcontato"))
            {
                if ((bool)contact["dyp_convidarcontato"])
                {
                    ConviteDoEvento conviteDoEvento = new ConviteDoEvento(this.Service);
                    conviteDoEvento.Create(
                        contact.ToEntityReference(),
                        (EntityReference)contact["dyp_proximoevento"],
                        contact["emailaddress1"].ToString()
                    );
                }
            }
        }
    }
}
