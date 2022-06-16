using CleanArchitecture.WebUI.Infrastructure.Configuration;
using CleanArchitecture.WebUI.Services.Api;
using Microsoft.Extensions.Options;
using SFA.DAS.HashingService;
using SFA.DAS.Http;

namespace CleanArchitecture.WebUI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddClient<IApiService>((c, s) => new ApiService(c, s.GetRequiredService<IHashingService>()));
        return serviceCollection;
    }
    public static IServiceCollection AddHashingService(this IServiceCollection serviceCollection)
    {
        _ = serviceCollection.AddSingleton<IHashingService>(c =>
        {
            var settings = c.GetService<IOptions<WebConfigurationOptions>>()?.Value;
            string hashString = "CleanArchitecture";
            if (settings != null && !string.IsNullOrEmpty(settings.Hashstring))
                hashString = settings.Hashstring;
            return new HashingService("56789BCDFGHJKLMNPRSTVWXY", hashString);
        });

        return serviceCollection;
    }

    private static IServiceCollection AddClient<T>(
        this IServiceCollection serviceCollection,
        Func<HttpClient, IServiceProvider, T> instance) where T : class
    {
        _ = serviceCollection.AddTransient(s =>
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var settings = s.GetService<IOptions<ApiOptions>>().Value;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            var clientBuilder = new HttpClientBuilder()
                .WithDefaultHeaders()
                //.WithApimAuthorisationHeader(settings)
                .WithLogging(s.GetService<ILoggerFactory>());

            var httpClient = clientBuilder.Build();

            if (!settings.ApiBaseUrl.EndsWith("/"))
            {
                settings.ApiBaseUrl += "/";
            }
            httpClient.BaseAddress = new Uri(settings.ApiBaseUrl);

            return instance.Invoke(httpClient, s);
        });

        return serviceCollection;
    }
}
