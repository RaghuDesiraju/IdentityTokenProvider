using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserProfilesAPI.Model;
//using static IdentityModel.OidcConstants;

namespace UserProfilesAPI.Data
{
    public class AuthDbContext: DbContext
    {
        public AuthDbContext()
        {
        }
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
           : base(options)
        {
        }

        public virtual DbSet<AuthUser> AuthUser { get; set; }
        public virtual DbSet<Role> UserRole { get; set; }
        
        public virtual DbSet<ClientCredentials> ClientCredential { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserandRole>()
                .HasKey(bc => new { bc.AuthUserId, bc.RoleId });
            modelBuilder.Entity<UserandRole>()
                .HasOne(bc => bc.AuthUser)
                .WithMany(b => b.UserandRole)
                .HasForeignKey(bc => bc.AuthUserId);
            modelBuilder.Entity<UserandRole>()
                .HasOne(bc => bc.UserRole)
                .WithMany(c => c.UserandRole)
                .HasForeignKey(bc => bc.RoleId);

            modelBuilder.Entity<ClientCredentials>().Property(m => 
            m.AllowOfflineAccess).IsRequired(false);

            /*modelBuilder
                .Entity<ClientCredentials>()
                .Property(e => e.RefreshTokenUsage)
                .HasConversion(
                    v => Int32.Parse(v.ToString()),
                    v => (TokenUsage)Enum.Parse(typeof(TokenUsage), v.ToString()));
            modelBuilder
            .Entity<IdentityServer4.Models.GrantTypes>().HasData(
                Enum.GetValues(typeof(IdentityServer4.Models.GrantTypes.))
                    .Cast<WineVariantId>()
                    .Select(e => new WineVariant()
                    {
                        WineVariantId = e,
                        Name = e.ToString()
                    })
            );


            var converter = new ValueConverter<TokenUsage, string>(
                            v => v.ToString(),
                            v => (TokenUsage)Enum.Parse(typeof(TokenUsage), v));

            modelBuilder
                .Entity<ClientCredentials>()
                .Property(e => e.RefreshTokenUsage)
                .HasConversion(converter);

            var TokenExpiration = new ValueConverter<TokenExpiration, string>(
                v => v.ToString(),
                v => (TokenExpiration)Enum.Parse(typeof(TokenExpiration), v));

            modelBuilder
                .Entity<ClientCredentials>()
                .Property(e => e.RefreshTokenExpiration)
                .HasConversion(TokenExpiration);*/

        }


    }
}
