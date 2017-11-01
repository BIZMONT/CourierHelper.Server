using CourierHelper.DataAccess.Enums;
using System;
using System.Collections.Generic;

namespace CourierHelper.DataAccess.Entities
{
    public class Courier
    {
        public Guid Id { get; set; }

        public CourierState State { get; set; }


        #region Relations
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public virtual ActivePoint Location { get; set; }

        public virtual Route ActiveRoute { get; set; }
        #endregion
    }
}
