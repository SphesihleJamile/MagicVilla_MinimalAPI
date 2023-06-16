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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/Coupon/GetAll", async (ILogger<Program> _logger, IMapper mapper) =>
{
    _logger.LogInformation("Getting All Coupons");
    var data = (await DataStore.GetAsync(mapper))?.ToList();
    if (data == null || !data.Any())
        return Results.NoContent();
    return Results.Ok(data);
}).WithName("GetCoupons")
  .Produces<IEnumerable<CouponReadVM>>(200)
  .Produces(204);

app.MapGet("/api/Coupon/Get/{id:int}", async (int id, IMapper mapper) =>
{
    var data = (await DataStore.GetAsync(mapper))?.FirstOrDefault(x => x.Id == id);
    if(data == null)
        return Results.NotFound();
    return Results.Ok(data);
})
    .WithName("GetCoupon")
    .Produces<CouponReadVM>(200)
    .Produces(404);

app.MapPost("/api/Coupon/Create", async (
    [FromBody] CouponCreateVM coupon, 
    IValidator<CouponCreateVM> _validator,
    IMapper mapper) =>
{

    var validationResult = await _validator.ValidateAsync(coupon);

    if (validationResult.IsValid)
    {
        try
        {
            await DataStore.CreateAsync(mapper, coupon);
            return Results.Ok(coupon);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
    else
    {
        return Results.BadRequest(validationResult.Errors);
    }

    
})
    .WithName("CreateCoupon")
    .Accepts<CouponCreateVM>("application/json")
    .Produces<CouponReadVM>(201)
    .Produces<ValidationFailure>(400);

app.MapPut("/api/Coupon/Update", () =>
{

})
    .WithName("UpdateCoupon")
    .Produces<CouponReadVM>(200)
    .Produces(404);

app.MapDelete("/api/Coupon/Delete/{id:int}", (int id) =>
{

})
    .WithName("DeleteCoupon")
    .Produces<CouponReadVM>(200)
    .Produces(404);

app.UseHttpsRedirection();

app.Run();