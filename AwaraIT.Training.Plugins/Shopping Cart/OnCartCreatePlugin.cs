using AwaraIT.Training.Application.Core;
using AwaraIT.Training.Domain.Extensions;
using AwaraIT.Training.Domain.Models.Crm.Entities;
using AwaraIT.Training.Plugins.PluginExtensions;
using AwaraIT.Training.Plugins.PluginExtensions.Enums;
using AwaraIT.Training.Plugins.PluginExtensions.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace AwaraIT.Arlan.Plugins.Shopping_Cart
{
    public class OnCartCreatePlugin : PluginBase
    {
        public OnCartCreatePlugin()
        {
            Subscribe
            .ToMessage(CrmMessage.Create)
            .ForEntity(ShoppingCart.EntityLogicalName)
            .When(PluginStage.PostOperation)
            .Execute(Execute);
        }

        private void Execute(IContextWrapper wrapper)
        {
            var logger = new Logger(wrapper.Service);
            try
            {
                var cart = wrapper.TargetEntity.ToEntity<ShoppingCart>();

                // Set Condition Values
                var query_deal_id = cart.DealId.Id;

                // Instantiate QueryExpression query
                var cartQuery = new QueryExpression("arl_shoppingcart");
                cartQuery.TopCount = 10000;

                // Add columns to query.ColumnSet
                cartQuery.ColumnSet.AddColumns("arl_baseprice", "arl_totalprice");

                // Add conditions to query.Criteria
                cartQuery.Criteria.AddCondition("arl_possibledeal", "arl_possibledealid", ConditionOperator.Equal, query_deal_id);

                // Add link-entity query_arl_possibledeal
                cartQuery.AddLink("arl_possibledeal", "arl_possibledealid", "arl_possibledealid");

                var cartsRes = wrapper.Service.RetrieveMultiple(cartQuery);

                double basePrice = 0.0, totalPrice = 0.0, totalDiscount = 0.0;

                foreach(var cartEntity in cartsRes.Entities)
                {
                    double basePriceBuf = cartEntity.GetAttributeValue<Double?>("arl_baseprice") ?? 0.0;
                    double totalPriceBuf = cartEntity.GetAttributeValue<Double?>("arl_totalprice") ?? 0.0;

                    basePrice += basePriceBuf;
                    totalPrice += totalPriceBuf;
                }

                var deal = wrapper.Service.Retrieve("arl_possibledeal", query_deal_id, 
                    new ColumnSet("arl_baseprice", "arl_totalprice", "arl_totaldiscount"));

                deal["arl_baseprice"] = basePrice;
                deal["arl_totalprice"] = totalPrice;
                deal["arl_totaldiscount"] = (Decimal)(basePrice - totalPrice);

                wrapper.Service.Update(deal);
            }
            catch (Exception ex)
            {
                logger.ERROR("Arlan.OnCartCreatePlugin",
                    ex.ToString(), wrapper.TargetEntity.LogicalName, wrapper.TargetEntity.Id);
            }
        }

    }
}
