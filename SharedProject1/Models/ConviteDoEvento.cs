using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject1.Models
{
    public class ConviteDoEvento
    {
        public IOrganizationService Service { get; set; }

        public string TableName = "dyp_convite_evento";

        public ConviteDoEvento(IOrganizationService service)
        {
            this.Service = service;
        }

        public Guid Create(EntityReference contato, EntityReference evento, string email)
        {
            Entity conviteDoEvento = new Entity(this.TableName);
            conviteDoEvento["dyp_contato"] = contato;
            conviteDoEvento["dyp_evento"] = evento;
            conviteDoEvento["dyp_emaildoconvidado"] = email;
            return this.Service.Create(conviteDoEvento);
        }
    }
}
