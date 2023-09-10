﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Summary.Data;

#nullable disable

namespace Summary.Migrations
{
    [DbContext(typeof(MytimeContext))]
    [Migration("20230904145028_AddCategory")]
    partial class AddCategory
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("Summary.Data.GeneratedToDoTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Finished")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("TaskDetails")
                        .HasMaxLength(5000)
                        .HasColumnType("TEXT");

                    b.Property<int>("ToDoTaskSettingId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Type")
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("priority")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ToDos");
                });

            modelBuilder.Entity("Summary.Data.MyTime", b =>
                {
                    b.Property<int>("currentIndex")
                        .HasColumnType("INTEGER")
                        .HasColumnName("currentIndex")
                        .HasColumnOrder(0);

                    b.Property<DateTime>("createDate")
                        .HasColumnType("TEXT")
                        .HasColumnName("createDate")
                        .HasColumnOrder(1);

                    b.Property<TimeSpan>("endTime")
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("lastTime")
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<string>("note")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("startTime")
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<int>("taskId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("type")
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<string>("userid")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("currentIndex", "createDate");

                    b.ToTable("MyTime");
                });
#pragma warning restore 612, 618
        }
    }
}