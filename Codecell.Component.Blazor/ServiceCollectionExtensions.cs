using Microsoft.Extensions.DependencyInjection;

namespace Codecell.Component.Blazor
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCodecellBlazor(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddScoped<PersianDatePickerJsInterop>();
            return services;
        }
    }
}
