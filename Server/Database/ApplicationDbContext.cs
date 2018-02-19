using System.Linq;
using Domain;
using Domain.Identity;
using Domain.Logging;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Database.EntityFrameworkCore
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, UserRole, string>
    {
        public DbSet<ServerEvent> ServerLog { get; set; }

        public DbSet<JwtResetToken> JwtResetTokens { get; set; }

        public DbSet<Film> Films { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public IQueryable<TEntity> GetQurable<TEntity>() where TEntity: class
        {
            return Set<TEntity>().AsQueryable();
        }
    }
}
