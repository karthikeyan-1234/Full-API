using Authenticate.Models;
using Microsoft.EntityFrameworkCore;

namespace Authenticate.Contexts
{
    public class TokenDBContext : DbContext
    {
        public DbSet<TokenModel>? Tokens { get; set; }

        public TokenDBContext(DbContextOptions<TokenDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var Tokens = modelBuilder.Entity<TokenModel>();

            Tokens.HasKey(t => new { t.Token, t.UserName});

            base.OnModelCreating(modelBuilder);
        }
    }
}
