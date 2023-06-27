//using System;
//using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;

//namespace DiplomApplication.DataBase;

//public partial class User051Context : DbContext
//{
//    public User051Context()
//    {
//    }

//    public User051Context(DbContextOptions<User051Context> options)
//        : base(options)
//    {
//    }

//    public virtual DbSet<Director> Directors { get; set; }

//    public virtual DbSet<Employee> Employees { get; set; }

//    public virtual DbSet<FeedBack> FeedBacks { get; set; }

//    public virtual DbSet<FeedBackRate> FeedBackRates { get; set; }

//    public virtual DbSet<Menu> Menus { get; set; }

//    public virtual DbSet<MenuItemType> MenuItemTypes { get; set; }

//    public virtual DbSet<Order> Orders { get; set; }

//    public virtual DbSet<Pbo> Pbos { get; set; }

//    public virtual DbSet<Position> Positions { get; set; }

//    public virtual DbSet<Product> Products { get; set; }

//    public virtual DbSet<Questionaire> Questionaires { get; set; }

//    public virtual DbSet<Role> Roles { get; set; }

//    public virtual DbSet<User> Users { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseNpgsql("host=192.168.2.7; database=user051;username=user051;password=99846");

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.Entity<Director>(entity =>
//        {
//            entity.HasKey(e => e.DirectorId).HasName("Directors_pkey");

//            entity.ToTable("Directors", "DiplomDataBase");

//            entity.Property(e => e.DirectorId)
//                .UseIdentityAlwaysColumn()
//                .HasColumnName("director_id");
//            entity.Property(e => e.Address)
//                .HasMaxLength(100)
//                .HasColumnName("address");
//            entity.Property(e => e.Dateofbirth).HasColumnName("dateofbirth");
//            entity.Property(e => e.Directorname)
//                .HasMaxLength(50)
//                .HasColumnName("directorname");
//            entity.Property(e => e.Email)
//                .HasMaxLength(50)
//                .HasColumnName("email");
//            entity.Property(e => e.Inn)
//                .HasColumnType("character varying")
//                .HasColumnName("inn");
//            entity.Property(e => e.Laborcontractnumber)
//                .HasColumnType("character varying")
//                .HasColumnName("laborcontractnumber");
//            entity.Property(e => e.Passportnumber).HasColumnName("passportnumber");
//            entity.Property(e => e.Passportseries).HasColumnName("passportseries");
//            entity.Property(e => e.Patronymic)
//                .HasMaxLength(50)
//                .HasColumnName("patronymic");
//            entity.Property(e => e.Phone)
//                .HasMaxLength(21)
//                .HasColumnName("phone");
//            entity.Property(e => e.Surname)
//                .HasMaxLength(50)
//                .HasColumnName("surname");
//        });

//        modelBuilder.Entity<Employee>(entity =>
//        {
//            entity.HasKey(e => e.EmployeeId).HasName("Employees_pkey");

//            entity.ToTable("Employees", "DiplomDataBase");

//            entity.HasIndex(e => e.PboId, "fki_fk_pboid");

//            entity.HasIndex(e => e.EmployeeId, "fki_fk_positionid");

//            entity.Property(e => e.EmployeeId)
//                .UseIdentityAlwaysColumn()
//                .HasColumnName("EmployeeID");
//            entity.Property(e => e.Address).HasMaxLength(200);
//            entity.Property(e => e.Email).HasMaxLength(50);
//            entity.Property(e => e.FirstName).HasMaxLength(50);
//            entity.Property(e => e.Image)
//                .HasColumnType("character varying")
//                .HasColumnName("image");
//            entity.Property(e => e.Inn).HasColumnName("INN");
//            entity.Property(e => e.LastName).HasMaxLength(50);
//            entity.Property(e => e.Patronymic).HasMaxLength(50);
//            entity.Property(e => e.PboId).HasColumnName("PboID");
//            entity.Property(e => e.Phone).HasMaxLength(50);
//            entity.Property(e => e.PositionId).HasColumnName("PositionID");

//            entity.HasOne(d => d.Position).WithMany(p => p.Employees)
//                .HasForeignKey(d => d.PositionId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("fk_positionid");
//        });

//        modelBuilder.Entity<FeedBack>(entity =>
//        {
//            entity.HasKey(e => e.FeedbackId).HasName("FeedBack_pkey");

//            entity.ToTable("FeedBack", "DiplomDataBase");

//            entity.HasIndex(e => e.FeedbackId, "fki_fk_feedbackrateid");

//            entity.Property(e => e.FeedbackId)
//                .UseIdentityAlwaysColumn()
//                .HasColumnName("FeedbackID");
//            entity.Property(e => e.FeedBackText).HasColumnType("character varying");
//            entity.Property(e => e.FeedbackRateId).HasColumnName("FeedbackRateID");
//            entity.Property(e => e.PboId).HasColumnName("PboID");

//            entity.HasOne(d => d.FeedbackRate).WithMany(p => p.FeedBacks)
//                .HasForeignKey(d => d.FeedbackRateId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("fk_feedbackrateid");
//        });

//        modelBuilder.Entity<FeedBackRate>(entity =>
//        {
//            entity.HasKey(e => e.FeedbackRateId).HasName("FeedBackRate_pkey");

//            entity.ToTable("FeedBackRate", "DiplomDataBase");

//            entity.Property(e => e.FeedbackRateId)
//                .UseIdentityAlwaysColumn()
//                .HasColumnName("FeedbackRateID");
//            entity.Property(e => e.Title).HasMaxLength(50);
//        });

//        modelBuilder.Entity<Menu>(entity =>
//        {
//            entity.HasKey(e => e.MenuItemId).HasName("Menu_pkey");

//            entity.ToTable("Menu", "DiplomDataBase");

//            entity.HasIndex(e => e.MenuItemId, "fki_fk_menuitemtypeid");

//            entity.Property(e => e.MenuItemId)
//                .UseIdentityAlwaysColumn()
//                .HasColumnName("MenuItemID");
//            entity.Property(e => e.Carbohydrates).HasColumnName("carbohydrates");
//            entity.Property(e => e.Composition).HasColumnType("character varying");
//            entity.Property(e => e.Cost).HasColumnType("money");
//            entity.Property(e => e.Description).HasColumnName("description");
//            entity.Property(e => e.Energyvalue).HasColumnName("energyvalue");
//            entity.Property(e => e.Fats).HasColumnName("fats");
//            entity.Property(e => e.Image).HasColumnType("character varying");
//            entity.Property(e => e.MenuItemTypeId).HasColumnName("MenuItemTypeID");
//            entity.Property(e => e.Proteins).HasColumnName("proteins");
//            entity.Property(e => e.Title).HasMaxLength(50);

//            entity.HasOne(d => d.MenuItemType).WithMany(p => p.Menus)
//                .HasForeignKey(d => d.MenuItemTypeId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("fk_menuitemtypeid");
//        });

//        modelBuilder.Entity<MenuItemType>(entity =>
//        {
//            entity.HasKey(e => e.MenuItemTypeId).HasName("MenuItemType_pkey");

//            entity.ToTable("MenuItemType", "DiplomDataBase");

//            entity.Property(e => e.MenuItemTypeId)
//                .UseIdentityAlwaysColumn()
//                .HasColumnName("MenuItemTypeID");
//            entity.Property(e => e.Title).HasMaxLength(50);
//        });

//        modelBuilder.Entity<Order>(entity =>
//        {
//            entity.HasKey(e => e.OrderId).HasName("Order_pkey");

//            entity.ToTable("Order", "DiplomDataBase");

//            entity.HasIndex(e => e.OrderId, "fki_fk_userid");

//            entity.Property(e => e.OrderId)
//                .UseIdentityAlwaysColumn()
//                .HasColumnName("OrderID");
//            entity.Property(e => e.Cost).HasColumnType("money");
//            entity.Property(e => e.PboId).HasColumnName("PboID");
//            entity.Property(e => e.SecurityCode).HasMaxLength(10);
//            entity.Property(e => e.UserId).HasColumnName("UserID");

//            entity.HasOne(d => d.Pbo).WithMany(p => p.Orders)
//                .HasForeignKey(d => d.PboId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("fk_pboid");

//            entity.HasOne(d => d.User).WithMany(p => p.Orders)
//                .HasForeignKey(d => d.UserId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("fk_userid");
//        });

//        modelBuilder.Entity<Pbo>(entity =>
//        {
//            entity.HasKey(e => e.PboId).HasName("PBO_pkey");

//            entity.ToTable("PBO", "DiplomDataBase");

//            entity.HasIndex(e => e.PboId, "fki_fk_directorid");

//            entity.Property(e => e.PboId)
//                .UseIdentityAlwaysColumn()
//                .HasColumnName("PboID");
//            entity.Property(e => e.Address).HasMaxLength(100);
//            entity.Property(e => e.DirectorId).HasColumnName("DirectorID");
//            entity.Property(e => e.ThisYearProceeds)
//                .HasDefaultValueSql("0")
//                .HasColumnType("money");
//            entity.Property(e => e.Title).HasMaxLength(50);

//            entity.HasOne(d => d.Director).WithMany(p => p.Pbos)
//                .HasForeignKey(d => d.DirectorId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("fk_directorid");
//        });

//        modelBuilder.Entity<Position>(entity =>
//        {
//            entity.HasKey(e => e.PositionId).HasName("Positions_pkey");

//            entity.ToTable("Positions", "DiplomDataBase");

//            entity.Property(e => e.PositionId)
//                .UseIdentityAlwaysColumn()
//                .HasColumnName("PositionID");
//            entity.Property(e => e.Title).HasMaxLength(50);
//        });

//        modelBuilder.Entity<Product>(entity =>
//        {
//            entity.HasKey(e => e.ProductsId).HasName("Products_pkey");

//            entity.ToTable("Products", "DiplomDataBase");

//            entity.Property(e => e.ProductsId)
//                .UseIdentityAlwaysColumn()
//                .HasColumnName("ProductsID");
//            entity.Property(e => e.Title).HasMaxLength(50);
//        });

//        modelBuilder.Entity<Questionaire>(entity =>
//        {
//            entity.HasKey(e => e.QuestionairId).HasName("Questionaires_pkey");

//            entity.ToTable("Questionaires", "DiplomDataBase");

//            entity.Property(e => e.QuestionairId)
//                .UseIdentityAlwaysColumn()
//                .HasColumnName("QuestionairID");
//            entity.Property(e => e.Email).HasMaxLength(50);
//            entity.Property(e => e.Name).HasMaxLength(50);
//            entity.Property(e => e.Patronymic).HasMaxLength(50);
//            entity.Property(e => e.PboId).HasColumnName("pboID");
//            entity.Property(e => e.Phone)
//                .HasMaxLength(20)
//                .HasColumnName("phone");
//            entity.Property(e => e.Questionairstatus)
//                .HasColumnType("character varying")
//                .HasColumnName("questionairstatus");
//            entity.Property(e => e.Surname).HasMaxLength(50);
//        });

//        modelBuilder.Entity<Role>(entity =>
//        {
//            entity.HasKey(e => e.RoleId).HasName("Role_pkey");

//            entity.ToTable("Role", "DiplomDataBase");

//            entity.Property(e => e.RoleId)
//                .UseIdentityAlwaysColumn()
//                .HasColumnName("RoleID");
//            entity.Property(e => e.Title).HasMaxLength(50);
//        });

//        modelBuilder.Entity<User>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("Users_pkey");

//            entity.ToTable("Users", "DiplomDataBase");

//            entity.HasIndex(e => e.Id, "fki_fk_roleid");

//            entity.Property(e => e.Id)
//                .UseIdentityAlwaysColumn()
//                .HasColumnName("UserID");
//            entity.Property(e => e.BonusCount).HasDefaultValueSql("0");
//            entity.Property(e => e.Email).HasMaxLength(100);
//            entity.Property(e => e.FirstName).HasMaxLength(50);
//            entity.Property(e => e.LastName).HasMaxLength(50);
//            entity.Property(e => e.Password).HasColumnType("character varying");
//            entity.Property(e => e.Patronymic).HasMaxLength(50);
//            entity.Property(e => e.Phone).HasMaxLength(20);
//            entity.Property(e => e.RoleId).HasColumnName("RoleID");
//            entity.Property(e => e.Userimage)
//                .HasColumnType("character varying")
//                .HasColumnName("userimage");

//            entity.HasOne(d => d.Role).WithMany(p => p.Users)
//                .HasForeignKey(d => d.RoleId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("fk_roleid");
//        });
//        modelBuilder.HasSequence("order_seq");
//        modelBuilder.HasSequence("role_seq");
//        modelBuilder.HasSequence("user_seq");

//        OnModelCreatingPartial(modelBuilder);
//    }

//    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//}
