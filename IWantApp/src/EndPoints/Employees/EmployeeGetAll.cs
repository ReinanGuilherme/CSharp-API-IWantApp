using IWantApp.Domain.Products;
using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IWantApp.EndPoints.Employees
{
    public class EmployeeGetAll
    {
        public static string Template => "/employees";
        public static string[] Methoods => new string[] { HttpMethod.Get.ToString() };
        public static Delegate Handle => Action;

        //para salvar o usuario é necessario adicionar o UserManager<IdentityUser>
        //paginacao
        public static IResult Action(int page, int rows, UserManager<IdentityUser> userManager)
        {
            var users = userManager.Users.Skip((page - 1) * rows).Take(rows).ToList();

            //resgatando a lista de claims
            var employees = new List<EmployeeResponse>();
            foreach (var user in users)
            {
                var claims = userManager.GetClaimsAsync(user).Result;
                var claimName = claims.FirstOrDefault(c => c.Type == "Name");
                var userName = claimName != null ? claimName.Value : string.Empty;
                employees.Add(new EmployeeResponse(user.Email, userName));
            }

            var employee = users.Select(u => new EmployeeResponse ( u.Email, "Name" ));

            return Results.Ok(employee);
        }
    }
}
