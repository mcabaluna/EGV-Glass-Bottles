
namespace Merchandising.Enums
{
    using System;

    [Flags]
    public enum AccessRoles
    {
        ADMIN = 0,
        OWNER = 1,
        INVENTORY = 2,
        CASHIER= 3,
        PURCHASER = 4
    }
}
