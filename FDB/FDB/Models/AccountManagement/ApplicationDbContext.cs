using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace FDB.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // Add an instance IDbSet using the 'new' keyword:
        new public virtual IDbSet<ApplicationRole> Roles { get; set; }

        // ADD THIS:
        public virtual IDbSet<Group> Groups { get; set; }

        public virtual IDbSet<MenuItem> MenuItems { get; set; }

        public virtual IDbSet<MenuItemRole> MenuItemRoles { get; set; }

        public virtual IDbSet<MenuItemGroup> MenuItemGroups { get; set; }

        public virtual IDbSet<MenuItemUser> MenuItemUsers { get; set; }

        public virtual IDbSet<AccountLog> AccountLogs { get; set; }

        public ApplicationDbContext()
            : base("FDB_QuanTri")
        {
        }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("modelBuilder");
            }

            // Keep this:
            modelBuilder.Entity<IdentityUser>().ToTable("AspNetUsers");

            // Change TUser to ApplicationUser everywhere else - IdentityUser 
            // and ApplicationUser essentially 'share' the AspNetUsers Table in the database:
            EntityTypeConfiguration<ApplicationUser> table =
                modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");

            table.Property((ApplicationUser u) => u.UserName).IsRequired();

            // EF won't let us swap out IdentityUserRole for ApplicationUserRole here:
            modelBuilder.Entity<ApplicationUser>()
                .HasMany<IdentityUserRole>((ApplicationUser u) => u.Roles);
            modelBuilder.Entity<IdentityUserRole>()
                .HasKey((IdentityUserRole r) =>
                new { UserId = r.UserId, RoleId = r.RoleId }).ToTable("AspNetUserRoles");

            // Add the group stuff here:
            modelBuilder.Entity<ApplicationUser>()
                .HasMany<ApplicationUserGroup>((ApplicationUser u) => u.Groups);
            modelBuilder.Entity<ApplicationUserGroup>()
                .HasKey((ApplicationUserGroup r) =>
                new { UserId = r.UserId, GroupId = r.GroupId }).ToTable("ApplicationUserGroups");

            // And here:
            modelBuilder.Entity<Group>()
                .HasMany<ApplicationRoleGroup>((Group g) => g.Roles);
            modelBuilder.Entity<ApplicationRoleGroup>()
                .HasKey((ApplicationRoleGroup gr) =>
                new { RoleId = gr.RoleId, GroupId = gr.GroupId }).ToTable("ApplicationRoleGroups");

            // And Here:
            EntityTypeConfiguration<Group> groupsConfig = modelBuilder.Entity<Group>().ToTable("Groups");
            groupsConfig.Property((Group r) => r.Name).IsRequired();

            //GiangTD3:
            EntityTypeConfiguration<MenuItem> MenuItemsConfig = modelBuilder.Entity<MenuItem>().ToTable("MenuItems");
            // MenuItemsConfig.Property((MenuItem r) => r.Item_Action).IsRequired();

            EntityTypeConfiguration<MenuItemRole> MenuItemRoleConfig = modelBuilder.Entity<MenuItemRole>().ToTable("MenuItemRoles");
            //groupsConfig.Property((Group r) => r.Name).IsRequired();

            EntityTypeConfiguration<MenuItemGroup> MenuItemGroupsConfig = modelBuilder.Entity<MenuItemGroup>().ToTable("MenuItemGroups");
            // groupsConfig.Property((Group r) => r.Name).IsRequired();

            EntityTypeConfiguration<MenuItemUser> MenuItemUsersConfig = modelBuilder.Entity<MenuItemUser>().ToTable("MenuItemUsers");
            // groupsConfig.Property((Group r) => r.Name).IsRequired();

            EntityTypeConfiguration<AccountLog> AccountLogsConfig = modelBuilder.Entity<AccountLog>().ToTable("AccountLogs");

            // Leave this alone:
            EntityTypeConfiguration<IdentityUserLogin> entityTypeConfiguration =
                modelBuilder.Entity<IdentityUserLogin>().HasKey((IdentityUserLogin l) =>
                    new
                    {
                        UserId = l.UserId,
                        LoginProvider = l.LoginProvider,
                        ProviderKey =
                            l.ProviderKey
                    }).ToTable("AspNetUserLogins");

            //entityTypeConfiguration.HasRequired<IdentityUser>((IdentityUserLogin u) => u.User);
            EntityTypeConfiguration<IdentityUserClaim> table1 =
                modelBuilder.Entity<IdentityUserClaim>().ToTable("AspNetUserClaims");
            //table1.HasRequired<IdentityUser>((IdentityUserClaim u) => u.User);

            // Add this, so that IdentityRole can share a table with ApplicationRole:
            modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles");

            // Change these from IdentityRole to ApplicationRole:
            EntityTypeConfiguration<ApplicationRole> entityTypeConfiguration1 =
                modelBuilder.Entity<ApplicationRole>().ToTable("AspNetRoles");
            entityTypeConfiguration1.Property((ApplicationRole r) => r.Name).IsRequired();
        }
    }
}