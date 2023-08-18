namespace Wheels_n_Deals.API.DataLayer.DTO;

public record VehicleDto(
    Guid Id,
    string VinNumber,
    string Make,
    string Model,
    uint Year,
    uint Mileage,
    string TechnicalState,
    float PriceInEuro,
    float PriceInRon,
    string CarBody,
    string FuelType,
    uint EngineSize,
    string Gearbox,
    uint HorsePower
);