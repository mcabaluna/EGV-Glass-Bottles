using System;

namespace Merchandising.Enums
{
    [Flags]
    public enum WTaxType
    {
        CREDITABLE_WITHHOLDING = 0,
        EXPANDED_WITHHOLDING = 1
    }
}
