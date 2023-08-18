using System.Linq.Expressions;
using Wheels_n_Deals.API.DataLayer.DTO;
using Wheels_n_Deals.API.DataLayer.Models;

namespace Wheels_n_Deals.API.DataLayer.Utils;

public static class VehicleExtensions
{
    public static Expression<Func<Vehicle, bool>> BuildFilterExpression(VehicleFiltersDto? vehicleFilters)
    {
        Expression<Func<Vehicle, bool>> filterExpression = v => true; // Start with a default condition

        if (vehicleFilters is null)
            return filterExpression;

        if (vehicleFilters.Make.Any())
            filterExpression = AddFilterCondition(filterExpression, v => vehicleFilters.Make.Contains(v.Make));

        if (vehicleFilters.Model.Any())
            filterExpression = AddFilterCondition(filterExpression, v => vehicleFilters.Model.Contains(v.Model));

        if (vehicleFilters.CarBody.Any())
            filterExpression = AddFilterCondition(filterExpression, v => v.Feature != null && vehicleFilters.CarBody.Contains(v.Feature.CarBody));

        if (vehicleFilters.FuelType.Any())
            filterExpression = AddFilterCondition(filterExpression, v => v.Feature != null && vehicleFilters.FuelType.Contains(v.Feature.Fuel));

        if (vehicleFilters.Gearbox.Any())
            filterExpression = AddFilterCondition(filterExpression, v => v.Feature != null && vehicleFilters.Gearbox.Contains(v.Feature.Gearbox));

        if (vehicleFilters.MinYear is not null)
            filterExpression = AddFilterCondition(filterExpression, v => v.Year >= vehicleFilters.MinYear);

        if (vehicleFilters.MaxYear is not null)
            filterExpression = AddFilterCondition(filterExpression, v => v.Year <= vehicleFilters.MaxYear);

        if (vehicleFilters.MinMileage is not null)
            filterExpression = AddFilterCondition(filterExpression, v => v.Mileage >= vehicleFilters.MinMileage);

        if (vehicleFilters.MaxMileage is not null)
            filterExpression = AddFilterCondition(filterExpression, v => v.Mileage <= vehicleFilters.MaxMileage);

        if (vehicleFilters.MinPrice is not null)
            filterExpression = AddFilterCondition(filterExpression, v => v.PriceInEuro >= vehicleFilters.MinPrice);

        if (vehicleFilters.MaxPrice is not null)
            filterExpression = AddFilterCondition(filterExpression, v => v.PriceInEuro <= vehicleFilters.MaxPrice);

        if (vehicleFilters.MinEngineSize is not null)
            filterExpression = AddFilterCondition(filterExpression, v => v.Feature != null && v.Feature.EngineSize >= vehicleFilters.MinEngineSize);

        if (vehicleFilters.MaxEngineSize is not null)
            filterExpression = AddFilterCondition(filterExpression, v => v.Feature != null && v.Feature.EngineSize <= vehicleFilters.MaxEngineSize);

        if (vehicleFilters.MinHorsePower is not null)
            filterExpression = AddFilterCondition(filterExpression, v => v.Feature != null && v.Feature.HorsePower >= vehicleFilters.MinHorsePower);

        if (vehicleFilters.MaxHorsePower is not null)
            filterExpression = AddFilterCondition(filterExpression, v => v.Feature != null && v.Feature.HorsePower <= vehicleFilters.MaxHorsePower);

        return filterExpression;
    }

    private static Expression<Func<Vehicle, bool>> AddFilterCondition(Expression<Func<Vehicle, bool>> baseExpression, Expression<Func<Vehicle, bool>> additionalCondition)
    {
        var parameter = Expression.Parameter(typeof(Vehicle));
        var combinedBody = Expression.AndAlso(
            Expression.Invoke(baseExpression, parameter),
            Expression.Invoke(additionalCondition, parameter)
        );

        return Expression.Lambda<Func<Vehicle, bool>>(combinedBody, parameter);
    }
}
