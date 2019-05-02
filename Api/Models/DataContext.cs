using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Models
{
    public class DataContext : DbContext
    {
        public DbSet<Flight> Flights { get; set; }
        public DataContext(DbContextOptions<DataContext> options) 
            :base(options)
        {
            Database.EnsureCreated();
        }
    }
}
