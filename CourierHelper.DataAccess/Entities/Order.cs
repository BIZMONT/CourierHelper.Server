using CourierHelper.DataAccess.Enums;
using System;

namespace CourierHelper.DataAccess.Entities
{
    public class Order
    {
        public long Id { get; set; }

        public OrderState State { get; set; }
		public string Address { get; set; }
		public string Content { get; set; }
		public string Comment { get; set; }

		public DateTime? Created { get; set; } = DateTime.Now;
		public DateTime? Edited { get; set; } = DateTime.Now;
		public DateTime? Deleted { get; set; }
		public DateTime? Synced { get; set; }


		#region Relations
		public virtual Customer Sender { get; set; }

		public virtual Customer Receiver { get; set; }
		
        public virtual Warehouse Warehouse { get; set; }
		
        public virtual Courier Courier { get; set; }
		
        public virtual ActivePoint Destination { get; set; }
        #endregion
    }
}
