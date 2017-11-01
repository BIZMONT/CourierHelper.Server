using System;
using System.Collections.Generic;
using System.Text;

namespace CourierHelper.DataAccess.Enums
{
    public enum CourierState
    {
		NotAccessible,
        Idle,
        OnWarehouse,
        PerformsDelivery,
        AtDestinationPoint
    }
}
