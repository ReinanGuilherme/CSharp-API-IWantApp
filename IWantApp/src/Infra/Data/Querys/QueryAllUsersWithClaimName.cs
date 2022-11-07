using Dapper;
using IWantApp.EndPoints.Employees;
using Microsoft.Data.SqlClient;

namespace IWantApp.Infra.Data.Querys
{
    public class QueryAllUsersWithClaimName
    {
        private readonly IConfiguration configuration;

        public QueryAllUsersWithClaimName(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IEnumerable<EmployeeRequest> Execute(int page, int rows)
        {
            //configurando string de conexao
            var db = new SqlConnection(configuration["ConnectionStrings:IWantDb"]);

            //scrip para usar com Dapper
            string script = @"select Email, ClaimValue
                                from AspNetUsers u 
                                inner join AspNetUserClaims c on u.id = c.UserId and claimtype = 'Name'
                                order by Name";

            //Dapper realizando consulta ao banco de dados e retornando a consulta ja convertida para o tipo de objeto selecionado.
            return db.Query<EmployeeRequest>(script);
        }
    }
}
