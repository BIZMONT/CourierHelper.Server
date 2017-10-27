using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CourierHelper.DataAccess.Entities.Configurations
{
    public class CourierEntityConfiguration : EntityTypeConfiguration<Courier>
    {
        public CourierEntityConfiguration()
        {
            HasKey(courier => courier.Id);
            Property(courier => courier.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
