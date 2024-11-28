function callActionOnFieldChange(executionContext) {
    var formContext = executionContext.getFormContext();

    var possibleDeal = formContext.getAttribute("arl_possibledealid").getValue();
    var product = formContext.getAttribute("arl_productsid").getValue();
    var discount = formContext.getAttribute("arl_discpount").getValue() || 0;
    var discountPercent = formContext.getAttribute("arl_discountpercent").getValue() || 0;

    if (possibleDeal && product) {
        var actionName = "ShopCartAction";
        var inputParams = {
            "Deal": {
                "@odata.type": "Microsoft.Dynamics.CRM.EntityReference",
                "id": possibleDeal[0].id,
                "logicalName": "arl_possibledealid"
            },
            "Product": {
                "@odata.type": "Microsoft.Dynamics.CRM.EntityReference",
                "id": product[0].id,
                "logicalName": "arl_productsid"
            },
            "Discount": discount,
            "DiscountPercent": discountPercent,
        };

        Xrm.WebApi.execute({
            entityName: actionName,
            entityType: "Microsoft.Dynamics.CRM.Action",
            parameters: inputParams
        }).then(
            function success(result) {
                var basePrice = result.response.OutputParameters.BasePrice;
                var priceAfterDiscount = result.response.OutputParameters.PriceAfterDiscount;

                formContext.getAttribute("arl_basePrice").setValue(basePrice);
                formContext.getAttribute("arl_totalPrice").setValue(priceAfterDiscount);
            },
            function error(error) {
                console.error(error.message);
                alert("Ошибка при вызове Action: " + error.message);
            }
        );
    }
}
