using AutoMapper;
using MyFinance.Application.Common.Mappings;
using MyFinance.Domain;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.Transactions.Queries.GetTransactionList
{
    public class TransactionListDto : IMapWith<Transaction>
    {
        public int Id { get; set; }
        public TransactionType TransactionType { get; set; }

        public int CategoryId { get; set; }

        public string Category { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public decimal Sum { get; set; }

        public DateTime DateOfCreation { get; set; }

        public DateTime? DateOfEditing { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Transaction, TransactionListDto>()
                .ForMember(transactionVm => transactionVm.Category, opt => opt.MapFrom(transaction => transaction.Category.Name));
        }
    }
}
