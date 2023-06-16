using AutoMapper;
using MagicVilla_CouponAPI.Models;
using MagicVilla_CouponAPI.Models.ViewModels;
using MagicVilla_CouponAPI.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_CouponAPI.Repositories.Concrete
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly String _secret_key;

        public AuthRepository(ApplicationDbContext dbContext,
                                IMapper mapper,
                                IConfiguration configuration)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            this._configuration = configuration;
            this._secret_key = _configuration.GetValue<string>("ApiSettings:SecretKey");
        }

        public bool IsUserUnique(string username)
        {
            if(_dbContext.LocalUsers.Count() == 0)
                return false;
            var doesExist = _dbContext.LocalUsers.Any(x => x.UserName.Equals(username,
                                                StringComparison.OrdinalIgnoreCase));
            return doesExist;
        }

        public async Task<LoginResponseVM> Login(LoginRequestVM loginRequest)
        {
            var users = await _dbContext.LocalUsers.ToListAsync();
            var user = await _dbContext.LocalUsers.SingleOrDefaultAsync(x =>
                            x.UserName.Equals(loginRequest.UserName) &&
                            x.Password.Equals(loginRequest.Password));

            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret_key);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponseVM loginResponse = new()
            {
                User = _mapper.Map<UsersVM>(user),
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
            return loginResponse;
        }

        public async Task<UsersVM> Register(RegistrationRequestVM registrationRequest)
        {
            var user = _mapper.Map<LocalUser>(registrationRequest);
            user.Role = "Admin";
            await _dbContext.AddAsync(user);
            var userVM = _mapper.Map<UsersVM>(user);
            return userVM;
        }
    }
}
