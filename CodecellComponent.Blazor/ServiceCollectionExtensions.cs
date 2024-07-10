using Microsoft.Extensions.DependencyInjection;

namespace CodecellComponent.Blazor
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCodecellBlazor(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddScoped<CodecellJsInterop>();
            return services;
        }
    }
}
