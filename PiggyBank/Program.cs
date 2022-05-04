using PiggyBank.DAL;
using PiggyBank.Services.Interfaces;
using PiggyBank.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<PiggyBankDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PiggyBankDbContext")));
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    //x =>
    //{
      //  x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        //{
          //  Title = "PiggyBank banking api",
            //Version = "v1",
            //Description = "Banking api with basic functionalities",
            //Contact = new Microsoft.OpenApi.Models.OpenAPIContact
            //{
              //  Name = "Richard Nwonah",
               // Email = "richardnwonah@outlook.com"
    
            //}
    //});
//}
);

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
