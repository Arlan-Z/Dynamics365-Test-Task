function callActionOnFieldChange(executionContext) {
    var formContext = executionContext.getFormContext();

    var possibleDeal = formContext.getAttribute("arl_possibledealid").getValue();
    var product = formContext.getAttribute("arl_productsid").getValue();
    var discount = formContext.getAttribute("arl_discpount").getValue() || 0;
    var discountPercent = formContext.getAttribute("arl_discountpercent").getValue() || 0;

    var execute_arl_ShopCartAction_Request = {
        // Parameters
        DiscountPercent: discountPercent, // Edm.Decimal
        Discount: discount, // Edm.Double
        PossibleDeal: { "@odata.type": "Microsoft.Dynamics.CRM.arl_possibledeal", arl_possibledealid :  possibleDeal[0].id}, // mscrm.arl_possibledeal
        ProductId: { "@odata.type": "Microsoft.Dynamics.CRM.arl_products", arl_productsid : product[0].id }, // mscrm.arl_products
    
        getMetadata: function () {
            return {
                boundParameter: null,
                parameterTypes: {
                    DiscountPercent: { typeName: "Edm.Decimal", structuralProperty: 1 },
                    Discount: { typeName: "Edm.Double", structuralProperty: 1 },
                    PossibleDeal: { typeName: "mscrm.arl_possibledeal", structuralProperty: 5 },
                    ProductId: { typeName: "mscrm.arl_products", structuralProperty: 5 }
                },
                operationType: 0, operationName: "arl_ShopCartAction"
            };
        }
    };
    
    Xrm.WebApi.execute(execute_arl_ShopCartAction_Request).then(
        function success(response) {
            if (response.ok) { return response.json(); }
        }
    ).then(function (responseBody) {
        var result = responseBody;
        console.log(result);
        // Return Type: mscrm.arl_ShopCartActionResponse
        // Output Parameters
        var baseprice = result["BasePrice"]; // Edm.Double
        var totalprice = result["TotalPrice"]; // Edm.Double
        formContext.getAttribute("arl_baseprice").setValue(baseprice);
        formContext.getAttribute("arl_totalprice").setValue(totalprice);
    }).catch(function (error) {
        console.log(error.message);
    });
}

var isManualSaveInProgress = false; 

async function onPriceListSave(executionContext) {
    var formContext = executionContext.getFormContext();
    var eventArgs = executionContext.getEventArgs();

    if (isManualSaveInProgress) {
        return;
    }

    eventArgs.preventDefault();

    try {
        var currentRecordId = formContext.data.entity.getId();
        if (currentRecordId) {
            currentRecordId = currentRecordId.replace("{", "").replace("}", "").toLowerCase();
        }

        var results = await Xrm.WebApi.retrieveMultipleRecords(
            "arl_pricelist",
            "?$select=arl_pricelistid&$top=1"
        );

        if (results.entities.length > 0) {
            var existingRecordId = results.entities[0]["arl_pricelistid"].toLowerCase();

            if (!currentRecordId || currentRecordId !== existingRecordId) {
                var alertStrings = {
                    confirmButtonLabel: "Ок",
                    title: "Сохранение невозможно",
                    text: "Объект Прайс-листа уже есть в системе",
                };
                var alertOptions = { height: 120, width: 260 };

                await Xrm.Navigation.openAlertDialog(alertStrings, alertOptions);
                return; 
            }
        }

        isManualSaveInProgress = true; 
        formContext.data.entity.save(); 
    } catch (error) {
        console.log("Ошибка при запросе:", error.message);
    }
}