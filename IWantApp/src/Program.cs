using IWantApp.EndPoints.Categories;
using IWantApp.EndPoints.Employees;
using IWantApp.EndPoints.Security;
using IWantApp.Infra.Data;
using IWantApp.Infra.Data.Querys;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["ConnectionStrings:IWantDb"]);
builder.Services.AddIdentity<IdentityUser, IdentityRole>(
    //removendo algumas regras de validação de senha
    options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 3;
        options.Password.RequireLowercase = false;
    })
    //add o context do banco de dados.
    .AddEntityFrameworkStores<ApplicationDbContext>();

//add JWT como serviço
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtBearerTokenSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtBearerTokenSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecretKey"]))
    };
});

//add funcao como serviço
builder.Services.AddScoped<QueryAllUsersWithClaimName>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseAuthorization();
app.UseAuthentication();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapMethods(CategoryPost.Template, CategoryPost.Methoods, CategoryPost.Handle);
app.MapMethods(CategoryGetAll.Template, CategoryGetAll.Methoods, CategoryGetAll.Handle);
app.MapMethods(CategoryPut.Template, CategoryPut.Methoods, CategoryPut.Handle);
app.MapMethods(EmployeePost.Template, EmployeePost.Methoods, EmployeePost.Handle);
app.MapMethods(EmployeeGetAll.Template, EmployeeGetAll.Methoods, EmployeeGetAll.Handle);
app.MapMethods(TokenPost.Template, TokenPost.Methoods, TokenPost.Handle);

app.Run();