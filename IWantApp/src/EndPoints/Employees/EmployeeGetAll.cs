using Dapper;
using Microsoft.Data.SqlClient;

namespace IWantApp.EndPoints.Employees
{
    public class EmployeeGetAll
    {
        public static string Template => "/employees";
        public static string[] Methoods => new string[] { HttpMethod.Get.ToString() };
        public static Delegate Handle => Action;

        public static IResult Action(int page, int rows, IConfiguration configuration)
        {
            var db = new SqlConnection(configuration["ConnectionStrings:IWantDb"]);

            string script = @"select Email, ClaimValue
                                from AspNetUsers u 
                                inner join AspNetUserClaims c on u.id = c.UserId and claimtype = 'Name'";

            var employees = db.Query<EmployeeRequest>(script);
        }
    }
}
