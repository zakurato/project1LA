using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace project1LA.Models
{
    public partial class AuthorizationConfig : DbContext
    {
        public AuthorizationConfig()
            : base("name=AuthorizationConfig")
        {
        }

        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<RolesUsuario> RolesUsuario { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Roles>()
                .Property(e => e.Description)
                .IsUnicode(false);
        }
    }
}
