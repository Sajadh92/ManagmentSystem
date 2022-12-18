using AutoMapper;
using Infrastructure.Mapping;
using Infrastructure.ORM;
using Infrastructure.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// For Sql Server Connection 
builder.Services.AddDbContext<DBContext>(option => option.UseSqlServer(DBConn.ConnectionString));

// For PostgreSQL Connection 
//builder.Services.AddDbContext<DBContext>(option => option.UseNpgsql(DBConn.ConnectionString));

builder.Services.Register<ISingleton>();

builder.Services.Register<IScopped>();

builder.Services.AddSingleton(new MapperConfiguration(config => config.AddProfile(new MappingProfile())).CreateMapper());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();