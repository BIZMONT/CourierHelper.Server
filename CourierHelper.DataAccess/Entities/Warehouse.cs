using System;
using System.Collections.Generic;

namespace CourierHelper.DataAccess.Entities
{
    public class Warehouse
    {
        public int Id { get; set; }


        #region Relations
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

		public Guid LocationId { get; set; }
        public virtual ActivePoint Location { get; set; }
        #endregion
    }
}
