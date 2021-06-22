using AmazonAsinTracker.Domain;
using AmazonAsinTracker.Infrastructure.FileStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AmazonAsinTracker.Infrastructure
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IFileStorageProvider>(x => new FileStorageProvider(configuration.GetSection("PersistantFolderLocation").Value));
            services.AddSingleton<IProductAsinRepository, ProductAsinFileRepository>();
            services.AddSingleton<IProductReviewRepository, ProductReviewFileRepository>();
            services.AddSingleton<IAmazonProductReader, AmazonProductReader>();
            return services;
        }
    }
}