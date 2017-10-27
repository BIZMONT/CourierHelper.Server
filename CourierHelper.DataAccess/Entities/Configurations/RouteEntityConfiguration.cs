using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CourierHelper.DataAccess.Entities.Configurations
{
    public class RouteEntityConfiguration : EntityTypeConfiguration<Route>
    {
        public RouteEntityConfiguration()
        {
            HasKey(route => route.Id);
            Property(route => route.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
