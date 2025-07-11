﻿using Microsoft.EntityFrameworkCore;
using AquaparkApp.Shared; // zakładam, że model TodoItem jest w Shared
using AquaparkApp.Shared; 

namespace AquaparkApp.Server.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Atrakcja> Atrakcja { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}
