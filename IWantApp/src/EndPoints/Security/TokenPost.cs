using IWantApp.EndPoints.Employees;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IWantApp.EndPoints.Security
{
    public class TokenPost
    {
        public static string Template => "/token";
        public static string[] Methoods => new string[] { HttpMethod.Post.ToString() };
        public static Delegate Handle => Action;

        //é necessario adicionar o UserManager<IdentityUser>
        public static IResult Action(LoginRequest loginRequest, UserManager<IdentityUser> userManager)
        {
            var user = userManager.FindByEmailAsync(loginRequest.Email).Result;
            if(user == null)
            {
                return Results.BadRequest();
            }
            if(!userManager.CheckPasswordAsync(user, loginRequest.Password).Result)
            {
                return Results.BadRequest();
            }

            //gerando o token
            var key = Encoding.ASCII.GetBytes("A@ferwfQQSDXCCer34");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, loginRequest.Email),
                }),
                SigningCredentials = 
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Audience = "IWantApp",
                    Issuer = "Issuer"
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Results.Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}
