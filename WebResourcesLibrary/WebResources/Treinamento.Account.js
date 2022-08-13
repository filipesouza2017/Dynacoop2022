if (typeof (Treinamento) == "undefined") { Treinamento = {} }
if (typeof (Treinamento.Account) == "undefined") { Treinamento.Account = {} }

Treinamento.Account = {
    OnLoad: function (executionContext) {
        var formContext = executionContext.getFormContext();

        //Set
        //formContext.getAttribute("dyp_cnpj").setValue("CNPJ JS");
        //formContext.getAttribute("industrycode").setValue(113);

        //var lookupEvento = [];
        //lookupEvento[0] = {};
        //lookupEvento[0].name = "Filipe";
        //lookupEvento[0].id = "3828d21d-1915-ed11-b83e-0022483766a7";
        //lookupEvento[0].entityType = "contact";
        //formContext.getAttribute("primarycontactid").setValue(lookupEvento);

        //formContext.getAttribute("parentaccountid").setRequiredLevel("required");
        //formContext.getControl("industrycode").setDisabled(true);
    },
    CNPJOnChange: function (executionContext) {
        var formContext = executionContext.getFormContext();

        var cnpj = formContext.getAttribute("dyp_cnpj").getValue();

        if (cnpj != null) {
            if (cnpj.length == 14) {
                var cnpjFormatado = cnpj.replace(/^(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, "$1.$2.$3/$4-$5");
                var id = Xrm.Page.data.entity.getId();

                var queryAccountId = "";

                if (id.length > 0) {
                    queryAccountId += " and accountid ne " + id;
                }

                Xrm.WebApi.online.retrieveMultipleRecords("account", "?$select=name&$filter=dyp_cnpj eq '" + cnpjFormatado + "'" + queryAccountId).then(
                    function success(results) {
                        if (results.entities.length == 0) {
                            formContext.getAttribute("dyp_cnpj").setValue(cnpjFormatado);
                        }
                        else {
                            formContext.getAttribute("dyp_cnpj").setValue("");
                            Treinamento.Account.DynamicsAlert("Essa conta já existe no sistema. Por favor insira um outro CNPJ", "CNPJ Duplicado");
                        }
                    },
                    function (error) {
                        Treinamento.Account.DynamicsAlert(error.message, "Error");
                    }
                );
            } else {
                Treinamento.Account.DynamicsAlert("Por favor coloque um cnpj válido", "CNPJ Inválido");
                formContext.getAttribute("dyp_cnpj").setValue("");
            }
        }
    },
    RecalcularOportunidadeOnClick: function () {
        Xrm.Utility.showProgressIndicator("Recalculando");

        var execute_new_SomaOportunidadesDoCliente_Request = {
            entity: { entityType: "account", id: Xrm.Page.data.entity.getId() },

            getMetadata: function () {
                return {
                    boundParameter: "entity",
                    parameterTypes: {
                        entity: { typeName: "mscrm.account", structuralProperty: 5 }
                    },
                    operationType: 0, operationName: "new_SomaOportunidadesDoCliente"
                };
            }
        };

        Xrm.WebApi.online.execute(execute_new_SomaOportunidadesDoCliente_Request).then(
            function success(response) {
                if (response.ok) {
                    Treinamento.Account.DynamicsAlert("As oportunidades foram recalculadas conforme o seu pedido", "Oportunidades Recalculada");
                }

                Xrm.Utility.closeProgressIndicator();
            }
        ).then(function (responseBody) {
            var result = responseBody;
            console.log(result);
            // Return Type: mscrm.new_SomaOportunidadesDoClienteResponse
            // Output Parameters
            var sucesso = result["Sucesso"]; // Edm.Boolean
        }).catch(function (error) {
            Xrm.Utility.closeProgressIndicator();
            console.log(error.message);
        });
    },
    DynamicsAlert: function (alertText, alertTitle) {
        var alertStrings = {
            confirmButtonLabel: "OK",
            text: alertText,
            title: alertTitle
        };

        var alertOptions = {
            heigth: 120,
            width: 200
        };

        Xrm.Navigation.openAlertDialog(alertStrings, alertOptions);
    }

}