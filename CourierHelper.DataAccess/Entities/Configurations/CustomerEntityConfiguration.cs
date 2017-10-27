using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CourierHelper.DataAccess.Entities.Configurations
{
    public class CustomerEntityConfiguration : EntityTypeConfiguration<Customer>
    {
        public CustomerEntityConfiguration()
        {
            HasKey(customer => customer.Id);
            Property(customer => customer.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
