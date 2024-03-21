using BookStore.Data;
using BookStore.Model;
using BookStore.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddIdentity<BookStoreUser, IdentityRole>().AddEntityFrameworkStores<BookStoreDBContext>().AddDefaultTokenProviders();

builder.Services.AddTransient<IBookRepository, BookRepository>();
builder.Services.AddTransient<IAccountRepository, AccountRepository>();

builder.Services.AddAutoMapper(typeof(Program));

var provider = builder.Services.BuildServiceProvider();
var config = provider.GetRequiredService<IConfiguration>();
builder.Services.AddDbContext<BookStoreDBContext>(option => option.UseSqlServer(config.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
