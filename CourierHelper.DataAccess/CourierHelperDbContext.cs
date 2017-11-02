using CourierHelper.DataAccess.Entities;
using CourierHelper.DataAccess.Entities.Configurations;
using System.Data.Entity;

namespace CourierHelper.DataAccess
{
	public class CourierHelperDbContext : DbContext
	{
		public DbSet<Order> Orders { get; set; }
		public DbSet<Courier> Couriers { get; set; }
		public DbSet<Warehouse> Warehouses { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Route> Routes { get; set; }
		public DbSet<ActivePoint> ActivePoints { get; set; }

		public CourierHelperDbContext(string connectionString) : base(connectionString) { }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			#region Entities configs
			modelBuilder.Configurations.Add(new OrderEntityConfiguration());
			modelBuilder.Configurations.Add(new CourierEntityConfiguration());
			modelBuilder.Configurations.Add(new WarehouseEntityConfiguration());
			modelBuilder.Configurations.Add(new CustomerEntityConfiguration());
			modelBuilder.Configurations.Add(new ActivePointEntityConfiguration());
			modelBuilder.Configurations.Add(new RouteEntityConfiguration());
			#endregion

			#region Entities relations

			#region Courier relations
			modelBuilder.Entity<Courier>()
				.HasOptional(c => c.Location)
				.WithOptionalPrincipal(ap => ap.Courier);

			modelBuilder.Entity<Courier>()
				.HasMany(c => c.Orders)
				.WithOptional(o => o.Courier);

			modelBuilder.Entity<Courier>()
				.HasOptional(c => c.ActiveRoute)
				.WithRequired(r => r.Courier)
				.Map(m => m.MapKey("CourierId"));
			#endregion

			#region Order relations
			modelBuilder.Entity<Order>()
				.HasRequired(o => o.Warehouse)
				.WithMany(w => w.Orders)
				.HasForeignKey(o => o.WarehouseId);

			modelBuilder.Entity<Order>()
				.HasRequired(o => o.Receiver)
				.WithMany(r => r.OrdersAsReceiver)
				.HasForeignKey(o => o.ReceiverId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Order>()
				.HasRequired(o => o.Sender)
				.WithMany(r => r.OrdersAsSender)
				.HasForeignKey(o => o.SenderId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Order>()
				.HasRequired(o => o.Destination)
				.WithOptional(ap => ap.Order)
				.Map(m => m.MapKey("DestinationId"));
			#endregion

			#region Warehouse relations
			modelBuilder.Entity<Warehouse>()
				.HasRequired(w => w.Location)
				.WithOptional(ap => ap.Warehouse)
				.Map(m => m.MapKey("LocationId"));

			#endregion

			modelBuilder.Entity<Route>()
				.HasMany(r => r.Points)
				.WithOptional(ap => ap.Route)
				.HasForeignKey(ap => ap.RouteId);
			#endregion

			base.OnModelCreating(modelBuilder);
		}
	}
}
