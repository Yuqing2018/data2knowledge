using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.OperationTool.CommandActions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommandExecutors(this IServiceCollection services)
        {
            return services.AddScoped<IUserOperator, HondaUserOperator>()
                .AddScoped<IModelOperator, HondaModelOperator>();
        }
    }
}
