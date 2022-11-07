using Dapper;
using IWantApp.Infra.Data.Querys;
using Microsoft.Data.SqlClient;

namespace IWantApp.EndPoints.Employees
{
    public class EmployeeGetAll
    {
        public static string Template => "/employees";
        public static string[] Methoods => new string[] { HttpMethod.Get.ToString() };
        public static Delegate Handle => Action;

        public static IResult Action(int? page, int? rows, QueryAllUsersWithClaimName query)
        {           

            return Results.Ok(query.Execute(page.Value, rows.Value));
        }
    }
}
