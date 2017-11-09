using System;
using System.Collections.Generic;

namespace CourierHelper.DataAccess.Entities
{
    public class Route
    {
        public Guid Id { get; set; }
		 
		public bool IsCurrent { get; set; }
		public double Distance { get; set; }

		public DateTime? Created { get; set; } = DateTime.Now;
		public DateTime? Edited { get; set; } = DateTime.Now;
		public DateTime? Completed { get; set; }
		public DateTime? Synced { get; set; }

        #region Relations
        public virtual ICollection<ActivePoint> Points { get; set; } = new List<ActivePoint>();

        public virtual Courier Courier { get; set; }
        #endregion
    }
}
