using FluentValidation.Results;
using System.Net;

namespace MagicVilla_CouponAPI.Models
{
    public class APIResponse
    {
        public bool IsSuccessful { get; set; } = true;
        public Object Result { get; set; } = null;
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.Continue;
        public List<ValidationFailure> Errors { get; set; }
        public String ErrorMessage { get; set; } = string.Empty;
        public APIResponse()
        {
            Errors = new();
        }
    }
}
