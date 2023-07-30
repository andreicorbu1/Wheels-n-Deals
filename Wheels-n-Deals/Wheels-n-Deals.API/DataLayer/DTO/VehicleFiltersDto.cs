namespace Wheels_n_Deals.API.DataLayer.DTO;

public record VehicleFiltersDto(
    string? Make,
    string? Model,
    uint? MinYear,
    uint? MaxYear,
    uint? MinMileage,
    uint? MaxMileage,
    float? MinPrice,
    float? MaxPrice,
    string? CarBody,
    uint? MinEngineSize,
    uint? MaxEngineSize,
    string? FuelType,
    string? Gearbox,
    uint? MinHorsePower,
    uint? MaxHorsePower
);
