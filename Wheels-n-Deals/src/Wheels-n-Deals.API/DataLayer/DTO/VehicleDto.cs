namespace Wheels_n_Deals.API.DataLayer.DTO;

public record VehicleDto(
    Guid Id,
    string VinNumber,
    string Make,
    string Model,
    uint Year,
    uint Mileage,
    float PriceInEuro,
    float PriceInRon,
    string TechnicalState,
    string CarBody,
    string FuelType,
    uint EngineSize,
    string Gearbox,
    uint HorsePower
);