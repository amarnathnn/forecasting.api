using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PS.CTS.Authentication.Repository;
using PS.CTS.AuthenticationService;
using PS.CTS.Common.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace PS.CTS.Authentication.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly AuthenticationContext _dbContext;
        private string secret = null;
        public AuthenticationRepository(IOptions<Settings> settings, AuthenticationContext dbContext)
        {
            _dbContext = dbContext;
            secret = settings.Value.Secret;
        }

        public LoginInfo Authenticate(string username, string password)
        {
            LoginInfo loginInfo = new LoginInfo();
            bool isMentor = false;
            string userId ="";
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                loginInfo.ErrorMessage = "Please input valid credentials.";
                return loginInfo;
            }
            var user = _dbContext.Users.SingleOrDefault(x => x.UserID == username && x.Password == password);
            if (user == null)
            {
                loginInfo.ErrorMessage = "You are not authorized user to this application. please contact Admin";
                return loginInfo;
            }
            if (user != null)
            {
                userId = user.UserID;
                loginInfo.UserDisplayName = user.FirstName + ", " + user.LastName;
            }            
            //// authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenValue = tokenHandler.WriteToken(token);

            loginInfo.Role = (isMentor) ? "mentor" : "user";
            loginInfo.Token = tokenValue;
            loginInfo.UserId = userId;            
            loginInfo.ErrorMessage =null;
            return loginInfo;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }


    }
}
