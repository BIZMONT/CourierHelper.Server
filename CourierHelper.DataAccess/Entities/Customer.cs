using System;
using System.Collections.Generic;

namespace CourierHelper.DataAccess.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }

		public DateTime? Deleted { get; set; }


        #region Relations
        public virtual ICollection<Order> OrdersAsSender { get; set; } = new List<Order>();
        public virtual ICollection<Order> OrdersAsReceiver { get; set; } = new List<Order>();
        #endregion
    }
}
