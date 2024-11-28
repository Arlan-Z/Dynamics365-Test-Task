using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;

namespace AwaraIT.Training.Domain.Models.Crm.Entities
{
    [EntityLogicalName(EntityLogicalName)]
    public class ShoppingCart : BaseEntity
    {
        public ShoppingCart() : base(EntityLogicalName) { }
        public const string EntityLogicalName = "arl_shoppingcart";

        public static class Metadata
        {
            public const string ProductId = "arl_productsid";
            public const string DealId = "arl_possibledealid";
            public const string Price = "arl_basePrice";
            public const string TotalPrice = "arl_totalPrice";
            public const string Discount = "arl_discpount";
            public const string DiscountPercent = "arl_discountpercent";
        }
    
        public EntityReference ProductId
        {
            get { return GetAttributeValue<EntityReference>(Metadata.ProductId); }
            set { Attributes[Metadata.ProductId] = value; }
        }

        public EntityReference DealId
        {
            get { return GetAttributeValue<EntityReference>(Metadata.DealId); }
            set { Attributes[Metadata.DealId] = value; }
        }

        public Double Price
        {
            get { return GetAttributeValue<Double?>(Metadata.Price) ?? 0.0; }
            set { Attributes[Metadata.Price] = value; }
        }

        public Double TotalPrice
        {
            get { return GetAttributeValue<Double?>(Metadata.TotalPrice) ?? 0.0; }
            set { Attributes[Metadata.TotalPrice] = value; }
        }

        public Double Discount
        {
            get { return GetAttributeValue<Double?>(Metadata.Discount) ?? 0.0; }
            set { Attributes[Metadata.Discount] = value; }
        }

        public Decimal DiscountPercent
        {
            get { return GetAttributeValue<decimal?>(Metadata.DiscountPercent) ?? 0m; }
            set { Attributes[Metadata.DiscountPercent] = value; }
        }
    }

}
