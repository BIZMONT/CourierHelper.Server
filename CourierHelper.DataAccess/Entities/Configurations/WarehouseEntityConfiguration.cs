using System.Data.Entity.ModelConfiguration;

namespace CourierHelper.DataAccess.Entities.Configurations
{
    public class WarehouseEntityConfiguration : EntityTypeConfiguration<Warehouse>
    {
        public WarehouseEntityConfiguration()
        {
            HasKey(warehouse => warehouse.Id);
        }
    }
}
