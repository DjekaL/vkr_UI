using System;
using WpfApp1.Confuguration.Models;
using Microsoft.EntityFrameworkCore;

namespace WpfApp1.Confuguration {
    public class ApplicationContext : DbContext {
        public DbSet<Device> Devices { get; set; } = null!;
        public DbSet<Models.Connection> Connections { get; set; } = null!;
        public DbSet<DataTransferLog> DataTransferLogs { get; set; } = null!;
        public DbSet<DeviceType> DeviceTypes { get; set; } = null!;
        public DbSet<State> States { get; set; } = null!;
        public DbSet<StateLog> StateLogs { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite(@"Data Source=./../../../monitor.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Device>()
                .HasMany(e => e.StateLogs)
                .WithOne(e => e.Device)
                .HasForeignKey(e => e.DeviceId);

            modelBuilder.Entity<DeviceType>()
                .HasMany(e => e.Devices)
                .WithOne(e => e.DeviceType)
                .HasForeignKey (e => e.DeviceTypeId);

            modelBuilder.Entity<StateLog>()
                .HasOne(e => e.State)
                .WithMany(e => e.StateLogs)
                .HasForeignKey(e => e.StateId);

            modelBuilder.Entity<Models.Connection>()
                .HasOne(e => e.FirstDevice)
                .WithMany(e => e.HostConnections)
                .HasForeignKey(e => e.FirstDeviceId);

            modelBuilder.Entity<Models.Connection>()
                .HasOne(e => e.SecondDevice)
                .WithMany(e => e.ClientConnections)
                .HasForeignKey(e => e.SecondDeviceId);

            modelBuilder.Entity<DataTransferLog>()
                .HasOne(e => e.Connection)
                .WithMany(e => e.DataTransferLogs)
                .HasForeignKey(e => e.ConnectionId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
