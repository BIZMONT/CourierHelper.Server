using CourierHelper.DataAccess.Enums;
using System;

namespace CourierHelper.DataAccess.Entities
{
    public class Order
    {
        public long Id { get; set; }

        public OrderState State { get; set; }


        #region Relations
        public virtual Customer Sender { get; set; }

        public virtual Customer Receiver { get; set; }

        public virtual Warehouse Warehouse { get; set; }

        public virtual Courier Courier { get; set; }

        public virtual ActivePoint Destination { get; set; }
        #endregion
    }
}
