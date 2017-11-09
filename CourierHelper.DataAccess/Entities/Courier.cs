using CourierHelper.DataAccess.Enums;
using System;
using System.Collections.Generic;

namespace CourierHelper.DataAccess.Entities
{
    public class Courier
    {
        public Guid Id { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string MiddleName { get; set; }

		public string Email { get; set; }
		public string PhoneNumber { get; set; }

        public CourierState State { get; set; }

		public DateTime? Created { get; set; } = DateTime.Now;
		public DateTime? Edited { get; set; }  = DateTime.Now;
		public DateTime? Deleted { get; set; }


		#region Relations
		public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public virtual ActivePoint Location { get; set; }

        public virtual ICollection<Route> Routes { get; set; } = new List<Route>();

		public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
		#endregion
	}
}
