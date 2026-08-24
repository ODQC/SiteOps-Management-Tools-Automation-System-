using BackendAPI3._1.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SAIH_Backend.Datos.Entidades;
using WebApiSAIH.Models.Entidades;

namespace WebApiSAIH.Models
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasKey(b => b.PK_idEmployee)
                .HasName("PK_idEmployee");

            modelBuilder.Entity<Region>()
            .HasKey(b => b.PK_IdRegion)
            .HasName("PK_idRegion");

            modelBuilder.Entity<Department>()
               .HasKey(b => b.PK_idDepartment)
               .HasName("PK_idDepartment");

            modelBuilder.Entity<Role>()
               .HasKey(b => b.PK_idRole)
               .HasName("PK_idRole");

            modelBuilder.Entity<Site>()
                .HasKey(b => b.PK_IdSite)
                .HasName("PK_idSite");

            modelBuilder.Entity<AuditLog>()
                .HasKey(b => b.PK_idAuditLog)
                .HasName("PK_idAuditLog");

            modelBuilder.Entity<TaskItem>()
                .HasKey(b => b.PK_idTaskItem)
                .HasName("PK_idTaskItem");

            modelBuilder.Entity<Project>()
                .HasKey(b => b.PK_idProject)
                .HasName("PK_idProject");

            modelBuilder.Entity<Goal>()
                .HasKey(b => b.PK_idGoal)
                .HasName("PK_idGoal");

            modelBuilder.Entity<Resource>()
                .HasKey(b => b.PK_idResource)
                .HasName("PK_idResource");

            modelBuilder.Entity<ResourceGoal>()
                .HasKey(b => b.PK_idGoalResource)
                .HasName("PK_idGoalResource");

            modelBuilder.Entity<Deliverable>()
                .HasKey(b => b.PK_idDeliverable)
                .HasName("PK_idDeliverable");

            modelBuilder.Entity<Document>()
                .HasKey(b => b.DocumentId)
                .HasName("DocumentId");

            modelBuilder.Entity<EmailTemplate>()
                .HasKey(b => b.PK_idTemplate)
                .HasName("PK_idTemplate");

            modelBuilder.Entity<Email>()
                .HasKey(b => b.PK_idEmail)
                .HasName("PK_idEmail");

            modelBuilder.Entity<ProjectTask>()
                .HasKey(b => b.PK_idProjectTask)
                .HasName("PK_idProjectTask");
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Role> EmployeeRoles { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<Project> Projects{ get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<ResourceGoal> ResourceGoals { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Deliverable> Deliverables { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
    }
}
