using AwaraIT.Training.Application.Core;
using AwaraIT.Training.Domain.Extensions;
using AwaraIT.Training.Domain.Models.Crm.Entities;
using AwaraIT.Training.Plugins.PluginExtensions;
using AwaraIT.Training.Plugins.PluginExtensions.Enums;
using AwaraIT.Training.Plugins.PluginExtensions.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Linq;
using System.Threading;

namespace AwaraIT.Arlan.Plugins.Shopping_Cart
{
    public class ShopCartAction : BasicActivity
    {
        [Input("DiscountPercent")]
        public InArgument<Decimal> DiscountPercent { get; set; }

        [Input("Discount")]
        public InArgument<Double> Discount { get; set; }

        [Input("PossibleDeal")]
        [ReferenceTarget(Deal.EntityLogicalName)]
        public InArgument<EntityReference> PossibleDeal { get; set; }

        [Input("Product")]
        [ReferenceTarget(Product.EntityLogicalName)]
        public InArgument<EntityReference> ProductId { get; set; }

        [Output("BasePrice")]
        public OutArgument<Double> BasePrice { get; set; }

        [Output("TotalPrice")]
        public OutArgument<Double> TotalPrice { get; set; }
        protected override void Execute(CodeActivityContext context)
        {
            base.Execute(context);
            try
            {
                var workflowContext = context.GetExtension<IWorkflowContext>();
                if (workflowContext == null) throw new Exception("workflow is NULL");
                var serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
                if (serviceFactory == null) throw new Exception("serviceFactory is NULL");
                var service = serviceFactory.CreateOrganizationService(workflowContext.UserId);
                if (service == null) throw new Exception("service is NULL");

                //var discount = Discount.Get(context);
                //var discountPercent = DiscountPercent.Get(context);
                var productId = ProductId.Get(context);
                var possibleDeal = PossibleDeal.Get(context);

                var deal = service.Retrieve(possibleDeal.LogicalName, possibleDeal.Id, new ColumnSet(Deal.Metadata.RegionId)).ToEntity<Deal>();
                var product = service.Retrieve(productId.LogicalName, productId.Id, new ColumnSet(Product.Metadata.EduFormat, Product.Metadata.Subject, Product.Metadata.PrepFormat)).ToEntity<Product>();
                GetPrice(product, service, context);
            }
            catch (Exception ex)
            {
                Logger.ERROR("Action TestAction", ex.ToString());
            }
        }

        private void GetPrice(Product product, IOrganizationService service, CodeActivityContext context)
        {
            // Set Condition Values
            var query_arl_educformatid = product.EduFormat?.Id ?? throw new Exception("EduFormat is null.");
            var query_arl_prepformatid = product.PrepFormat?.Id ?? throw new Exception("PrepFormat is null.");
            var query_arl_subject = product.Subject?.Id ?? throw new Exception("Subject is null.");

            // Instantiate QueryExpression query
            var query = new QueryExpression("arl_pricelistitems")
            {
                TopCount = 10000,
                ColumnSet = new ColumnSet("arl_price") 
            };

            query.Criteria.AddCondition("arl_educformatid", ConditionOperator.Equal, query_arl_educformatid);
            query.Criteria.AddCondition("arl_prepformatid", ConditionOperator.Equal, query_arl_prepformatid);
            query.Criteria.AddCondition("arl_subject", ConditionOperator.Equal, query_arl_subject);

            var res = service.RetrieveMultiple(query);
            if (res.Entities.Count == 0)
            {
                throw new Exception("No Price List Item for this Product Found");
            }

            var firstEntity = res.Entities.First();
            if (!firstEntity.Contains("arl_price") || firstEntity["arl_price"] == null)
            {
                throw new Exception("'arl_price' field is null or missing.");
            }

            var price = firstEntity.GetAttributeValue<Double?>("arl_price") ?? throw new Exception("Unable to cast 'arl_price' to Double.");

            var discountPercent = (Double)DiscountPercent.Get(context); 
            var discount = Discount.Get(context);      

            var totalPrice = price * ((100 - discountPercent) / 100) - discount;

            BasePrice.Set(context, price);
            TotalPrice.Set(context, totalPrice);
        }
    }
}
