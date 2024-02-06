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
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=COV-NB-003\\SQLEXPRESS;Database=sist_presupuestos;Trusted_Connection=True;Encrypt=False");
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:Conexion");

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
            entity.HasKey(e => e.CodControl).HasName("PK__CONTROL___A43CFC27813B8B78");

            entity.ToTable("CONTROL_FACTURA");

            entity.Property(e => e.CodControl)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("cod_control");
            entity.Property(e => e.CodFactura)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("cod_factura");
            entity.Property(e => e.CodPresupuesto)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("cod_presupuesto");
            entity.Property(e => e.Comentario)
                .HasMaxLength(50)
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
                .HasConstraintName("FK__CONTROL_F__cod_f__09A971A2");

            entity.HasOne(d => d.CodPresupuestoNavigation).WithMany(p => p.ControlFacturas)
                .HasForeignKey(d => d.CodPresupuesto)
                .HasConstraintName("FK__CONTROL_F__cod_p__08B54D69");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.CodFactura).HasName("PK__FACTURA__94EEA41044178253");

            entity.ToTable("FACTURA");

            entity.Property(e => e.CodFactura)
                .HasMaxLength(6)
                .IsUnicode(false)
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
                .HasConstraintName("FK__FACTURA__6754599E");
        });

        modelBuilder.Entity<Ipc>(entity =>
        {
            entity.HasKey(e => e.Anio).HasName("PK__IPC__61B22F47AAA237D0");

            entity.ToTable("IPC");

            entity.Property(e => e.Anio)
                .ValueGeneratedNever()
                .HasColumnName("anio");
            entity.Property(e => e.Valor)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("valor");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.CodItem).HasName("PK__ITEM__0317CA76497138A8");

            entity.ToTable("ITEM");

            entity.Property(e => e.CodItem)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
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
                .HasConstraintName("FK__ITEM__cod_concep__5FB337D6");
        });

        modelBuilder.Entity<Presupuesto>(entity =>
        {
            entity.HasKey(e => e.CodPresupuesto).HasName("PK__PRESUPUE__D67DB3395FD4A489");

            entity.ToTable("PRESUPUESTO");

            entity.Property(e => e.CodPresupuesto)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("cod_presupuesto");
            entity.Property(e => e.Anio).HasColumnName("anio");
            entity.Property(e => e.CodItem)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("cod_item");
            entity.Property(e => e.Mes).HasColumnName("mes");
            entity.Property(e => e.PresupuestoMes).HasColumnName("presupuesto_mes");

            entity.HasOne(d => d.AnioNavigation).WithMany(p => p.Presupuestos)
                .HasForeignKey(d => d.Anio)
                .HasConstraintName("FK__PRESUPUEST__anio__05D8E0BE");

            entity.HasOne(d => d.CodItemNavigation).WithMany(p => p.Presupuestos)
                .HasForeignKey(d => d.CodItem)
                .HasConstraintName("FK__PRESUPUES__cod_i__04E4BC85");
        });

        modelBuilder.Entity<TipoCambio>(entity =>
        {
            entity.HasKey(e => new { e.Mes, e.Anio }).HasName("PK__TIPO_CAM__B94B33AE856F80B5");

            entity.ToTable("TIPO_CAMBIO");

            entity.Property(e => e.Mes).HasColumnName("mes");
            entity.Property(e => e.Anio).HasColumnName("anio");
            entity.Property(e => e.Valor).HasColumnName("valor");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
