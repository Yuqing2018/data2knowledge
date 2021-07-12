using Microsoft.AspNetCore.Components.Forms;
using MongoDB.Bson;
using MongoDB.Driver;
using MusicKG.HondaPlugins.DataAccess;
using MusicKG.HondaPlugins.DataAccess.DataModels.Labeling;
using MusicKG.HondaPlugins.Services.Helpers;
using MusicKG.HondaPlugins.Services.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.Services.Repositories.Implementations
{
    public class IgnoredVehicleService : IIgnoredVehicleService
    {
        private readonly IHondaMongoDbContext context;

        public IgnoredVehicleService(IHondaMongoDbContext context)
        {
            this.context = context;
        }

        public async Task<Dictionary<string, List<string>>> ListAsync(string keyword)
        {
            var data = await context.IgnoredVehicle.AsQueryable().FirstOrDefaultAsync();

            if (data == null)
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.IgnoredVehicleNotExists, HttpStatusCode.NotFound);

            return string.IsNullOrWhiteSpace(keyword) ? data.Data : data.Data.Where(v => v.Key.Contains(keyword) || v.Value.Any(carType => carType.Contains(keyword))).ToDictionary(k => k.Key, v => v.Value);
        }

        public async Task AddAsync(string carModel, List<string> carTypes)
        {
            var data = await context.IgnoredVehicle.AsQueryable().FirstOrDefaultAsync();

            if (data == null)
            {
                await context.IgnoredVehicle.InsertOneAsync(new IgnoredVehicle
                {
                    Id = ObjectId.GenerateNewId(),
                    Data = new Dictionary<string, List<string>>
                    {
                        { carModel, carTypes }
                    }
                });
            }
            else
            {
                if (data.Data.ContainsKey(carModel))
                    ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.IgnoredVehicleAlreadyExists, HttpStatusCode.NotFound);

                data.Data.Add(carModel, carTypes);

                await context.IgnoredVehicle.ReplaceOneAsync(v => v.Id == data.Id, data);
            }
        }

        public async Task UpdateAsync(string carModel, List<string> carTypes)
        {
            var data = await context.IgnoredVehicle.AsQueryable().FirstOrDefaultAsync();

            if (data == null || !data.Data.ContainsKey(carModel))
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.IgnoredVehicleNotExists, HttpStatusCode.NotFound);

            data.Data[carModel] = carTypes;

            await context.IgnoredVehicle.ReplaceOneAsync(v => v.Id == data.Id, data);
        }

        public async Task DeleteAsync(string carModel)
        {
            var data = await context.IgnoredVehicle.AsQueryable().FirstOrDefaultAsync();

            if (data == null || !data.Data.ContainsKey(carModel))
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.IgnoredVehicleNotExists, HttpStatusCode.NotFound);

            data.Data.Remove(carModel);

            await context.IgnoredVehicle.ReplaceOneAsync(v => v.Id == data.Id, data);
        }
    }
}
