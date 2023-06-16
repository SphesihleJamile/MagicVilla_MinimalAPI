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

app.MapGet("/api/Coupon/GetAll",
    async (ILogger<Program> _logger,
        IUnitOfWork unitOfWork) =>
{
    APIResponse response = new APIResponse();
    try
    {
        _logger.LogInformation("Getting All Coupons");
        var data = await unitOfWork.CouponRepository.GetAllAsync();
        if (data == null || !data.Any())
        {
            response.IsSuccessful = true;
            response.Result = null;
            response.StatusCode = HttpStatusCode.NoContent;
        }
        else
        {
            response.IsSuccessful = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Result = data;
        }
    }catch(Exception ex)
    {
        response.IsSuccessful = false;
        response.StatusCode = HttpStatusCode.InternalServerError;
        response.Result = ex;
        response.ErrorMessage = ex.Message;
    }
    return Results.Ok(response);
})
    .WithName("GetCoupons")
    .Produces<APIResponse>(200);

app.MapGet("/api/Coupon/Get/{id:int}", 
    async ([FromRoute] int id,
        IUnitOfWork unitOfWork) =>
{
    APIResponse response = new APIResponse();
    try
    {
        var data = await unitOfWork.CouponRepository.GetAsync(id);
        if (data == null)
        {
            response.IsSuccessful = false;
            response.StatusCode = HttpStatusCode.NotFound;
            response.ErrorMessage = $"Coupon with id ({id}) could not be found";
            return Results.NotFound(response);
        }
        else
        {
            response.IsSuccessful = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Result = data;
        }
    }catch(Exception ex)
    {
        response.IsSuccessful = false;
        response.StatusCode = HttpStatusCode.InternalServerError;
        response.Result = ex;
        response.ErrorMessage = ex.Message;
    }
    return Results.Ok(response);
})
    .WithName("GetCoupon")
    .Produces<APIResponse>(200);

app.MapPost("/api/Coupon/Create", 
    async ([FromBody] CouponCreateVM coupon, 
        IValidator<CouponCreateVM> _validator,
        IUnitOfWork unitOfWork) =>
{
    APIResponse response = new();
    try
    {
        var validationResult = await _validator.ValidateAsync(coupon);
        if (validationResult.IsValid)
        {
            await unitOfWork.CouponRepository.CreateAsync(coupon);
            response.IsSuccessful = true;
            response.StatusCode = HttpStatusCode.OK;
        }
        else
        {
            response.IsSuccessful = false;
            response.StatusCode = HttpStatusCode.BadRequest;
            response.Errors = validationResult.Errors;
            return Results.BadRequest(response);
        }
    }
    catch(Exception ex)
    {
        response.IsSuccessful = false;
        response.StatusCode = HttpStatusCode.InternalServerError;
        response.Result = ex;
        response.ErrorMessage = ex.Message;
    }
    return Results.Ok(response);
})
    .WithName("CreateCoupon")
    .Accepts<CouponCreateVM>("application/json")
    .Produces<APIResponse>(200);

app.MapPut("/api/Coupon/Update/{id:int}", 
    async ([FromRoute] int id,
        [FromBody] CouponUpdateVM couponUpdate, 
        IValidator<CouponUpdateVM> _validator,
        IUnitOfWork unitOfWork) =>
{
    APIResponse response = new();
    try
    {
        var validationResult = await _validator.ValidateAsync(couponUpdate);
        if(validationResult.IsValid)
        {
            var processingResponse = await unitOfWork.CouponRepository.UpdateAsync(id, couponUpdate);
            if(processingResponse)
            {
                response.IsSuccessful = true;
                response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                response.IsSuccessful = false;
                response.StatusCode = HttpStatusCode.NotFound;
                response.ErrorMessage = $"Coupon with id ({id}) could not be found";
                return Results.NotFound(response);
            }
        }
        else
        {
            response.IsSuccessful = false;
            response.StatusCode = HttpStatusCode.BadRequest;
            response.Errors = validationResult.Errors;
            return Results.BadRequest(response);
        }
    }catch(Exception ex)
    {
        response.IsSuccessful = false;
        response.StatusCode = HttpStatusCode.InternalServerError;
        response.Result = ex;
        response.ErrorMessage = ex.Message;
    }
    return Results.Ok(response);
})
    .WithName("UpdateCoupon")
    .Produces<APIResponse>(200);

app.MapDelete("/api/Coupon/Delete/{id:int}", (int id) =>
{

})
    .WithName("DeleteCoupon")
    .Produces<APIResponse>(200);

app.UseHttpsRedirection();

app.Run();