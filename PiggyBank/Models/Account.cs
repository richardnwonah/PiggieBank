using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PiggyBank.Models
{ 
    [Table("Accounts")]
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal CurrentAccountBalance { get; set; }
        public AccountType AccountType { get; set; }
        public string AccountNumberGenerated { get; set; }
        [JsonIgnore]
        public byte[] PinHash { get; set; }
        [JsonIgnore]
        public byte[] PinSalt { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }    

    public Account()
    {
        var rand = new Random();
        AccountNumberGenerated = Convert.ToString((long) Math.Floor(rand.NextDouble() * 9_000_000_000L + 1_000_000_000L));

        AccountName = $"{FirstName} {LastName}"; 
    }
}
    public enum AccountType
    {
        Savings,
        Current,
        Corporate,
        Government
    }
}
