using System;

namespace Merchandising.Enums
{
    [Flags]
    public enum InvoiceType
    {
        FULLYPAID = 0,
        PARTIALLY_PAID = 1,
        UNPAID = 2,
        CANCELED = 3
    }
}
