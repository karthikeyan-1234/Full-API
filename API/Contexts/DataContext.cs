﻿using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Contexts
{
    public class DataContext : DbContext, IDataContext
    {
        public DbSet<Employee>? Employees { get; set; }
        public DbSet<City>? Cities { get; set; }
        IConfiguration configration;

        public DataContext(DbContextOptions<DataContext> options,IConfiguration configuration) : base(options)
        {
            this.configration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configration.GetConnectionString("DataContext"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var Employees = modelBuilder.Entity<Employee>().HasQueryFilter(e => e.id != 1);
            var Cities = modelBuilder.Entity<City>();

            Employees.HasKey(e => e.id);
            Employees.Property(e => e.id).UseIdentityColumn(1, 1);

            Cities.HasKey(c => c.id);
            Cities.Property(c => c.id).UseIdentityColumn(1, 1);
            //Cities.HasIndex(c => c.name).IsClustered(); Only one clustered index allowed per table. This is only for demo purposes

            Employees.HasOne(e => e.City_obj).WithMany(c => c.Employee_Objs).HasForeignKey(e => e.city_id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
