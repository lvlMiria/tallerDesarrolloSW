using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Presentacion.Models;

public partial class SistPresupuestosContext : DbContext
{
    public SistPresupuestosContext()
    {
    }

    public SistPresupuestosContext(DbContextOptions<SistPresupuestosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Concepto> Conceptos { get; set; }

    public virtual DbSet<ControlFactura> ControlFacturas { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<Ipc> Ipcs { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Presupuesto> Presupuestos { get; set; }

    public virtual DbSet<TipoCambio> TipoCambios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=COV-NB-003\\SQLEXPRESS;Database=sist_presupuestos;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Concepto>(entity =>
        {
            entity.HasKey(e => e.CodConcepto).HasName("PK__CONCEPTO__99DD44B06ABECE69");

            entity.ToTable("CONCEPTO");

            entity.Property(e => e.CodConcepto).HasColumnName("cod_concepto");
            entity.Property(e => e.DescConcepto)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("desc_concepto");
        });

        modelBuilder.Entity<ControlFactura>(entity =>
        {
            entity.HasKey(e => e.CodControl).HasName("PK__CONTROL___A43CFC27EC63CC82");

            entity.ToTable("CONTROL_FACTURA");

            entity.Property(e => e.CodControl)
                .ValueGeneratedNever()
                .HasColumnName("cod_control");
            entity.Property(e => e.CodFactura).HasColumnName("cod_factura");
            entity.Property(e => e.CodPresupuesto).HasColumnName("cod_presupuesto");
            entity.Property(e => e.Comentario)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("comentario");
            entity.Property(e => e.FechaEntrega)
                .HasColumnType("date")
                .HasColumnName("fecha_entrega");
            entity.Property(e => e.FechaRecepcion)
                .HasColumnType("date")
                .HasColumnName("fecha_recepcion");
            entity.Property(e => e.Origen)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("origen");

            entity.HasOne(d => d.CodFacturaNavigation).WithMany(p => p.ControlFacturas)
                .HasForeignKey(d => d.CodFactura)
                .HasConstraintName("FK__CONTROL_F__cod_f__662B2B3B");

            entity.HasOne(d => d.CodPresupuestoNavigation).WithMany(p => p.ControlFacturas)
                .HasForeignKey(d => d.CodPresupuesto)
                .HasConstraintName("FK__CONTROL_F__cod_p__65370702");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.CodFactura).HasName("PK__FACTURA__94EEA4103A5AF8BB");

            entity.ToTable("FACTURA");

            entity.Property(e => e.CodFactura)
                .ValueGeneratedNever()
                .HasColumnName("cod_factura");
            entity.Property(e => e.AnioContable).HasColumnName("anio_contable");
            entity.Property(e => e.Empresa)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("empresa");
            entity.Property(e => e.MesContable).HasColumnName("mes_contable");
            entity.Property(e => e.Monto).HasColumnName("monto");
            entity.Property(e => e.NumFactura)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("num_factura");

            entity.HasOne(d => d.TipoCambio).WithMany(p => p.Facturas)
                .HasForeignKey(d => new { d.MesContable, d.AnioContable })
                .HasConstraintName("FK__FACTURA__59C55456");
        });

        modelBuilder.Entity<Ipc>(entity =>
        {
            entity.HasKey(e => e.Anio).HasName("PK__IPC__61B22F4730306A86");

            entity.ToTable("IPC");

            entity.Property(e => e.Anio)
                .ValueGeneratedNever()
                .HasColumnName("anio");
            entity.Property(e => e.Valor)
                .HasColumnType("decimal(6, 1)")
                .HasColumnName("valor");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.CodItem).HasName("PK__ITEM__0317CA76F027CDEA");

            entity.ToTable("ITEM");

            entity.Property(e => e.CodItem)
                .ValueGeneratedNever()
                .HasColumnName("cod_item");
            entity.Property(e => e.CodConcepto).HasColumnName("cod_concepto");
            entity.Property(e => e.ContNuevo)
                .HasMaxLength(24)
                .IsUnicode(false)
                .HasColumnName("cont_nuevo");
            entity.Property(e => e.DescItem)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("desc_item");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.GastoInversion)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("gasto_inversion");

            entity.HasOne(d => d.CodConceptoNavigation).WithMany(p => p.Items)
                .HasForeignKey(d => d.CodConcepto)
                .HasConstraintName("FK__ITEM__cod_concep__40F9A68C");
        });

        modelBuilder.Entity<Presupuesto>(entity =>
        {
            entity.HasKey(e => e.CodPresupuesto).HasName("PK__PRESUPUE__D67DB339E4509E5F");

            entity.ToTable("PRESUPUESTO");

            entity.Property(e => e.CodPresupuesto).HasColumnName("cod_presupuesto");
            entity.Property(e => e.Anio).HasColumnName("anio");
            entity.Property(e => e.CodItem).HasColumnName("cod_item");
            entity.Property(e => e.Mes).HasColumnName("mes");
            entity.Property(e => e.PresupuestoMes).HasColumnName("presupuesto_mes");

            entity.HasOne(d => d.CodItemNavigation).WithMany(p => p.Presupuestos)
                .HasForeignKey(d => d.CodItem)
                .HasConstraintName("FK__PRESUPUES__cod_i__625A9A57");
        });

        modelBuilder.Entity<TipoCambio>(entity =>
        {
            entity.HasKey(e => new { e.Mes, e.Anio }).HasName("PK__TIPO_CAM__B94B33AE271E5509");

            entity.ToTable("TIPO_CAMBIO");

            entity.Property(e => e.Mes).HasColumnName("mes");
            entity.Property(e => e.Anio).HasColumnName("anio");
            entity.Property(e => e.Valor).HasColumnName("valor");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
