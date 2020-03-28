using Merchandising.DTO;
using Merchandising.Helper;
using Merchandising.VM.Portal;
using Merchandising.VM.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;

namespace Merchandising.Api.Controllers.Portal
{
    public class DashboardController : ApiController
    {
        private DbContextModel db = new DbContextModel();

        /// <summary>
        /// GetDashboard
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/dashboard/getdashboard")]
        public IHttpActionResult GetDashboard()
        {
            decimal salespertoday = 0m;
            decimal collectionfortoday = 0m;
            decimal totalpermonth = 0m;
            decimal monthlycollection = 0m;

            if (db.SalesInvoices.Any(x => x.Date.Month == DateTime.Now.Month
                                        && x.Date.Year == DateTime.Now.Year))
            {
                //Sales total per month
                totalpermonth = db.SalesInvoices.Where(x => x.Date.Month == DateTime.Now.Month
                                                           && x.Date.Year == DateTime.Now.Year).Sum(b => b.GrossTotal);
            }

            if (db.Incomings.Any(x => x.DatePaid.Month == DateTime.Now.Month
                                                         && x.DatePaid.Year == DateTime.Now.Year && x.Lines.Any(b => b.InvType == "SI")))
            {
                //Monthly Collection
                monthlycollection = db.Incomings.Where(x => x.DatePaid.Month == DateTime.Now.Month
                                                        && x.DatePaid.Year == DateTime.Now.Year && x.Lines.Any(b => b.InvType == "SI")).DefaultIfEmpty().Sum(b => b.AmountPaid);
            }
            if (db.Incomings.Any(x => x.DatePaid.Day == DateTime.Now.Day
                                                        && x.DatePaid.Month == DateTime.Now.Month
                                                        && x.DatePaid.Year == DateTime.Now.Year && x.Lines.Any(b => b.InvType == "SI")))
            {
                //Collection for today
                collectionfortoday = db.Incomings.Where(x => x.DatePaid.Day == DateTime.Now.Day
                                                        && x.DatePaid.Month == DateTime.Now.Month
                                                        && x.DatePaid.Year == DateTime.Now.Year && x.Lines.Any(b => b.InvType == "SI")).Sum(b => b.AmountPaid);
            }
            if (db.SalesInvoices.Any(x => x.Date.Day == DateTime.Now.Day
                                                             && x.Date.Month == DateTime.Now.Month
                                                             && x.Date.Year == DateTime.Now.Year))
            {
                //Sales for today
                salespertoday = db.SalesInvoices.Where(x => x.Date.Day == DateTime.Now.Day
                                                                && x.Date.Month == DateTime.Now.Month
                                                                && x.Date.Year == DateTime.Now.Year).Sum(x => x.GrossTotal);
            }
            //Get all onhand per whse
            var onhandperwhse = (from n in db.ItemOnHandPerWhse.ToList()
                                 from a in db.Items.Where(o => o.ItemCode == n.ItemCode).ToList()
                                 from b in db.Warehouses.Where(o => o.Code == n.WhseId).ToList()
                                 select new ItemsPerWhse()
                                 {
                                     ItemCode = n.ItemCode,
                                     ItemName = a.ItemName,
                                     WhseCode = n.WhseId,
                                     WhseName = b.Name,
                                     OnHand = n.OnHand
                                 }
                                 ).ToList();

            //Get saleable items per branch
            var saleableitems = (from n in db.SalesInvoiceLines.ToList()
                                 from a in db.Warehouses.Where(x => x.Code == n.Whse).ToList()
                                 from b in db.Branches.Where(x => x.Code == a.BranchCode).ToList()
                                 group n by new { b.Code, b.Name, n.ItemCode, n.ItemName } into itg
                                 select new SaleableItems()
                                 {
                                     ItemCode = itg.Key.ItemCode,
                                     ItemName = itg.Key.ItemName,
                                     BranchCode = itg.Key.Code,
                                     BranchName = itg.Key.Name,
                                     SoldQty = itg.Sum(b => b.Quantity)
                                 }).OrderByDescending(b => b.SoldQty).ToList();

            //Daily Sales Invoice Count
            var invoicecount = db.SalesInvoices.Count(x => x.Date.Day == DateTime.Now.Day);

            //Get Warehouse
            var objwarehouse = db.Warehouses.ToList();
            var warehouse = objwarehouse.Select(y => new { y.Code, y.Name }).Distinct().ToList();
            warehouse.Add(new { Code = "All", Name = "Show All" });
            IEnumerable<SelectListItem> warehouselist =
                from s in warehouse
                select new SelectListItem
                {
                    Text = s.Code != "All" ? s.Code + " - " + s.Name : s.Name,
                    Value = s.Code,
                    Selected = s.Code != "All" ? false : true
                };
            //Get Branch
            var objbranch = db.Branches.ToList();
            var branch = objbranch.Select(y => new { y.Code, y.Name }).Distinct().ToList();
            branch.Add(new { Code = "All", Name = "Show All" });
            IEnumerable<SelectListItem> branchlist =
                from s in branch
                select new SelectListItem
                {
                    Text = s.Code != "All" ? s.Code + " - " + s.Name : s.Name,
                    Value = s.Code,
                    Selected = s.Code != "All" ? false : true
                };
            Dashboard_Results dashboard_Results = new Dashboard_Results()
            {
                TotalSalesForTheDay = salespertoday,
                Collectionfortoday = collectionfortoday,
                TotalSalesPerMonth = totalpermonth,
                MonthlyCollection = monthlycollection,
                DailySInvoiceCount = invoicecount,
                SaleableItems = saleableitems,
                ItemPerWhse = onhandperwhse,
                BranchOption = branchlist,
                WarehouseOption = warehouselist
            };
            return Ok(dashboard_Results);
        }
        /// <summary>
        /// CheckStatus
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/dashboard/checkstatus")]
        public IHttpActionResult CheckStatus(string transtype, int docentry)
        {
            bool status = false;
            string message = string.Empty;
            switch (transtype)
            {
                #region "transactions"
                case "PurchaseInvoices":
                    //status = db.SalesOrders.Where(s => s.DocEntry == docEntry && s.U_SAPDocEntry != 0 && s.U_SAPStatus == true).Any();
                    //message = db.SalesOrders.Where(s => s.DocEntry == docEntry).DefaultIfEmpty().FirstOrDefault().Status ?? "";
                    break;
                case "SalesInvoices":
                    status = db.SalesInvoices.Any(s => s.DocEntry == docentry);
                    message =GlobalFunctions.GetTransStatusValue((int)db.SalesInvoices.Where(s => s.DocEntry == docentry).DefaultIfEmpty().FirstOrDefault().Status) ?? "";
                    break;
                #endregion
                #region "Payments"
                case "Incomings":
                    //status = (from x in db.Incomings.Where(s => s.DocEntry == docEntry)
                    //          join b in db.IncomingLines on x.DocEntry equals b.DocEntry
                    //          where
                    //         b.U_SAPDocEntry != 0 && b.U_SAPStatus == true
                    //          select x).Any();
                    //status = db.Incomings.Where(s => s.DocEntry == docEntry && s.U_SAPDocEntry != 0 && s.U_SAPStatus == true).Any();
                    //message = db.Incomings.Where(s => s.DocEntry == docEntry).DefaultIfEmpty().FirstOrDefault().Remarks ?? "";
                    break;
                #endregion
            }
            CheckStatusVM checkStatus = new CheckStatusVM()
            {
                Status = status,
                Remarks = message
            };
            return Ok(checkStatus);
        }
    }
}
