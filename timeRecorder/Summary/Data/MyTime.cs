using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        [StringLength(10), AllowNull]
        public string type { get; set; }
        [StringLength(50), AllowNull]
        public string userid { get; set; }
    }
    
    public class MytimeContext : DbContext
    {
       
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Trusted_Connection=True;Initial Catalog=MyTime;User ID=sa;Password=abcd-1234;integrated security=false;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True;");
            }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MyTime>().HasKey(m => new { m.currentIndex, m.createDate });
            base.OnModelCreating(modelBuilder);

        }
        public DbSet<MyTime> MyTime { set; get; }

    }
}
