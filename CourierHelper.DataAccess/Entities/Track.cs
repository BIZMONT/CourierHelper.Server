using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHelper.DataAccess.Entities
{
	public class Track
	{
		public Guid Id { get; set; }

		public bool IsCurrent { get; set; }

		public DateTime? Created { get; set; } = DateTime.Now;
		public DateTime? Edited { get; set; } = DateTime.Now;
		public DateTime? Deleted { get; set; }

		public virtual ICollection<ActivePoint> Points { get; set; } = new List<ActivePoint>();
		public virtual Order Order { get; set; }
		public virtual Courier Courier { get; set; }
	}
}
