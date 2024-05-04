using Microsoft.EntityFrameworkCore;
using UserManagement.Backend.Domain.Database;

namespace UserManagement.Backend.Infrastructure.DataContext;

public class ManagementContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Group> Groups { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<GroupPermission> GroupPermission { get; set; }
    public DbSet<UserGroup> UserGroups { get; set; }
    public DbSet<LoginStatusLookup> LoginStatusLookup { get; set; }
    public DbSet<UserLoginHistory> UserLoginHistory { get;set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>(entity =>
        {
            entity.ToTable("Groups");

            entity.HasKey(e => e.Code)
                  .HasName("pk_Group_Code");

            entity.Property(e => e.Code)
                  .HasMaxLength(10)
                  .IsRequired()
                  .HasColumnName("GroupCode");

            entity.Property(e => e.Name)
                  .HasMaxLength(50)
                  .IsRequired()
                  .HasColumnName("Name");

            entity.Property(e => e.Description)
                  .HasMaxLength(255)
                  .IsRequired().HasColumnName("Description");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");

            entity.HasKey(e => e.EntryId)
                  .HasName("pk_User_Entry_Id");

            entity.Property(e => e.EntryId)
                  .HasColumnName("EntryId")
                  .IsRequired()
                  .UseIdentityColumn();

            entity.Property(e => e.FirstName)
                  .HasMaxLength(100)
                  .IsRequired()
                  .HasColumnName("FirstName");

            entity.Property(e => e.LastName)
                  .HasMaxLength(100)
                  .IsRequired()
                  .HasColumnName("LastName");

            entity.Property(e => e.Email)
                  .HasMaxLength(255)
                  .IsRequired()
                  .HasColumnName("Email");

            entity.HasIndex(x => x.Email)
                  .IsUnique()
                  .HasDatabaseName("idx_Users_Email");

            entity.Property(e => e.PasswordHash)
                  .HasMaxLength(255)
                  .IsRequired()
                  .HasColumnName("PasswordHash");

            entity.Property(e => e.PasswordSalt)
                  .HasMaxLength(255)
                  .IsRequired()
                  .HasColumnName("PasswordSalt");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.ToTable("Permissions");

            entity.HasKey(e => e.Code);

            entity.Property(e => e.Code)
                  .IsRequired()
                  .HasMaxLength(10)
                  .HasColumnName("PermissionCode");

            entity.Property(e => e.Description)
                  .HasMaxLength(255)
                  .IsRequired()
                  .HasColumnName("Description");
        });

        modelBuilder.Entity<GroupPermission>(entity =>
        {
            entity.ToTable("GroupPermission");
            entity.HasKey(x => new { x.GroupCode, x.PermissionCode });

            entity.HasOne(x => x.Group)
                  .WithMany(x => x.GroupPermissions)
                  .HasForeignKey(e => e.GroupCode)
                  .HasConstraintName("FK_GroupPermission_Groups_GroupsCode");

            entity.HasOne(x => x.Permission)
                  .WithMany(x => x.GroupPermissions)
                  .HasForeignKey(e => e.PermissionCode)
                  .HasConstraintName("FK_GroupPermission_Permissions_PermissionsCode");

            entity.Property(x => x.GroupCode)
                  .HasColumnName("GroupCode")
                  .HasMaxLength(10)
                  .IsRequired();

            entity.Property(x => x.PermissionCode)
                  .HasColumnName("PermissionCode")
                  .HasMaxLength(10)
                  .IsRequired();
        });

        modelBuilder.Entity<UserGroup>(entity =>
        {
            entity.ToTable("UserGroups");
            entity.HasKey(x => new { x.GroupCode, x.UserId });

            entity.HasOne(x => x.Group)
                  .WithMany(x => x.UserGroups)
                  .HasForeignKey(e => e.GroupCode)
                  .HasConstraintName("FK_GroupUser_Groups_GroupsCode");

            entity.HasOne(x => x.User)
                  .WithMany(x => x.UserGroups)
                  .HasForeignKey(e => e.UserId)
                  .HasConstraintName("FK_GroupUser_Users_UsersEntryId");

            entity.Property(x => x.GroupCode)
                  .HasColumnName("GroupCode")
                  .HasMaxLength(10)
                  .IsRequired();

            entity.Property(x => x.UserId)
                  .HasColumnName("UserId")
                  .IsRequired();
        });

        modelBuilder.Entity<LoginStatusLookup>(entity =>
        {
            entity.ToTable("LoginStatusLookup");

            entity.HasKey(e => e.StatusCode)
                  .HasName("StatusCode");

            entity.Property(e => e.Description)
                  .HasColumnName("Description")
                  .HasMaxLength(255)
                  .IsRequired();

            entity.Property(e => e.StatusCode)
                  .HasColumnName("StatusCode")
                  .HasMaxLength(3)
                  .IsRequired();

            entity.HasMany(x => x.UserLoginHistory)
                  .WithOne(x => x.StatusLookup);
        });

        modelBuilder.Entity<UserLoginHistory>(entity =>
        {
            entity.ToTable("UserLoginHistory");

            entity.HasKey(e => e.EntryId);

            entity.Property(e => e.EntryId)
                  .IsRequired()
                  .UseIdentityColumn()
                  .HasColumnName("EntryId");

            entity.Property(e => e.UserId)
                  .HasColumnName("UserId");

            entity.Property(e => e.Ip)
                  .IsRequired()
                  .HasColumnName("Ip");

            entity.Property(e => e.Date)
                  .HasColumnName("Date")
                  .IsRequired();

            entity.Property(e=>e.LoginStatus)
                  .HasColumnName("StatusCode")
                  .HasMaxLength(3)
                  .IsRequired();

            entity.HasOne(e => e.User)
                  .WithMany(e => e.UserLoginHistory)
                  .HasConstraintName("FK_UserLogin_Users_UsersEntryId");

            entity.HasOne(e => e.StatusLookup)
                  .WithMany(e => e.UserLoginHistory)
                  .HasConstraintName("FK_UserLoginHistory_Status_StatusLookup");

        });

        base.OnModelCreating(modelBuilder);
    }


}
