using Microsoft.EntityFrameworkCore;
using Orqa_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orqa_Application.Data
{
    public class WorkstationDbContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<WorkPositionModel> WorkPositions { get; set; }
        public DbSet<UserWorkPositionModel> UserWorkPositions { get; set; }
        public WorkstationDbContext(DbContextOptions<WorkstationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<UserWorkPositionModel>()
                .HasOne(uwp => uwp.WorkPosition)
                .WithMany(r => r.UserWorkPositions)
                .HasForeignKey(uwp => uwp.WorkPositionId);

            modelBuilder.Entity<UserWorkPositionModel>()
                .HasOne(uwp => uwp.User)
                .WithOne(u => u.UserWorkPosition)
                .HasForeignKey<UserWorkPositionModel>(uwp => uwp.UserId);
        }
    }
}
