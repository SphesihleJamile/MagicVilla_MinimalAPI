using MagicVilla_CouponAPI.Models;
using MagicVilla_CouponAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MagicVilla_CouponAPI.Models.Profiles;
using AutoMapper;
using MagicVilla_CouponAPI.Models.ViewModels;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Results;
using System.Net;
using MagicVilla_CouponAPI.Repositories.Abstract;
using MagicVilla_CouponAPI.Repositories.Concrete;
using MagicVilla_CouponAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddAutoMapper(typeof(ModelProfiles));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddScoped<ICouponRepository, CouponRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureCouponEndpoints();

app.UseHttpsRedirection();

app.Run();