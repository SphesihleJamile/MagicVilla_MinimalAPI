using MagicVilla_CouponAPI.Models;
using MagicVilla_CouponAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MagicVilla_CouponAPI.Models.Profiles;
using AutoMapper;
using MagicVilla_CouponAPI.Models.ViewModels;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/Coupon/GetAll", (ILogger<Program> _logger, IMapper mapper) =>
{
    _logger.LogInformation("Getting All Coupons");
    var data = DataStore.GetAsync(mapper).Result?.ToList();
    if (data == null || !data.Any())
        return Results.NoContent();
    return Results.Ok(data);
}).WithName("GetCoupons")
  .Produces<IEnumerable<CouponVM>>(200)
  .Produces(204);

app.MapGet("/api/Coupon/Get/{id:int}", (int id, IMapper mapper) =>
{
    var data = DataStore.GetAsync(mapper).Result.FirstOrDefault(x => x.Id == id);
    if(data == null)
        return Results.NotFound();
    return Results.Ok(data);
}).WithName("GetCoupon")
  .Produces<CouponVM>(200)
  .Produces(404);

app.MapPost("/api/Coupon/Create", ([FromBody] CreateCouponVM coupon, IMapper mapper) =>
{
    if(string.IsNullOrEmpty(coupon.Name) || coupon.Percent == 0)
    {
        return Results.BadRequest("Invalid Coupon Name or Percent");
    }

    try
    {
        DataStore.CreateAsync(mapper, coupon).Wait();
        return Results.Ok(coupon);
    }catch(Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
}).WithName("CreateCoupon")
  .Accepts<CreateCouponVM>("application/json")
  .Produces<CouponVM>(201)
  .Produces(400);

app.MapPut("/api/Coupon/Update", () =>
{

}).WithName("UpdateCoupon")
  .Produces<CouponVM>(200)
  .Produces(404);

app.MapDelete("/api/Coupon/Delete/{id:int}", (int id) =>
{

}).WithName("DeleteCoupon")
  .Produces<CouponVM>(200)
  .Produces(404);

app.UseHttpsRedirection();

app.Run();