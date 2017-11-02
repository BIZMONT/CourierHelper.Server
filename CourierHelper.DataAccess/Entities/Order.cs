using CourierHelper.DataAccess.Enums;
using System;

namespace CourierHelper.DataAccess.Entities
{
    public class Order
    {
        public long Id { get; set; }

        public OrderState State { get; set; }

		public DateTime? Deleted { get; set; }


        #region Relations
		public Guid SenderId { get; set; }
        public virtual Customer Sender { get; set; }

		public Guid ReceiverId { get; set; }
		public virtual Customer Receiver { get; set; }

		public int WarehouseId { get; set; }
        public virtual Warehouse Warehouse { get; set; }

		public Guid CourierId { get; set; }
        public virtual Courier Courier { get; set; }

		public Guid DestinationId { get; set; }
        public virtual ActivePoint Destination { get; set; }
        #endregion
    }
}
