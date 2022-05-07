using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PiggyBank.Models
{ 
    public class AuthenticateModel
    {
      [Required]//validating the account is 10-digits using Regexp attribute
      [RegularExpression(@"^[0][1-9]\d{9}$|^[1-9]\d{9}$")]
      public string AccountNumber { get; set; }
      [Required]
      public string Pin { get; set; }    
    }
}
