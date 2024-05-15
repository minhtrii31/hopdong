using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HopDong.Models;

public partial class HopDongDbContext : DbContext
{
    public HopDongDbContext()
    {
    }

    public HopDongDbContext(DbContextOptions<HopDongDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<ContractType> ContractTypes { get; set; }

    public virtual DbSet<ContractsDetail> ContractsDetails { get; set; }

    public virtual DbSet<Party> Parties { get; set; }

    public virtual DbSet<Term> Terms { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=MT\\MT;Initial Catalog=HopDongDB;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contract>(entity =>
        {
            entity.ToTable("Contract");

            entity.Property(e => e.ContractId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ContractDate).HasColumnType("datetime");
            entity.Property(e => e.ContractEnd).HasColumnType("datetime");
            entity.Property(e => e.ContractLocation).HasMaxLength(255);
            entity.Property(e => e.ContractName).HasMaxLength(255);
            entity.Property(e => e.ContractStart).HasColumnType("datetime");
            entity.Property(e => e.ContractType)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.ContractTypeNavigation).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.ContractType)
                .HasConstraintName("FK_Contract_ContractType");
        });

        modelBuilder.Entity<ContractType>(entity =>
        {
            entity.HasKey(e => e.ContractTypeId).HasName("PK__Contract__68A61565A84BEF90");

            entity.ToTable("ContractType");

            entity.Property(e => e.ContractTypeId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ContractTypeName).HasMaxLength(255);
        });

        modelBuilder.Entity<ContractsDetail>(entity =>
        {
            entity.HasKey(e => e.ContractDetailId);

            entity.ToTable("ContractsDetail");

            entity.Property(e => e.ContractDetailId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ContractDetailContent).HasMaxLength(255);
            entity.Property(e => e.ContractDetailStatus).HasMaxLength(50);
            entity.Property(e => e.ContractId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PartyA)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PartyB)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Contract).WithMany(p => p.ContractsDetails)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_ContractsDetail_Contract");

            entity.HasOne(d => d.PartyANavigation).WithMany(p => p.ContractsDetailPartyANavigations)
                .HasForeignKey(d => d.PartyA)
                .HasConstraintName("FK_ContractsDetail_Parties");

            entity.HasOne(d => d.PartyBNavigation).WithMany(p => p.ContractsDetailPartyBNavigations)
                .HasForeignKey(d => d.PartyB)
                .HasConstraintName("FK_ContractsDetail_Parties1");
        });

        modelBuilder.Entity<Party>(entity =>
        {
            entity.HasKey(e => e.PartyId).HasName("PK_Party");

            entity.Property(e => e.PartyId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PartyAccount).HasMaxLength(255);
            entity.Property(e => e.PartyAddress).HasMaxLength(255);
            entity.Property(e => e.PartyContact).HasMaxLength(255);
            entity.Property(e => e.PartyName).HasMaxLength(255);
            entity.Property(e => e.PartyPosition).HasMaxLength(255);
            entity.Property(e => e.PartyRepresentative).HasMaxLength(255);
            entity.Property(e => e.PartyTax).HasMaxLength(255);
        });

        modelBuilder.Entity<Term>(entity =>
        {
            entity.ToTable("Term");

            entity.Property(e => e.TermId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ContractId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TermContent).HasMaxLength(255);
            entity.Property(e => e.TermHeader).HasMaxLength(255);

            entity.HasOne(d => d.Contract).WithMany(p => p.Terms)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_Term_Contract");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.UserFullName).HasMaxLength(255);
            entity.Property(e => e.UserPassword)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserRole).HasMaxLength(50);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
