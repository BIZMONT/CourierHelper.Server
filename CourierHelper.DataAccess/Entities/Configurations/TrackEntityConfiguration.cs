using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CourierHelper.DataAccess.Entities.Configurations
{
    public class TrackEntityConfiguration : EntityTypeConfiguration<Track>
    {
        public TrackEntityConfiguration()
        {
            HasKey(track => track.Id);
            Property(track => track.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
