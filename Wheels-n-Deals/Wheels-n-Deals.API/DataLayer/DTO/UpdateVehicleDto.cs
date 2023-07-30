namespace Wheels_n_Deals.API.DataLayer.DTO;

public record UpdateVehicleDto(
    string VinNumber,
    string Make,
    string Model,
    uint Year,
    uint Mileage,
    string TechnicalState,
    float PriceInEuro,
    string CarBody,
    string FuelType,
    uint EngineSize,
    string Gearbox,
    uint HorsePower
);
