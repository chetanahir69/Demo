using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WebApi.Model;

namespace WebApi.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly AppSettings _appSettings;

        public AuthenticateService(IOptions<AppSettings> appsettings)
        {
            _appSettings = appsettings.Value;
        }

        private List<User> lst = new List<User>()
            {
                new User { UserId=1, FirstName="Chetan", LastName="Ahir",UserName="chetan",Password="12345" }
            };

        public User Authenticate(string UserName, string Password)
        {
            var user = lst.Where(x => x.UserName == UserName && x.Password == Password).FirstOrDefault();

            // If User Not Found 
            if (user == null)
                return null;

            // If User Found 

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]{
                     new Claim(ClaimTypes.Name,user.UserId.ToString()),
                     new Claim(ClaimTypes.Role,"Admin"),
                     new Claim(ClaimTypes.Version,"V1")
                    }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = null;
            return user;
        }
    }
}
