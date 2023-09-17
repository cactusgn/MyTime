using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Data
{
    public class MyTime
    {
        [NotNull,Key, Column("currentIndex", Order = 0)]
        public int currentIndex { get; set; }
        [StringLength(10),AllowNull]
        public TimeSpan startTime { get; set; }
        [StringLength(10), AllowNull]
        public TimeSpan endTime { get; set; }
        [StringLength(10), AllowNull]
        public TimeSpan lastTime { get; set; }
        [Key, Column("createDate", Order = 1)]
        public DateTime createDate { get; set; }
        [StringLength(50), AllowNull]
        public string note { get; set; }
        public int taskId{ get; set; }
        [StringLength(10), AllowNull]
        public string type { get; set; }
        [StringLength(50), AllowNull]
        public string userid { get; set; }
    }
    public class ToDoTaskSetting
    {
        public bool Finished { get; set; }
        [NotNull]
        public DateTime CreateDate { get; set; }
        public DateTime GeneratedDate { get; set; }
        [StringLength(50), NotNull]
        public string Note { get; set; }
        [StringLength(10), AllowNull]
        public string Type { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public DateTime FinishedDate { get; set; }
        [StringLength(5000), AllowNull]
        public string TaskDetails{ get; set; }
        public int priority{ get; set; }
        public int RepeatType{ get; set; }
    }
    public class GeneratedToDoTask{
        public bool Finished { get; set; }
        [NotNull]
        public DateTime CreateDate { get; set; }
        [StringLength(50), NotNull]
        public string Note { get; set; }
        [StringLength(10), AllowNull]
        public string Type { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int ToDoTaskSettingId{ get; set; }
        public DateTime UpdatedDate { get; set; }
        [StringLength(5000), AllowNull]
        public string TaskDetails { get; set; }
        public int priority { get; set; }
    }
    public class Category{
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(50), NotNull]
        public string Name { get; set; }
        [AllowNull]
        public int BonusPerHour{ get; set; }
        [DefaultValue(true)]
        public bool Visible{ get; set; }
        public int ParentCategoryId{ get; set; }
        public string Color{ get; set; }
        [DefaultValue(true)]
        public bool AutoAddTask { get; set; }
    }
    public class MytimeContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Filename=TimeTests.db");
            //optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Trusted_Connection=True;Initial Catalog=MyTime;User ID=sa;Password=abcd-1234;integrated security=false;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MyTime>().HasKey(m => new { m.currentIndex, m.createDate });
            modelBuilder.Entity<GeneratedToDoTask>().HasKey(m => m.Id);
            modelBuilder.Entity<Category>();
            modelBuilder.Entity<ToDoTaskSetting>();
            base.OnModelCreating(modelBuilder);

        }
        public DbSet<MyTime> MyTime { set; get; }
        public DbSet<GeneratedToDoTask> ToDos { set; get; }
        public DbSet<Category> Categories { set; get; }
        public DbSet<ToDoTaskSetting> ToDoTaskSettings { set; get; }
    }
}
