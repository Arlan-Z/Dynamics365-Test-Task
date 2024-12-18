﻿using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using static AwaraIT.Training.Domain.Models.Crm.Entities.Deal.Metadata;

namespace AwaraIT.Training.Domain.Models.Crm.Entities
{
    [EntityLogicalName(EntityLogicalName)]
    public class Deal : BaseEntity
    {
        public Deal() : base(EntityLogicalName) { }
        public const string EntityLogicalName = "arl_possibledeal";

        public static class Metadata
        {
            public const string RegionId = "arl_regionid";
            public const string ContactId = "arl_contactid";
            public const string ShoppingCartId = "arl_shoppingcardid";
            public const string Status = "arl_status";
            public const string Discount = "arl_totaldiscount";
            public const string Price = "arl_baseprice";
            public const string TotalPrice = "arl_totalprice";

            public enum DealStatusOptions
            {
                [Description("Открыта")]
                Новый = 1,

                [Description("В работе")]
                В_работе = 2,

                [Description("Выиграна")]
                Согласие = 3,
            }
        }

        public EntityReference RegionId
        {
            get { return GetAttributeValue<EntityReference>(Metadata.RegionId); }
            set { Attributes[Metadata.RegionId] = value; }
        }

        public EntityReference ContactId
        {
            get { return GetAttributeValue<EntityReference>(Metadata.ContactId); }
            set { Attributes[Metadata.ContactId] = value; }
        }

        public EntityReference ShoppingCartId
        {
            get { return GetAttributeValue<EntityReference>(Metadata.ShoppingCartId); }
            set { Attributes[Metadata.ShoppingCartId] = value; }
        }

        public DealStatusOptions? Status
        {
            get { return (DealStatusOptions?)GetAttributeValue<OptionSetValue>(Metadata.Status)?.Value; }
            set { Attributes[Metadata.ContactId] = value != null ? new OptionSetValue((int)value.Value) : new OptionSetValue((int)DealStatusOptions.Новый); }
        }

        public Decimal Discount
        {
            get { return GetAttributeValue<Decimal>(Metadata.Discount); }
            set { Attributes[Metadata.Discount] = value; }
        }

        public Double Price
        {
            get { return GetAttributeValue<Double>(Metadata.Price); }
            set { Attributes[Metadata.Price] = value; }
        }

        public Double TotalPrice
        {
            get { return GetAttributeValue<Double>(Metadata.TotalPrice); }
            set { Attributes[Metadata.TotalPrice] = value; }
        }
    }

}
