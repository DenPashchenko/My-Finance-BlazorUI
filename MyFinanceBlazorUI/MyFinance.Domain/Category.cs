using System.ComponentModel.DataAnnotations;

namespace MyFinance.Domain
{
    public class Category
    {
        public int Id { get; private set; }

        public string Name { get; set; } = null!;
    }
}
