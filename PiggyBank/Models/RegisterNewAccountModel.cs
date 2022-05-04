using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PiggyBank.Models
{ 
    public class RegisterNewAccountModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
       // public string AccountName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        //public decimal CurrentAccountBalance { get; set; }
        public AccountType AccountType { get; set; }
        //public strinng AccountNumberGenerated { get; set; }
        //public byte[] PinHash { get; set; }
        //public byte[] PinSalt { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]\d(4)$", ErrorMessage = "Pin must not be more than 4 digits")] //it should be a 4-digit string
        public string Pin { get; set; }
        [Required]

        [Compare("Pin", ErrorMessage = "Pind do not match")]
        public string ConfirmPin { get; set; }
        
        
        
          
    }
}
