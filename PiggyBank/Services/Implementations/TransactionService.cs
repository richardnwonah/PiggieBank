
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PiggyBank.Models;
using PiggyBank.Services.Interfaces;
using PiggyBank.DAL;
using System.Text;
using PiggyBank.Utils;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;



namespace PiggyBank.Services.Implementations;

  public class TransactionService : ITransactionService
  {
    private PiggyBankDbContext _dbContext;
    private ILogger _logger;
    private AppSettings _settings;
    private static string _ourBankSettlementAccount;
    private readonly IAccountService _accountService;
    public TransactionService(PiggyBankDbContext dbContext, ILogger<TransactionService> logger, IOptions<AppSettings> settings, IAccountService accountService)
    {
      _dbContext = dbContext;
      _logger = logger;
      _settings = settings.Value;
      _ourBankSettlementAccount = _settings.OurBankSettlementAccount;
      _accountService = accountService;
    }
   
     public Response CreateNewTransaction(Transaction transaction)
     {
       //create a new transaction
       Response response = new Response();
       _dbContext.Transactions.Add(transaction);
       _dbContext.SaveChanges();
       response.ResponseCode = "00";
       response.ResponseMessage = "Transaction created successfully!";
       response.Data = null;

       return response;
     }
    public Response FindTransactionByDate(DateTime date)
    {
      Response response = new Response();
      var transaction = _dbContext.Transactions.Where(x => x.TransactionDate == date).ToList(); //possibility of multiple transactions a day
       response.ResponseCode = "00";
       response.ResponseMessage = "Transaction created successfully!";
       response.Data = transaction;

       return response;

    }
    public Response MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
    {
        //make deposite
        Response response = new Response();
        Account sourceAccount;
        Account destinationAccount;
        Transaction transaction = new Transaction();

        var authUser = _accountService.Authenticate(AccountNumber, TransactionPin);
        if(authUser == null) throw new ApplicationException("Invalid credentials");

        //if validation passes
        try
        {
          //for deposite, our bankSettlementAccount is the source giving money to the user's account
          sourceAccount = _accountService.GetByAccountNumber(_ourBankSettlementAccount);
          destinationAccount = _accountService.GetByAccountNumber(AccountNumber);

          //TO update their account balances
          sourceAccount.CurrentAccountBalance -= Amount;
          destinationAccount.CurrentAccountBalance += Amount;

          //chec if there's an update
          if((_dbContext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
            (_dbContext.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
          {
            //if transaction is successful
            transaction.TransactionStatus = TranStatus.Success;
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction successful!";
            response.Data = null;
          }
          else
          {
            //if transaction is unsuccessful
            transaction.TransactionStatus = TranStatus.Failed;
            response.ResponseCode = "02";
            response.ResponseMessage = "Transaction failed!";
            response.Data = null;
          
          }
        }
        catch (Exception ex)
        {
          _logger.LogError($"AN ERROR OCCURRED... => {ex.Message}");
        }
        //Other props of transaction
        transaction.TransactionType = TranType.Deposit;
        transaction.TransactionSourceAccount = _ourBankSettlementAccount;
        transaction.TransactionDestinationAccount = AccountNumber;
        transaction.TransactionAmount = Amount;
        transaction.TransactionDate = DateTime.Now;
        transaction.TransactionParticulars = $"NEW TRANSACTION FROM SOURCE {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} TO DESTINATION ACCOUNT => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} ON DATE => {transaction.TransactionDate} FOR AMOUNT => {JsonConvert.SerializeObject(transaction.TransactionAmount)} TRANSACTION TYPE => {JsonConvert.SerializeObject(transaction.TransactionType)} TRANSACTION STATUS => {JsonConvert.SerializeObject(transaction.TransactionStatus)}";

        //commit to db
        _dbContext.Transactions.Add(transaction);
        _dbContext.SaveChanges();

        return response;

    }
    public Response MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
    {
         //make withdrawal

        Response response = new Response();
        Account sourceAccount;
        Account destinationAccount;
        Transaction transaction = new Transaction();

        var authUser = _accountService.Authenticate(AccountNumber, TransactionPin);
        if(authUser == null) throw new ApplicationException("Invalid credentials");

        //if validation passes
        try
        {
          //for withdrawals, our bankSettlementAccount is the destination giving money from the user's account
          sourceAccount = _accountService.GetByAccountNumber(AccountNumber);
          destinationAccount = _accountService.GetByAccountNumber(_ourBankSettlementAccount);

          //TO update their account balances
          sourceAccount.CurrentAccountBalance -= Amount;
          destinationAccount.CurrentAccountBalance += Amount;

          //chec if there's an update
          if((_dbContext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
            (_dbContext.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
          {
            //if transaction is successful
            transaction.TransactionStatus = TranStatus.Success;
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction successful!";
            response.Data = null;
          }
          else
          {
            //if transaction is unsuccessful
            transaction.TransactionStatus = TranStatus.Failed;
            response.ResponseCode = "02";
            response.ResponseMessage = "Transaction failed!";
            response.Data = null;
          
          }
        }
        catch (Exception ex)
        {
          _logger.LogError($"AN ERROR OCCURRED... => {ex.Message}");
        }
        //Other props of transaction
        transaction.TransactionType = TranType.Withdrawal;
        transaction.TransactionSourceAccount = _ourBankSettlementAccount;
        transaction.TransactionDestinationAccount = AccountNumber;
        transaction.TransactionAmount = Amount;
        transaction.TransactionDate = DateTime.Now;
        transaction.TransactionParticulars = $"NEW TRANSACTION FROM SOURCE {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} TO DESTINATION ACCOUNT => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} ON DATE => {transaction.TransactionDate} FOR AMOUNT => {JsonConvert.SerializeObject(transaction.TransactionAmount)} TRANSACTION TYPE => {JsonConvert.SerializeObject(transaction.TransactionType)} TRANSACTION STATUS => {JsonConvert.SerializeObject(transaction.TransactionStatus)}";

        //commit to db
        _dbContext.Transactions.Add(transaction);
        _dbContext.SaveChanges();

        return response;
    }
    public Response MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
    {
               //make withdrawal

        Response response = new Response();
        Account sourceAccount;
        Account destinationAccount;
        Transaction transaction = new Transaction();

        var authUser = _accountService.Authenticate(FromAccount, TransactionPin);
        if(authUser == null) throw new ApplicationException("Invalid credentials");

        //if validation passes
        try
        {
          //for withdrawals, our bankSettlementAccount is the destination giving money from the user's account
          sourceAccount = _accountService.GetByAccountNumber(FromAccount);
          destinationAccount = _accountService.GetByAccountNumber(ToAccount);

          //TO update their account balances
          sourceAccount.CurrentAccountBalance -= Amount;
          destinationAccount.CurrentAccountBalance += Amount;

          //chec if there's an update
          if((_dbContext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
            (_dbContext.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
          {
            //if transaction is successful
            transaction.TransactionStatus = TranStatus.Success;
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction successful!";
            response.Data = null;
          }
          else
          {
            //if transaction is unsuccessful
            transaction.TransactionStatus = TranStatus.Failed;
            response.ResponseCode = "02";
            response.ResponseMessage = "Transaction failed!";
            response.Data = null;
          
          }
        }
        catch (Exception ex)
        {
          _logger.LogError($"AN ERROR OCCURRED... => {ex.Message}");
        }
        //Other props of transaction
        transaction.TransactionType = TranType.Transfer;
        transaction.TransactionSourceAccount = FromAccount;
        transaction.TransactionDestinationAccount = ToAccount;
        transaction.TransactionAmount = Amount;
        transaction.TransactionDate = DateTime.Now;
        transaction.TransactionParticulars = $"NEW TRANSACTION FROM SOURCE {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} TO DESTINATION ACCOUNT => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} ON DATE => {transaction.TransactionDate} FOR AMOUNT => {JsonConvert.SerializeObject(transaction.TransactionAmount)} TRANSACTION TYPE => {JsonConvert.SerializeObject(transaction.TransactionType)} TRANSACTION STATUS => {JsonConvert.SerializeObject(transaction.TransactionStatus)}";

        //commit to db
        _dbContext.Transactions.Add(transaction);
        _dbContext.SaveChanges();

        return response;
    }
  }
