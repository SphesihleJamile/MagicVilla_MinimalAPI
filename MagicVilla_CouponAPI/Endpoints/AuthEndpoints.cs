using FluentValidation;
using MagicVilla_CouponAPI.Models.ViewModels;
using MagicVilla_CouponAPI.Models;
using MagicVilla_CouponAPI.Repositories.Abstract;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_CouponAPI.Endpoints
{
    public static class AuthEndpoints
    {
        private async static Task<IResult> Register([FromBody] RegistrationRequestVM registrationRequest,
                    IValidator<RegistrationRequestVM> _validator,
                    IUnitOfWork _unitOfWork)
        {
            APIResponse response = new();
            try
            {
                var validationResponse = await _validator.ValidateAsync(registrationRequest);
                if(!validationResponse.IsValid)
                {
                    response.IsSuccessful = false;
                    response.Errors = validationResponse.Errors;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return Results.BadRequest(response);
                }

                string username = registrationRequest.UserName;
                var isUsernameUnique = _unitOfWork.AuthRepository.IsUserUnique(registrationRequest.UserName);
                if (!isUsernameUnique)
                {
                    var usersVM = await _unitOfWork.AuthRepository.Register(registrationRequest);
                    await _unitOfWork.SaveChangesAsync();
                    response.IsSuccessful = true;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Result = usersVM;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.StatusCode = HttpStatusCode.Conflict;
                    response.ErrorMessage = "Username already exists";
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

        private async static Task<IResult> Login([FromBody] LoginRequestVM loginRequest, 
                    IValidator<LoginRequestVM> _validator, 
                    IUnitOfWork _unitOfWork)
        {
            APIResponse response = new();
            try
            {
                var validationResponse = await _validator.ValidateAsync(loginRequest);
                if (!validationResponse.IsValid)
                {
                    response.IsSuccessful = false;
                    response.Errors = validationResponse.Errors;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return Results.BadRequest(response);
                }

                var loginResponse = await _unitOfWork.AuthRepository.Login(loginRequest);
                if(loginResponse == null)
                {
                    response.IsSuccessful = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.ErrorMessage = "Invalid username or password";
                    return Results.BadRequest(response);
                }
                else
                {
                    response.IsSuccessful = true;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Result = loginResponse;
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

        public static void ConfigureAuthEndpoints(this WebApplication app)
        {
            app.MapPost("/api/login", Login)
                .WithName("Login")
                .Accepts<LoginRequestVM>("application/json")
                .Produces<APIResponse>(200)
                .Produces<APIResponse>(400);

            app.MapPost("/api/register", Register)
                .WithName("Register")
                .Accepts<RegistrationRequestVM>("application/json")
                .Produces<APIResponse>(200)
                .Produces<APIResponse>(400);
        }
    }
}
