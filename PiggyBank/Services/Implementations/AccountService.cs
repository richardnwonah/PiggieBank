
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PiggyBank.Models;
using PiggyBank.Services.Interfaces;
using PiggyBank.DAL;
using System.Text;


namespace PiggyBank.Services.Implementations

{
  public class AccountService : IAccountService
  {
    private PiggyBankDbContext _dbContext;
    public AccountService(PiggyBankDbContext dbContext)
    {
      _dbContext = dbContext;
    }
      public Account Authenticate(string AccountNumber, string Pin)
      {
          //Making Authentication
          //does account exist for that account number
          var account = _dbContext.Accounts.Where(x => x.AccountNumberGenerated == AccountNumber).SingleOrDefault();
          if (account == null)
          return null;
          //if we have a match
          //verify pinHash
          if(!VerifyPinHash(Pin, account.PinHash, account.PinSalt))
            return null;

          //Ok so Authentucation is passed
          return account;
      } 

      private static bool VerifyPinHash(string Pin, byte[] pinHash, byte[] pinSalt)
      {
        if (string.IsNullOrWhiteSpace(Pin)) throw new ArgumentNullException("Pin");
        //verify pin
        using(var hmac = new System.Security.Cryptography.HMACSHA512(pinSalt))
        {
          var computedPinHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Pin));
          for (int i = 0; i < computedPinHash.Length; i++)
          {
            if(computedPinHash[i] != pinHash[i]) return false;
          }
        }
        return true;
      }
      public Account Create(Account account, string Pin, string ConfirmPin)
      {
        //this is to create a new account
        if(_dbContext.Accounts.Any(x => x.Email == account.Email)) throw new ApplicationException("An account already exist with this email");
        //validate pin
        if(!Pin.Equals(ConfirmPin)) throw new ArgumentException("Pins do not match", "Pin");

        //if all validations passes, we create the account
        //salting pin
        byte[] pinHash, pinSalt;
        CreatePinHash(Pin, out pinHash, out pinSalt);

        account.PinHash = pinHash;
        account.PinSalt = pinSalt;

        //add new account to database
        _dbContext.Accounts.Add(account);
        _dbContext.SaveChanges();

        return account;
      }
      private static void CreatePinHash(string pin, out byte[] pinHash, out byte[] pinSalt)
      {
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
          pinSalt = hmac.Key;
          pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
        }
      }
      public void Delete(int Id)
      {
        var account = _dbContext.Accounts.Find(Id);
        if(account != null)
        {
          _dbContext.Accounts.Remove(account);
          _dbContext.SaveChanges();
        }
      }
      public IEnumerable<Account> GetAllAccounts()
      {
        return _dbContext.Accounts.ToList();
      }
     public Account GetByAccountNumber(string AccountNumber)
      {
        var account = _dbContext.Accounts.Where(x => x.AccountNumberGenerated == AccountNumber).FirstOrDefault();
        if(account == null) return null;

        return account;
      }
      public Account GetById( int Id)
      {
        var account = _dbContext.Accounts.Where(x => x.Id == Id).FirstOrDefault();
       if (account == null) return null;

       return account;
      }
      public void Update(Account account, string Pin = null)
      {
        var accountToBeUpdated = _dbContext.Accounts.Where(x => x.Email == account.Email).SingleOrDefault();
        if(accountToBeUpdated == null) throw new ApplicationException("Account does not exist");
        //if it exists, listen for user to change any of his properties
        if(!string.IsNullOrWhiteSpace(account.Email))
        {
          //user wishes to change email
          //check if the one he is changing to in not already taken
          if(_dbContext.Accounts.Any(x => x.Email == account.Email)) throw new ApplicationException("This Email" + account.Email + "already exists");
          //else change email

          accountToBeUpdated.Email = account.Email;
        }

        if(!string.IsNullOrWhiteSpace(account.PhoneNumber))
        {
          //user wishes to change phone number
          //check if the one he is changing to in not already taken
          if(_dbContext.Accounts.Any(x => x.PhoneNumber == account.PhoneNumber)) throw new ApplicationException("This Phone number" + account.PhoneNumber + "already exists");
          //else change phone number

          accountToBeUpdated.PhoneNumber = account.PhoneNumber;
        }

        //we want to allow the user to only be able to change Emali and phone number
           if(!string.IsNullOrWhiteSpace(Pin))
        {
          //user wishes to change pin

          byte[] pinHash, pinSalt;
          CreatePinHash(Pin,out pinHash, out pinSalt);

          accountToBeUpdated.PinHash = pinHash;
          accountToBeUpdated.PinSalt = pinSalt; 
         
        }
         accountToBeUpdated.DateLastUpdated = DateTime.Now;

        //persist the update to database
        _dbContext.Accounts.Update(accountToBeUpdated);
        _dbContext.SaveChanges();
      }
      
    
  }
}