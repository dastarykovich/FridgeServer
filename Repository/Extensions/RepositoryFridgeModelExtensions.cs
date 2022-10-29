using Entities.Models;
using Repository.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Repository.Extensions
{
    public static class RepositoryFridgeModelExtensions
    {
        public static IQueryable<FridgeModel> FilterFridgeModels(this IQueryable<FridgeModel>
            fridgeModels, uint minYear, uint maxYear) =>
            fridgeModels.Where(i => (i.Year >= minYear && i.Year <= maxYear));

        public static IQueryable<FridgeModel> Search(this IQueryable<FridgeModel> fridgeModels,
            string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return fridgeModels;
            }

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return fridgeModels.Where(i => i.Name!.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<FridgeModel> Sort(this IQueryable<FridgeModel> fridgeModels,
            string? orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return fridgeModels.OrderBy(i => i.Name);
            }

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<FridgeModel>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                return fridgeModels.OrderBy(i => i.Name);
            }

            return fridgeModels.OrderBy(orderQuery);
        }
    }
}
