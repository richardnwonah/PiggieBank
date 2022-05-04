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
public class AccountsController : ControllerBase
{
    private IAccountService _accountService;
    IMapper _mapper;

    public AccountsController(AccountService accountService, IMapper mapper)
    {
        _accountService = accountService;
        _mapper = mapper;
    }

    //registerNewAccount
    [HttpPost]
    [Route("register_new_account")]
    public IActionResult RegisterNewAccount([FromBody] RegisterNewAccountModel newAccount)
    {
        if(!ModelState.IsValid) return BadRequest(newAccount);
        
        //map to account
        var account = _mapper.Map<Account>(newAccount);
        return Ok(_accountService.Create(account, newAccount.Pin, newAccount.ConfirmPin));
    }

    [HttpGet]
    [Route("get_all_accounts")]
    public IActionResult GetAllAccount()
    {
        var accounts = _accountService.GetAllAccounts();
        var cleanedAccounts = _mapper.Map<IList<GetAccountModel>>(accounts);
        return Ok(cleanedAccounts);
    }

    [HttpPost]
    [Route("authenticate")]
    public IActionResult Authenticate([FromBody] AuthenticateModel model)
    {
        if (!ModelState.IsValid) return BadRequest(model);

        return Ok(_accountService.Authenticate(model.AccountNumber, model.Pin));
        //returnsa an account.
    }

    [HttpGet]
    [Route("get_by_account_number")]
    public IActionResult GetByAccountNumber(string AccountNumber)
    {
        if(!Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d(9)$")) return BadRequest("Account number must be 10-digits");

        var account = _accountService.GetByAccountNumber(AccountNumber);
        var cleanedAccount = _mapper.Map<GetAccountModel>(account);
        return Ok(cleanedAccount);
    }

    [HttpGet]
    [Route("get_account_by_id")]
    public IActionResult GetAccountById(int Id)
    {

        var account = _accountService.GetById(Id);
        var cleanedAccount = _mapper.Map<GetAccountModel>(account);
        return Ok(cleanedAccount);
    }

    [HttpPost]
    [Route("update_account")]
    public IActionResult UpdateAccount([FromBody] UpdateAccountModel model, string Pin)
    {
        if (!ModelState.IsValid) return BadRequest(model);

        var account = _mapper.Map<Account>(model);
        _accountService.Update(account, model.Pin);
        return Ok();
    }
}
