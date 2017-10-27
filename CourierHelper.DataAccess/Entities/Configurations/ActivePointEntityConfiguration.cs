
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CourierHelper.DataAccess.Entities.Configurations
{
    public class ActivePointEntityConfiguration : EntityTypeConfiguration<ActivePoint>
    {
        public ActivePointEntityConfiguration()
        {
            HasKey(activePoint => activePoint.Id);
            Property(activePoint => activePoint.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Ignore(activePoint => activePoint.Coordinates);
        }
    }
}
