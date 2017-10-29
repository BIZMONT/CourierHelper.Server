﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CourierHelper.DataAccess.Enums
{
    public enum OrderState
    {
        NotAssigned,
        WaitingOnWarehouse,
        Fulfillment,
        Completed
    }
}
