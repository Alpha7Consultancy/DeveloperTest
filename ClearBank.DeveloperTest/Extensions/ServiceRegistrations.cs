using System;
using System.Collections.Generic;
using System.Text;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Helpers;
using ClearBank.DeveloperTest.Setttings;
using Microsoft.Extensions.DependencyInjection;

namespace ClearBank.DeveloperTest.Extensions
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection AddAccountDataStore(this IServiceCollection services, DataStoreSetting dataStoreSetting)
        {
            services.AddScoped<IAccountDataStore>(s =>
            {
                switch (dataStoreSetting.DataStoreType)
                {
                    case SystemConstants.BackupDataStoreType:
                        return s.GetService<BackupAccountDataStore>();
                    default:
                        return s.GetService<AccountDataStore>();
                }
            });

            return services;
        }
    }
}
