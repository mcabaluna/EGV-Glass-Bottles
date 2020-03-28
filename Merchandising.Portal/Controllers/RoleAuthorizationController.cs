using Merchandising.DTO.Models;
using Merchandising.Portal.Models;
using Merchandising.VM.Portal;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Merchandising.Portal.Controllers
{
    public class RoleAuthorizationController : Controller
    {
        // GET: RoleAuthorization
        public ActionResult Index()
        {
            var objroles = MerchandisingApiWrapper.Get<List<Roles>>(
                        typeof(Roles).Name + "/getrolesinfo");

            var roles = objroles.Select(y => new { y.RoleId, y.RoleName }).Distinct().ToList();
            IEnumerable<SelectListItem> rolelist =
                from s in roles
                select new SelectListItem
                {
                    Text = s.RoleName,
                    Value = s.RoleId.ToString()
                };
            RoleAuthorizationVM authVM = new RoleAuthorizationVM()
            {
                RolesOption = new SelectList(rolelist, "Value", "Text"),
                RoleMenus = new List<RoleMenus>(),
                RolePage = new List<RolePage>()
            };
            return View(authVM);
        }

        public ActionResult GetRoleAuthorization(string id)
        {
            RoleAuthorization auth = new RoleAuthorization();
            var obj = MerchandisingApiWrapper.Get<RoleAuthorization>(
                                  typeof(RoleAuthorization).Name + $"/{id}");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDefaultAuthorization(string id)
        {
            RoleAuthorization auth = new RoleAuthorization();

            //var objroles = MerchandisingApiWrapper.Get<List<Roles>>(
            //          typeof(Roles).Name + "/getrolesinfo");

            List<RoleMenus> rolemenusList = new List<RoleMenus>();
            List<RolePage> rolepageList = new List<RolePage>();
            rolemenusList.Add(new RoleMenus() { RoleId = id, MenuName = "Administration", SubMenuName = "Roles", Visible = true, Status = true });
            rolemenusList.Add(new RoleMenus() { RoleId = id, MenuName = "Administration", SubMenuName = "Use Authorization", Visible = true, Status = true });
            rolemenusList.Add(new RoleMenus() { RoleId = id, MenuName = "Administration", SubMenuName = "Series", Visible = true, Status = true });
            rolemenusList.Add(new RoleMenus() { RoleId = id, MenuName = "Administration", SubMenuName = "Users", Visible = true, Status = true });
            rolemenusList.Add(new RoleMenus() { RoleId = id, MenuName = "Administration", SubMenuName = "Branch", Visible = true, Status = true });
            rolemenusList.Add(new RoleMenus() { RoleId = id, MenuName = "Administration", SubMenuName = "VAT", Visible = true, Status = true });
            rolemenusList.Add(new RoleMenus() { RoleId = id, MenuName = "Administration", SubMenuName = "WTax", Visible = true, Status = true });
            rolemenusList.Add(new RoleMenus() { RoleId = id, MenuName = "Administration", SubMenuName = "Mode of Payment", Visible = true, Status = true });
            rolemenusList.Add(new RoleMenus() { RoleId = id, MenuName = "Administration", SubMenuName = "BP Group", Visible = true, Status = true });
            rolemenusList.Add(new RoleMenus() { RoleId = id, MenuName = "Administration", SubMenuName = "Payment Terms", Visible = true, Status = true });
            rolemenusList.Add(new RoleMenus() { RoleId = id, MenuName = "Administration", SubMenuName = "UoM", Visible = true, Status = true });
            rolemenusList.Add(new RoleMenus() { RoleId = id, MenuName = "Administration", SubMenuName = "Item Group", Visible = true, Status = true });
            rolemenusList.Add(new RoleMenus() { RoleId = id, MenuName = "Administration", SubMenuName = "Warehouse", Visible = true, Status = true });
            rolemenusList.Add(new RoleMenus() { RoleId = id, MenuName = "Administration", SubMenuName = "Pricelist", Visible = true, Status = true });
            rolemenusList.Add(new RoleMenus() { RoleId = id, MenuName = "MasterLists", SubMenuName = "Business Partner", Visible = true, Status = true });
            rolemenusList.Add(new RoleMenus() { RoleId = id, MenuName = "MasterLists", SubMenuName = "Items", Visible = true, Status = true });
            rolemenusList.Add(new RoleMenus() { RoleId = id, MenuName = "Transactions", SubMenuName = "Sales Invoice", Visible = true, Status = true });
            rolemenusList.Add(new RoleMenus() { RoleId = id, MenuName = "Transactions", SubMenuName = "Purchase Invoice", Visible = true, Status = true });
            rolemenusList.Add(new RoleMenus() { RoleId = id, MenuName = "Transactions", SubMenuName = "Inventory Adjustment", Visible = true, Status = true });
            rolemenusList.Add(new RoleMenus() { RoleId = id, MenuName = "Payments", SubMenuName = "Collection", Visible = true, Status = true });
            rolemenusList.Add(new RoleMenus() { RoleId = id, MenuName = "Payments", SubMenuName = "Payments", Visible = true, Status = true });
            rolemenusList.Add(new RoleMenus() { RoleId = id, MenuName = "Reports", SubMenuName = "Total Sales", Visible = true, Status = true });
            rolemenusList.Add(new RoleMenus() { RoleId = id, MenuName = "Reports", SubMenuName = "Total Purchases", Visible = true, Status = true });
            rolemenusList.Add(new RoleMenus() { RoleId = id, MenuName = "Reports", SubMenuName = "Items on Hand", Visible = true, Status = true });
            rolemenusList.Add(new RoleMenus() { RoleId = id, MenuName = "Reports", SubMenuName = "BP Balances", Visible = true, Status = true });
            #region "Default"
            //foreach (var role in objroles)
            //{
            //    switch (role.RoleId.ToUpper())
            //    {
            //        #region "Default Role Menu and Page"
            //        case id:
            //            // role menu
            //            //rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Administration", SubMenuName = "Roles", Visible = true, Status = true });
            //            //rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Administration", SubMenuName = "Use Authorization", Visible = true, Status = true });
            //            //rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Administration", SubMenuName = "Series", Visible = true, Status = true });
            //            //rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Administration", SubMenuName = "users", Visible = true, Status = true });
            //            //rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Administration", SubMenuName = "Branch", Visible = true, Status = true });
            //            //rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Administration", SubMenuName = "VAT", Visible = true, Status = true });
            //            //rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Administration", SubMenuName = "WTax", Visible = true, Status = true });
            //            //rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Administration", SubMenuName = "Mode of Payment", Visible = true, Status = true });
            //            //rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Administration", SubMenuName = "BP Group", Visible = true, Status = true });
            //            //rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Administration", SubMenuName = "Payment Terms", Visible = true, Status = true });
            //            //rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Administration", SubMenuName = "UoM", Visible = true, Status = true });
            //            //rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Administration", SubMenuName = "Item Group", Visible = true, Status = true });
            //            //rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Administration", SubMenuName = "Warehouse", Visible = true, Status = true });
            //            //rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Administration", SubMenuName = "Pricelist", Visible = true, Status = true });
            //            //rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "MasterLists", SubMenuName = "Business Partner", Visible = true, Status = true });
            //            //rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "MasterLists", SubMenuName = "Items", Visible = true, Status = true });
            //            //rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Transactions", SubMenuName = "Sales Invoice", Visible = true, Status = true });
            //            //rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Transactions", SubMenuName = "Purchase Invoice", Visible = true, Status = true });
            //            //rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Transactions", SubMenuName = "Inventory Adjustment", Visible = true, Status = true });
            //            //rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Payments", SubMenuName = "Collection", Visible = true, Status = true });
            //            //rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Payments", SubMenuName = "Payments", Visible = true, Status = true });
            //            //rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Reports", SubMenuName = "Total Sales", Visible = true, Status = true });
            //            //rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Reports", SubMenuName = "Total Purchases", Visible = true, Status = true });
            //            //rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Reports", SubMenuName = "Items on Hand", Visible = true, Status = true });
            //            //rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Reports", SubMenuName = "BP Balances", Visible = true, Status = true });

            //            //role page
            //            //rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Reports", SubMenuName = "BP Balances", Visible = true, Status = true });

            //            break;
            //        //case "OWNER":
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Reports", SubMenuName = "Total Sales", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Reports", SubMenuName = "Total Purchases", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Reports", SubMenuName = "Items on Hand", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Reports", SubMenuName = "BP Balances", Visible = true, Status = true });
            //        //    break;
            //        //case "INVENTORY":
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "MasterLists", SubMenuName = "Items", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Transactions", SubMenuName = "Inventory Adjustment", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Reports", SubMenuName = "Items on Hand", Visible = true, Status = true });
            //        //    break;
            //        //case "PURCHASING":
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "MasterLists", SubMenuName = "Items", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Transactions", SubMenuName = "Purchase Invoice", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Transactions", SubMenuName = "Inventory Adjustment", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Reports", SubMenuName = "Items on Hand", Visible = true, Status = true });
            //        //    break;
            //        //case "USER":
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Transactions", SubMenuName = "Sales Invoice", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Transactions", SubMenuName = "Purchase Invoice", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Transactions", SubMenuName = "Inventory Adjustment", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Payments", SubMenuName = "Collection", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Payments", SubMenuName = "Payments", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Reports", SubMenuName = "Total Sales", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Reports", SubMenuName = "Total Purchases", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Reports", SubMenuName = "Items on Hand", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Reports", SubMenuName = "BP Balances", Visible = true, Status = true });

            //        //    break;
            //        //case "STAFF":
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Transactions", SubMenuName = "Sales Invoice", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Transactions", SubMenuName = "Purchase Invoice", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Transactions", SubMenuName = "Inventory Adjustment", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Reports", SubMenuName = "Total Sales", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Reports", SubMenuName = "Total Purchases", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Reports", SubMenuName = "Items on Hand", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Reports", SubMenuName = "BP Balances", Visible = true, Status = true });
            //        //    break;
            //        //case "CASHIER":
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Payments", SubMenuName = "Collection", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Payments", SubMenuName = "Payments", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Reports", SubMenuName = "Total Sales", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Reports", SubMenuName = "Total Purchases", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Reports", SubMenuName = "Items on Hand", Visible = true, Status = true });
            //        //    rolemenusList.Add(new RoleMenus() { RoleId = role.RoleId, MenuName = "Reports", SubMenuName = "BP Balances", Visible = true, Status = true });
            //        //    break;
            //            #endregion
            //    }
            //}

            #endregion

            auth = new RoleAuthorization()
            {
                ListOfRoleMenus = rolemenusList.ToList()
            };
            return Json(auth, JsonRequestBehavior.AllowGet);
        }
    }
}