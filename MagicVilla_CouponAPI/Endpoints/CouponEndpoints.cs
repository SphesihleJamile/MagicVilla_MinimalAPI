using FluentValidation;
using MagicVilla_CouponAPI.Models.ViewModels;
using MagicVilla_CouponAPI.Models;
using MagicVilla_CouponAPI.Repositories.Abstract;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_CouponAPI.Endpoints
{
    public static class CouponEndpoints
    {

        private async static Task<IResult> GetAll(IUnitOfWork unitOfWork)
        {
            APIResponse response = new APIResponse();
            try
            {
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
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Result = ex;
                response.ErrorMessage = ex.Message;
            }
            return Results.Ok(response);
        }
        private async static Task<IResult> Get([FromRoute] int id, IUnitOfWork unitOfWork)
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
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Result = ex;
                response.ErrorMessage = ex.Message;
            }
            return Results.Ok(response);
        }
        private async static Task<IResult> Create([FromBody] CouponCreateVM coupon, 
                    IValidator<CouponCreateVM> _validator, 
                    IUnitOfWork unitOfWork)
        {
            APIResponse response = new();
            try
            {
                var validationResult = await _validator.ValidateAsync(coupon);
                if (validationResult.IsValid)
                {
                    await unitOfWork.CouponRepository.CreateAsync(coupon);
                    await unitOfWork.SaveChangesAsync();
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
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Result = ex;
                response.ErrorMessage = ex.Message;
            }
            return Results.Ok(response);
        }
        private async static Task<IResult> Update([FromRoute] int id,
                    [FromBody] CouponUpdateVM couponUpdate,
                    IValidator<CouponUpdateVM> _validator,
                    IUnitOfWork unitOfWork)
        {
            APIResponse response = new();
            try
            {
                var validationResult = await _validator.ValidateAsync(couponUpdate);
                if (validationResult.IsValid)
                {
                    var processingResponse = await unitOfWork.CouponRepository.UpdateAsync(id, couponUpdate);
                    await unitOfWork.SaveChangesAsync();
                    if (processingResponse)
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
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Result = ex;
                response.ErrorMessage = ex.Message;
            }
            return Results.Ok(response);
        }
        private async static Task<IResult> Delete([FromRoute] int id, IUnitOfWork unitOfWork)
        {
            APIResponse response = new();
            try
            {
                var result = await unitOfWork.CouponRepository.DeleteAsync(id);
                await unitOfWork.SaveChangesAsync();
                if (result)
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
            }catch(Exception ex)
            {
                response.IsSuccessful = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Result = ex;
                response.ErrorMessage = ex.Message;
            }
            return Results.Ok(response);
        }
        public static void ConfigureCouponEndpoints(this WebApplication app)
        {
            app.MapGet("/api/Coupon/GetAll", GetAll)
                .WithName("GetCoupons")
                .Produces<APIResponse>(200);

            app.MapGet("/api/Coupon/Get/{id:int}", Get)
                .WithName("GetCoupon")
                .Produces<APIResponse>(200);

            app.MapPost("/api/Coupon/Create", Create)
                .WithName("CreateCoupon")
                .Accepts<CouponCreateVM>("application/json")
                .Produces<APIResponse>(200);

            app.MapPut("/api/Coupon/Update/{id:int}", Update)
                .WithName("UpdateCoupon")
                .Produces<APIResponse>(200);

            app.MapDelete("/api/Coupon/Delete/{id:int}", Delete)
                .WithName("DeleteCoupon")
                .Produces<APIResponse>(200);
        }
    }
}
