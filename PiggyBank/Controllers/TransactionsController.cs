using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PiggyBank.Models;
using PiggyBank.Services;
using PiggyBank.Services.Interfaces;
using PiggyBank.Services.Implementations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace PiggyBank.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionsController : ControllerBase
{
    private ITransactionService _accountService;
    IMapper _mapper;
    private TransactionService _transactionService;

    public TransactionsController(TransactionService transactionService, IMapper mapper)
    {
        _transactionService = transactionService;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("create_new_transaction")]
    public IActionResult CreateNewTransaction([FromBody] TransactionRequestDto transactionRequest)
    {
        if(!ModelState.IsValid) return BadRequest(transactionRequest);

        var transaction = _mapper.Map<Transaction>(transactionRequest);
        return Ok(_transactionService.CreateNewTransaction(transaction));
    }

    [HttpPost]
    [Route("make_deposit")]
    public IActionResult MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
    {
         if(!Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$")) return BadRequest("Account number must be 10-digits");
        return Ok(_transactionService.MakeDeposit(AccountNumber, Amount, TransactionPin));

    }

    [HttpPost]
    [Route("make_withdrawal")]
    public IActionResult MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
    {
        if(!Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$")) return BadRequest("Account number must be 10-digits");
            return Ok(_transactionService.MakeWithdrawal(AccountNumber, Amount, TransactionPin));
    }

    [HttpPost]
    [Route("make_funds_transfer")]
     public IActionResult MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
    {
        if (!Regex.IsMatch(FromAccount, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$") || !Regex.IsMatch(ToAccount, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$")) 
            return BadRequest("Account number must be 10-digits");
       
       return Ok(_transactionService.MakeFundsTransfer(FromAccount, ToAccount, Amount, TransactionPin));
       
    }

}