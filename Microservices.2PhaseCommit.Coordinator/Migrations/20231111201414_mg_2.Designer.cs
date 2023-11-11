﻿// <auto-generated />
using System;
using Microservices._2PhaseCommit.Coordinator.Models.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Microservices._2PhaseCommit.Coordinator.Migrations
{
    [DbContext(typeof(TwoPhaseCommitContext))]
    [Migration("20231111201414_mg_2")]
    partial class mg_2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microservices._2PhaseCommit.Coordinator.Models.Node", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Nodes");

                    b.HasData(
                        new
                        {
                            Id = new Guid("c1df9a1d-e204-4ad0-b96f-cbaa77ed0817"),
                            Name = "Order.API"
                        },
                        new
                        {
                            Id = new Guid("0bb5ae92-5749-4f53-83bd-66ce78e0f5b4"),
                            Name = "Stock.API"
                        },
                        new
                        {
                            Id = new Guid("8b764654-2268-419a-9b37-37b5a84cc276"),
                            Name = "Payment.API"
                        });
                });

            modelBuilder.Entity("Microservices._2PhaseCommit.Coordinator.Models.NodeState", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("NodeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Ready")
                        .HasColumnType("int");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TransactionState")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NodeId");

                    b.ToTable("NodeStates");
                });

            modelBuilder.Entity("Microservices._2PhaseCommit.Coordinator.Models.NodeState", b =>
                {
                    b.HasOne("Microservices._2PhaseCommit.Coordinator.Models.Node", "Node")
                        .WithMany("NodeStates")
                        .HasForeignKey("NodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Node");
                });

            modelBuilder.Entity("Microservices._2PhaseCommit.Coordinator.Models.Node", b =>
                {
                    b.Navigation("NodeStates");
                });
#pragma warning restore 612, 618
        }
    }
}
