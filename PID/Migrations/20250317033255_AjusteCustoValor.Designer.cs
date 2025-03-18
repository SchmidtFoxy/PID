﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PID.Data;

#nullable disable

namespace PID.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250317033255_AjusteCustoValor")]
    partial class AjusteCustoValor
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PID.Models.Custo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DesenvolvimentoIdDesenvolvimento")
                        .HasColumnType("int");

                    b.Property<int>("IdDesenvolvimento")
                        .HasColumnType("int");

                    b.Property<int>("IdDispendio")
                        .HasColumnType("int");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("DesenvolvimentoIdDesenvolvimento");

                    b.HasIndex("IdDesenvolvimento");

                    b.HasIndex("IdDispendio");

                    b.ToTable("Custos");
                });

            modelBuilder.Entity("PID.Models.Desenvolvimento", b =>
                {
                    b.Property<int>("IdDesenvolvimento")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdDesenvolvimento"));

                    b.Property<string>("Classificacao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Custo")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("DataFim")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataInicio")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Dificuldade")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ERP")
                        .HasColumnType("int");

                    b.Property<string>("Fase")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Produto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ProjetoFinep")
                        .HasColumnType("bit");

                    b.Property<bool>("ProjetoLeiBem")
                        .HasColumnType("bit");

                    b.Property<string>("Solicitante")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TempoDesenvolvimento")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdDesenvolvimento");

                    b.ToTable("Desenvolvimentos");
                });

            modelBuilder.Entity("PID.Models.Dispendio", b =>
                {
                    b.Property<int>("IdDispendio")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdDispendio"));

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ProjetoFinep")
                        .HasColumnType("bit");

                    b.Property<bool>("ProjetoLeiBem")
                        .HasColumnType("bit");

                    b.HasKey("IdDispendio");

                    b.ToTable("Dispendios");
                });

            modelBuilder.Entity("PID.Models.Historico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Categoria")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdDesenvolvimento")
                        .HasColumnType("int");

                    b.Property<string>("Responsavel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdDesenvolvimento");

                    b.ToTable("Historicos");
                });

            modelBuilder.Entity("PID.Models.ProjetoPD", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Ano")
                        .HasColumnType("int");

                    b.Property<string>("Classificacao")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Componente")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Estrutura")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("IdDesenvolvimento")
                        .HasColumnType("int");

                    b.Property<int>("IdDispendio")
                        .HasColumnType("int");

                    b.Property<bool>("ProjetoFinep")
                        .HasColumnType("bit");

                    b.Property<bool>("ProjetoLeiBem")
                        .HasColumnType("bit");

                    b.Property<string>("Solicitante")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("TipoProduto")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("IdDesenvolvimento");

                    b.HasIndex("IdDispendio");

                    b.ToTable("ProjetosPD");
                });

            modelBuilder.Entity("PID.Models.Tarefa", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdDesenvolvimento")
                        .HasColumnType("int");

                    b.Property<string>("Prioridade")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdDesenvolvimento");

                    b.ToTable("Tarefas");
                });

            modelBuilder.Entity("PID.Models.Custo", b =>
                {
                    b.HasOne("PID.Models.Desenvolvimento", null)
                        .WithMany("Custos")
                        .HasForeignKey("DesenvolvimentoIdDesenvolvimento");

                    b.HasOne("PID.Models.Desenvolvimento", "Desenvolvimento")
                        .WithMany()
                        .HasForeignKey("IdDesenvolvimento")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PID.Models.Dispendio", "Dispendio")
                        .WithMany()
                        .HasForeignKey("IdDispendio")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Desenvolvimento");

                    b.Navigation("Dispendio");
                });

            modelBuilder.Entity("PID.Models.Historico", b =>
                {
                    b.HasOne("PID.Models.Desenvolvimento", "Desenvolvimento")
                        .WithMany()
                        .HasForeignKey("IdDesenvolvimento")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Desenvolvimento");
                });

            modelBuilder.Entity("PID.Models.ProjetoPD", b =>
                {
                    b.HasOne("PID.Models.Desenvolvimento", "Desenvolvimento")
                        .WithMany()
                        .HasForeignKey("IdDesenvolvimento")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PID.Models.Dispendio", "Dispendio")
                        .WithMany()
                        .HasForeignKey("IdDispendio")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Desenvolvimento");

                    b.Navigation("Dispendio");
                });

            modelBuilder.Entity("PID.Models.Tarefa", b =>
                {
                    b.HasOne("PID.Models.Desenvolvimento", "Desenvolvimento")
                        .WithMany()
                        .HasForeignKey("IdDesenvolvimento")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Desenvolvimento");
                });

            modelBuilder.Entity("PID.Models.Desenvolvimento", b =>
                {
                    b.Navigation("Custos");
                });
#pragma warning restore 612, 618
        }
    }
}
