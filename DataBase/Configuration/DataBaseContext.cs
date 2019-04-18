using DataBase.Entities.Library;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Configuration
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
            // 
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<DownloadLog> DownloadLogs { get; set; }

        /// <summary>
        /// 自定义DbContext实体属性名与数据库表对应名称（默认 表名与属性名对应是 User与Users）
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().ToTable("library.nook");
            modelBuilder.Entity<User>().ToTable("library.user");
            modelBuilder.Entity<Author>().ToTable("library.author");
            modelBuilder.Entity<DownloadLog>().ToTable("library.downloadlog");
        }
    }
}
