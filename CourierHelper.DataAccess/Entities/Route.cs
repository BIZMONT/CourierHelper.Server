using System;
using System.Collections.Generic;

namespace CourierHelper.DataAccess.Entities
{
    public class Route
    {
        public Guid Id { get; set; }


        #region Relations
        public virtual List<ActivePoint> Points { get; set; } = new List<ActivePoint>();

        public virtual Courier Courier { get; set; }
        #endregion
    }
}
