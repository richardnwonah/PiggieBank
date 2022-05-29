using PiggyBank.DAL;
using Microsoft.OpenApi.Models;
using PiggyBank.Services.Interfaces;
using PiggyBank.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using PiggyBank.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PiggyBankDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
builder.Services.Configure<AppSettings>(System.Configuration.Configuration.GetSection("AppSettings"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
    {
       x.SwaggerDoc("v1", new OpenApiInfo
      {
        Title = "PiggyBank banking api",
          Version = "v1",
          Description = "Banking api with basic functionalities",
          Contact = new OpenApiContact
          {
               Name = "Richard Nwonah",
               Email = "richardnwonah@outlook.com",
               Url = new Uri("https://github.com/richardnwonah/PiggieBank")
    
            }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        //x =>
    //{
      //  var profix = string.IsNullOrEmpty(x.RouteProfile) ? "." : "..";
       // x.SwaggerEndpoint($"{prefix}/swagger/v1/swagger.json", "PiggyBanking Api doc");

    //}
    );
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
