﻿using Microsoft.EntityFrameworkCore;
using Wheels_n_Deals.API.DataLayer.Entities;
using Wheels_n_Deals.API.DataLayer.Enums;

namespace Wheels_n_Deals.API.DataLayer.Repositories;

public class FeaturesRepository : BaseRepository<Features>
{
    public FeaturesRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<Features?> GetFeatureFromFeatures(string carBody, uint horsePower, uint engineSize, GearboxType gearbox,
        FuelType fuel)
    {
        var features = GetRecords();

        var searchedFeature = await features
            .FirstOrDefaultAsync(feature =>
                feature.CarBody == carBody &&
                feature.EngineSize == engineSize &&
                feature.FuelType == fuel &&
                feature.HorsePower == horsePower &&
                feature.Gearbox == gearbox
            );

        return searchedFeature;
    }

}