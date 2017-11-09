using System;
using System.Collections.Generic;

namespace CourierHelper.DataAccess.Entities
{
    public class Warehouse
    {
        public int Id { get; set; }

		public string Name { get; set; }
		public string Address { get; set; }

		public DateTime? Created { get; set; } = DateTime.Now;
		public DateTime? Edited { get; set; } = DateTime.Now;
		public DateTime? Deleted { get; set; }


        #region Relations
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public virtual ActivePoint Location { get; set; }
        #endregion
    }
}
