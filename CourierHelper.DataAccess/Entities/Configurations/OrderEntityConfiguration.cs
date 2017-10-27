using System.Data.Entity.ModelConfiguration;

namespace CourierHelper.DataAccess.Entities.Configurations
{
    public class OrderEntityConfiguration : EntityTypeConfiguration<Order>
    {
        public OrderEntityConfiguration()
        {
            HasKey(order => order.Id);
        }
    }
}
