using AngularProjectAPI.Data;
using AngularProjectAPI.Helpers;
using AngularProjectAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AngularProjectAPI.Services
{
    public class UserService : IUcommentService
    {
        private readonly AppSettings _appSettings;
        private readonly NewsContext _newsContext;

        public UserService(IOptions<AppSettings> appSettings, NewsContext newsContext)
        {
            _appSettings = appSettings.Value;
            _newsContext = newsContext;
        }

        public User Authenticate(string username, string password)
        {
            var user = _newsContext.Users.Include(r=>r.Role).SingleOrDefault(x => x.UserName == username && x.Password == password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("userID", user.UserID.ToString()),
                    new Claim("email", user.Email),
                    new Claim("userName", user.UserName),
                    new Claim("roleID", user.RoleID.ToString()),
                    new Claim("role", user.Role.Name)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning3
            user.Password = null;

            return user;
        }

        public async Task<User> PostUser(User user)
        {
            try
            {
                var role = _newsContext.Roles.Find(user.RoleID);

                user.Role = role;
                _newsContext.Users.Add(user);
                await _newsContext.SaveChangesAsync();
                return user;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
