using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDeIntegracao.Models
{
    public class Conta
    {
        public IOrganizationService Service { get; set; }
       
        public string LogicalName = "account";

        public string Nome { get; set; }

        public Conta(IOrganizationService service)
        {
            this.Service = service;
        }

        private static void DeleteAccount(IOrganizationService service)
        {
            Console.WriteLine("Digite o id que você deseja deletar");
            var idParaDeletar = Console.ReadLine();

            if (ValidateGuid(idParaDeletar))
            {
                Conta conta = new Conta(service);
                conta.Delete(new Guid(idParaDeletar));
            }
        }

        private static bool ValidateGuid(string id)
        {
            if (id.ToString().Count() == 36)
            {
                Console.WriteLine("Id validado com sucesso");
                return true;
            }
            else
            {
                Console.WriteLine("Falha ao validar o id");
                return false;
            }
        }

        private static void UpdateAccount(IOrganizationService service)
        {
            Console.WriteLine("Por favor informe o nome da Conta que será atualizado");
            var nomeDaConta = Console.ReadLine();

            Console.WriteLine("Por favor informe o id da conta");
            var idDaConta = Console.ReadLine();

            if (ValidateGuid(idDaConta))
            {
                Console.WriteLine("Informações validadas. Atualizando Conta...");
                Conta conta = new Conta(service);
                conta.Nome = nomeDaConta;
                bool success = conta.Update(new Guid(idDaConta));

                if (success)
                {
                    Console.WriteLine("Conta atualizada com sucesso");
                }
                else
                {
                    Console.WriteLine("Erro ao atualizar a conta");
                }
            }
        }

        private static void CriarEDeletar(IOrganizationService service)
        {
            Console.WriteLine("Você deseja atualizar ou deletar uma conta? Digite A para atualizar ou D para deletar (A/D)");
            var respostaDoUsuario = Console.ReadLine();

            if (respostaDoUsuario == "A")
            {
                UpdateAccount(service);
            }
            else
            {
                if (respostaDoUsuario == "D")
                {
                    DeleteAccount(service);
                }
                else
                {
                    Console.WriteLine("Não foi possivel entender a sua digitação. Tente novamente.");
                }
            }
        }

        private void Create()
        {
            Entity conta = new Entity(this.LogicalName);
            conta["name"] = "Nova Conta - Campos Personalizados";
            conta["telephone1"] = "(11) 98983-6426";
            conta["fax"] = "12313918731";
            conta["websiteurl"] = "dynacoop2022.com.br";

            //Pequena
            conta["dyp_portedaempresa"] = new OptionSetValue(781870000);
            conta["dyp_totaldeoportunidades"] = new Money(150);
            conta["dyp_totaldeprodutos"] = new decimal(100.56);

            Guid accountId = this.Service.Create(conta);
        }

        public bool Update(Guid accountId)
        {
            if (this.Nome != null)
            {
                Entity conta = new Entity(this.LogicalName);
                conta.Id = accountId;
                conta["name"] = this.Nome;
                this.Service.Update(conta);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Delete(Guid accountId)
        {
            this.Service.Delete(this.LogicalName, accountId);
        }
    }
}
