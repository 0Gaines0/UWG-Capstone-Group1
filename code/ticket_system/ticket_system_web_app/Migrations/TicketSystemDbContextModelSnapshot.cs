﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ticket_system_web_app.Data;

#nullable disable

namespace ticket_system_web_app.Migrations
{
    [DbContext(typeof(TicketSystemDbContext))]
    partial class TicketSystemDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EmployeeGroup", b =>
                {
                    b.Property<int>("EmployeesEId")
                        .HasColumnType("int");

                    b.Property<int>("GroupsExistingInGId")
                        .HasColumnType("int");

                    b.HasKey("EmployeesEId", "GroupsExistingInGId");

                    b.HasIndex("GroupsExistingInGId");

                    b.ToTable("group_member", (string)null);
                });

            modelBuilder.Entity("GroupProject", b =>
                {
                    b.Property<int>("AssignedGroupsGId")
                        .HasColumnType("int");

                    b.Property<int>("AssignedProjectsPId")
                        .HasColumnType("int");

                    b.HasKey("AssignedGroupsGId", "AssignedProjectsPId");

                    b.HasIndex("AssignedProjectsPId");

                    b.ToTable("project_group", (string)null);
                });

            modelBuilder.Entity("ticket_system_web_app.Models.BoardState", b =>
                {
                    b.Property<int>("StateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("state_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StateId"));

                    b.Property<int>("BoardId")
                        .HasColumnType("int")
                        .HasColumnName("board_id");

                    b.Property<int>("Position")
                        .HasColumnType("int")
                        .HasColumnName("position");

                    b.Property<string>("StateName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("state_name");

                    b.HasKey("StateId");

                    b.HasIndex("BoardId");

                    b.ToTable("BoardStates");
                });

            modelBuilder.Entity("ticket_system_web_app.Models.Employee", b =>
                {
                    b.Property<int>("EId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("e_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EId"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("email");

                    b.Property<string>("FName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("f_name");

                    b.Property<string>("HashedPassword")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("hashed_password");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("bit")
                        .HasColumnName("is_active");

                    b.Property<bool?>("IsAdmin")
                        .HasColumnType("bit")
                        .HasColumnName("is_admin");

                    b.Property<bool?>("IsManager")
                        .HasColumnType("bit")
                        .HasColumnName("is_manager");

                    b.Property<string>("LName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("l_name");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("username");

                    b.HasKey("EId");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            EId = 1,
                            Email = "admin@temp.com",
                            FName = "admin",
                            HashedPassword = "$2a$11$tqFhRcVPxPe/F7g4i2.9c.tms9AlneY5RDZb1SipsY1FQtMcaaecu",
                            IsActive = true,
                            IsAdmin = true,
                            IsManager = true,
                            LName = "admin",
                            Username = "tempAdmin"
                        });
                });

            modelBuilder.Entity("ticket_system_web_app.Models.Group", b =>
                {
                    b.Property<int>("GId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("g_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GId"));

                    b.Property<string>("GDescription")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("g_description");

                    b.Property<string>("GName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("g_name");

                    b.Property<int>("ManagerId")
                        .HasColumnType("int")
                        .HasColumnName("manager_id");

                    b.HasKey("GId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("ticket_system_web_app.Models.Project", b =>
                {
                    b.Property<int>("PId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("p_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PId"));

                    b.Property<string>("PDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("p_description");

                    b.Property<string>("PTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("p_title");

                    b.Property<int>("ProjectLeadId")
                        .HasColumnType("int")
                        .HasColumnName("project_lead_id");

                    b.HasKey("PId");

                    b.HasIndex("ProjectLeadId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("ticket_system_web_app.Models.ProjectBoard", b =>
                {
                    b.Property<int>("BoardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("board_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BoardId"));

                    b.Property<int>("ProjectId")
                        .HasColumnType("int")
                        .HasColumnName("p_id");

                    b.HasKey("BoardId");

                    b.ToTable("ProjectBoards");
                });

            modelBuilder.Entity("ticket_system_web_app.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EmployeeGroup", b =>
                {
                    b.HasOne("ticket_system_web_app.Models.Employee", null)
                        .WithMany()
                        .HasForeignKey("EmployeesEId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ticket_system_web_app.Models.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupsExistingInGId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GroupProject", b =>
                {
                    b.HasOne("ticket_system_web_app.Models.Group", null)
                        .WithMany()
                        .HasForeignKey("AssignedGroupsGId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ticket_system_web_app.Models.Project", null)
                        .WithMany()
                        .HasForeignKey("AssignedProjectsPId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ticket_system_web_app.Models.BoardState", b =>
                {
                    b.HasOne("ticket_system_web_app.Models.ProjectBoard", "Board")
                        .WithMany("States")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Board");
                });

            modelBuilder.Entity("ticket_system_web_app.Models.Project", b =>
                {
                    b.HasOne("ticket_system_web_app.Models.Employee", "ProjectLead")
                        .WithMany("ProjectsLeading")
                        .HasForeignKey("ProjectLeadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProjectLead");
                });

            modelBuilder.Entity("ticket_system_web_app.Models.Employee", b =>
                {
                    b.Navigation("ProjectsLeading");
                });

            modelBuilder.Entity("ticket_system_web_app.Models.ProjectBoard", b =>
                {
                    b.Navigation("States");
                });
#pragma warning restore 612, 618
        }
    }
}
