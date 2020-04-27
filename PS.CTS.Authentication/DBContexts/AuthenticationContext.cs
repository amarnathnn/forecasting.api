using Microsoft.EntityFrameworkCore;
using PS.CTS.Common.Entities;

namespace PS.CTS.AuthenticationService
{
    public class AuthenticationContext : DbContext
    {
        public DbSet<User> Users { get; set; }       
       
        public AuthenticationContext(DbContextOptions<AuthenticationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           // modelBuilder.Entity<MentorSkill>().HasKey(ms => new { ms.MentorId, ms.SkillId });
        }
    }
}
