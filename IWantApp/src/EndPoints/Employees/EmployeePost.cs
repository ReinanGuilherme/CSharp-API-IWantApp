using IWantApp.Domain.Products;
using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Identity;

namespace IWantApp.EndPoints.Employees
{
    public class EmployeePost
    {
        public static string Template => "/employees";
        public static string[] Methoods => new string[] { HttpMethod.Post.ToString() };
        public static Delegate Handle => Action;

        //para salvar o usuario é necessario adicionar o UserManager<IdentityUser>
        public static IResult Action(EmployeeRequest employeeRequest, UserManager<IdentityUser> userManager)
        {
            var user = new IdentityUser { UserName = employeeRequest.Email, Email = employeeRequest.Email };
            //o password deverá ser passado na função abaixo.
            var result = userManager.CreateAsync(user, employeeRequest.Password).Result;

            //verificando se o usuario foi salvo com sucesso.
            if (!result.Succeeded)
            {
                return Results.BadRequest(result.Errors.First());
            }

            return Results.Created($"/employees/{user.Id}", user.Id);
        }
    }
}
