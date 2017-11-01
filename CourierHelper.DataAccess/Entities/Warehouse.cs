using System;
using System.Collections.Generic;

namespace CourierHelper.DataAccess.Entities
{
    public class Warehouse
    {
        public int Id { get; set; }


        #region Relations
        public virtual IEnumerable<Order> Orders { get; set; } = new List<Order>();

        public virtual ActivePoint Location { get; set; }
        #endregion
    }
}
