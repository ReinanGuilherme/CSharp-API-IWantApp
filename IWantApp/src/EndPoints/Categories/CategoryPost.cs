using IWantApp.Domain.Products;
using IWantApp.Infra.Data;

namespace IWantApp.EndPoints.Categories
{
    public class CategoryPost
    {
        public static string Template => "/categories";
        public static string[] Methoods => new string[] { HttpMethod.Post.ToString() };
        public static Delegate Handle => Action;

        public static IResult Action(CategoryRequest categoryRequest, ApplicationDbContext context)
        {
            var category = new Category();

            category.Name = categoryRequest.Name;
            category.CreatedBy = "Test";
            category.CreatedOn = DateTime.Now;
            category.EditedBy = "Test";
            category.EditedOn = DateTime.Now;

            context.Categories.Add(category);
            context.SaveChanges();

            return Results.Created($"/categories/{category.Id}", category.Id);
        }
    }
}
