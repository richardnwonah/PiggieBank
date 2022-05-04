using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PiggyBank.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace PiggyBank.DAL
{
     public class PiggyBankDbContext : DbContext
    {
        public PiggyBankDbContext(DbContextOptions<PiggyBankDbContext> options) : base(options)
        {
            
        }
        
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}