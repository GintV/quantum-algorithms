using Microsoft.EntityFrameworkCore;

namespace Ginsoft.IDP.Entities
{
    public class GinsoftUserContext : DbContext
    {
        public GinsoftUserContext(DbContextOptions<GinsoftUserContext> options)
           : base(options)
        {
           
        }

        public DbSet<User> Users { get; set; }
    }
}
