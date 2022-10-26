using Flunt.Validations;
using IWantApp.EndPoints.Categories;
using System.Xml.Linq;

namespace IWantApp.Domain.Products
{
    public class Category: Entity
    {
        public string Name { get; set; }
        public bool Active { get; set; }

        public Category()
        {

        }

        public Category(string name, string createBy, string editedBy)
        {
            Name = name;
            Active = true;
            CreatedBy = createBy;
            EditedBy = editedBy;
            CreatedOn = DateTime.Now;
            EditedOn = DateTime.Now;

            Validade();
        }

        private void Validade()
        {
            var contract = new Contract<Category>()
                .IsNotNullOrEmpty(Name, "Name")
                .IsNotNullOrWhiteSpace(Name, "NameSpace")
                .IsGreaterOrEqualsThan(Name, 3, "NameLenght");
            AddNotifications(contract);
        }

        public void EditInfo(string name, bool active)
        {
            Name = name;
            Active = active;

            Validade();
        }
    }
}
