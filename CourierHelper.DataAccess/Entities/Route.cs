﻿using System;
using System.Collections.Generic;

namespace CourierHelper.DataAccess.Entities
{
    public class Route
    {
        public Guid Id { get; set; }
		 
		public double Distance { get; set; }

        #region Relations
        public virtual ICollection<ActivePoint> Points { get; set; } = new List<ActivePoint>();

		public Guid CourierId { get; set; }
        public virtual Courier Courier { get; set; }
        #endregion
    }
}
