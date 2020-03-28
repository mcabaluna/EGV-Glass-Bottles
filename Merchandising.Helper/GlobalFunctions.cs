using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Merchandising.Enums;
using Merchandising.DTO;
namespace Merchandising.Helper
{
    public static class GlobalFunctions
    {
        public static string GetAccessRoleValue(int? value)
        {
            string role = string.Empty;
            switch (value.ToString())
            {
                case "0":
                    role = AccessRoles.ADMIN.ToString();
                    break;
                case "1":
                    role = AccessRoles.OWNER.ToString();
                    break;
                case "2":
                    role = AccessRoles.INVENTORY.ToString();
                    break;
                case "3":
                    role = AccessRoles.CASHIER.ToString();
                    break;
                case "4":
                    role = AccessRoles.PURCHASER.ToString();
                    break;
            }
            return role;
        }
        public static string GetStatusValue(int? value)
        {
            string status = string.Empty;
            switch (value.ToString())
            {
                case "1":
                    status = StatusType.ACTIVE.ToString();
                    break;
                case "0":
                    status = StatusType.IN_ACTIVE.ToString();
                    break;
            }
            return status;
        }
        public static string GetInvoiceStatusValue(int? value)
        {
            string status = string.Empty;
            switch (value.ToString())
            {
                case "2":
                    status = InvoiceStatus.CANCELLED.ToString();
                    break;
                case "1":
                    status = InvoiceStatus.CLOSED.ToString();
                    break;
                case "0":
                    status = InvoiceStatus.OPEN.ToString();
                    break;
            }
            return status;
        }
        public static string GetVatValue(int? value)
        {
            string status = string.Empty;
            switch (value.ToString())
            {
                case "1":
                    status = VatType.OUTPUT_VAT.ToString();
                    break;
                case "0":
                    status = VatType.INPUT_VAT.ToString();
                    break;
            }
            return status;
        }
        public static string GetWTaxValue(int? value)
        {
            string status = string.Empty;
            switch (value.ToString())
            {
                case "0":
                    status = WTaxType.CREDITABLE_WITHHOLDING.ToString();
                    break;
                case "1":
                    status = WTaxType.EXPANDED_WITHHOLDING.ToString();
                    break;
            }
            return status;
        }
        public static string GetBPGroupValue(int? value)
        {
            string status = string.Empty;
            switch (value.ToString())
            {
                case "1":
                    status = BpType.SUPPLIER.ToString();
                    break;
                case "0":
                    status = BpType.CUSTOMER.ToString();
                    break;
            }
            return status;
        }
        public static string GetTransStatusValue(int? value)
        {
            string status = string.Empty;
            switch (value.ToString())
            {
                case "0":
                    status = InvoiceType.FULLYPAID.ToString();
                    break;
                case "1":
                    status = InvoiceType.PARTIALLY_PAID.ToString();
                    break;
                case "2":
                    status = InvoiceType.UNPAID.ToString();
                    break;
                case "3":
                    status = InvoiceType.CANCELED.ToString();
                    break;
            }
            return status;
        }
        public static string GetAdjustmentTypeValue(int? value)
        {
            string status = string.Empty;
            switch (value.ToString())
            {
                case "0":
                    status = AdjustmentType.STOCK_IN.ToString();
                    break;
                case "1":
                    status = AdjustmentType.STOCK_OUT.ToString();
                    break;
            }
            return status;
        }
        //public static string GetDefaultSeries(int defaultseries, int objectcode)
        //{
        //    var result = string.Empty;
        //    result = db.SequenceDocument.ToList()
        //}
    }
}
