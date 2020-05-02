using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Text;

namespace InventoryManagement.Models
{
    public class InventoryDbModel
    {
        public int ID { get; set; }
        public string SerialNo { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public string ModelName { get; set; }
        public string ConsumerName { get; set; }
        public string EmployeeName { get; set; }
        public string ConsumerAddress { get; set; }
        public string SenderAddress { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public decimal? PriceTL { get; set; }
        public decimal? PriceDolar { get; set; }

    }
    public class DBContext : DbContext
    {
        public DBContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var appSettingsReader = new AppSettingsReader();
                optionsBuilder.UseSqlServer((string)appSettingsReader.GetValue("connectionString", typeof(string)));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InventoryDbModel>().HasKey(c => new { c.ID });
        }
        public virtual DbSet<InventoryDbModel> InventoryDbModels { get; set; }
    }
}
