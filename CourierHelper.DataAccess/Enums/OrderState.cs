using System;
using System.Collections.Generic;
using System.Text;

namespace CourierHelper.DataAccess.Enums
{
    public enum OrderState
    {
        NotIntended,
        WaitingOnWarehouse,
        Fulfillment,
        Completed
    }
}
