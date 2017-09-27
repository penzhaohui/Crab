using System;
using System.Collections.Generic;
using System.Text;
using Crab.DataModel.Common;
using Crab.Runtime.Contract;
using System.Runtime.Serialization;

namespace Crab.Business.Contract
{
    [DataContract, Serializable, EntityClass("ShippingExportContract")]
    public class ShippingExportDC: ExtensibleDC
    {
        /// <summary>
        /// Gets or sets the unique identifier of shipping export contract
        /// </summary>
        public Guid Id
        {
            get { return GetValue<Guid>("Id"); }
            set { SetValue<Guid>("Id", value); }
        }

        /// <summary>
        /// Gets or sets the tenant ID
        /// </summary>
        public Guid TenantId
        {
            get { return GetValue<Guid>("TenantId"); }
            set { SetValue<Guid>("TenantId", value); }
        }

        /// <summary>
        /// Gets or sets the No. of the shipping export contract
        /// </summary>
        public string Number
        {
            get { return GetValue<string>("Number"); }
            set { SetValue<string>("Number", value); }
        }

        public Guid? WorkflowId
        {
            get { return GetValue<Guid?>("WorkflowId"); }
            set { SetValue<Guid?>("WorkflowId", value); }
        }

        public Guid? Creator
        {
            get { return GetValue<Guid?>("Creator"); }
            set { SetValue<Guid?>("Creator", value); }
        }

        public DateTime? CreateDate
        {
            get { return GetValue<DateTime?>("CreateDate"); }
            set { SetValue<DateTime?>("CreateDate", value); }
        }

        public Guid? Shipper
        {
            get { return GetValue<Guid?>("Shipper"); }
            set { SetValue<Guid?>("Shipper", value); }
        }

        public Guid? Consignee
        {
            get { return GetValue<Guid?>("Consignee"); }
            set { SetValue<Guid?>("Consignee", value); }
        }

        public Guid? NotifyPart
        {
            get { return GetValue<Guid?>("NotifyPart"); }
            set { SetValue<Guid?>("NotifyPart", value); }
        }

        public string ExportSite
        {
            get { return GetValue<string>("ExportSite"); }
            set { SetValue<string>("ExportSite", value); }
        }

        public string Destination
        {
            get { return GetValue<string>("Destination"); }
            set { SetValue<string>("Destination", value); }
        }

        public bool? Batch
        {
            get { return GetValue<bool?>("Batch"); }
            set { SetValue<bool?>("Batch", value); }
        }

        public bool? Reship
        {
            get { return GetValue<bool?>("Reship"); }
            set { SetValue<bool?>("Reship", value); }
        }

        public string CreditId
        {
            get { return GetValue<string>("CreditId"); }
            set { SetValue<string>("CreditId", value); }
        }

        public string PaymentMethod
        {
            get { return GetValue<string>("PaymentMethod"); }
            set { SetValue<string>("PaymentMethod", value); }
        }

        public decimal? Amount
        {
            get { return (decimal?)GetValue<decimal?>("Amount"); }
            set { SetValue<decimal?>("Amount", value); }
        }

        public string Description
        {
            get { return GetValue<string>("Description"); }
            set { SetValue<string>("Description", value); }
        }

        #region Cargo information
        public string ShippingMarks
        {
            get { return GetValue<string>("ShippingMarks"); }
            set { SetValue<string>("ShippingMarks", value); }
        }

        public string ProductName
        {
            get { return GetValue<string>("ProductName"); }
            set { SetValue<string>("ProductName", value); }
        }

        public decimal? Quantity
        {
            get { return GetValue<decimal?>("Quantity"); }
            set { SetValue<decimal?>("Quantity", value); }
        }

        public decimal? Capacity
        {
            get { return GetValue<decimal?>("Capacity"); }
            set { SetValue<decimal?>("Capacity", value); }
        }

        public decimal? Gross
        {
            get { return GetValue<decimal?>("Gross"); }
            set { SetValue<decimal?>("Gross", value); }
        }

        public decimal? Net
        {
            get { return GetValue<decimal?>("Net"); }
            set { SetValue<decimal?>("Net", value); }
        }
        #endregion
    }
}
