
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PiggyBank.Models;

namespace PiggyBank.Services.Interfaces

{
  public interface ITransactionService
  {
    Response CreateNewTransaction(Transaction transaction); //using our custom created response
    Response FindTransactionByDate(DateTime date);
    Response MakeDeposite(string AccountNumber, decimal Amount, string TransactionPin);
    Response MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin);
    Response MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin);
    
  }
}