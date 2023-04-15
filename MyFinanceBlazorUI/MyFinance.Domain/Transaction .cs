using MyFinance.Domain.Enums;

namespace MyFinance.Domain
{
    public class Transaction
    {
        public int Id { get; private set; }

        public TransactionType TransactionType { get; set; }

        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Sum { get; set; }

        public DateTime DateOfCreation { get; set; }

        public DateTime? DateOfEditing { get; set; }
    }
}
