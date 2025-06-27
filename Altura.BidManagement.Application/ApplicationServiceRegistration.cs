using Altura.BidManagement.Infrastructure;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Altura.BidManagement.Application;

public static class ApplicationServiceRegistration
{
    public static void AddApplicationServices(
        this IServiceCollection services,
        string connectionString)
    {
        var applicationService = typeof(ApplicationServiceRegistration);
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(applicationService.Assembly);
        });
        services.AddValidatorsFromAssembly(applicationService.Assembly);
        services.AddInfrastructureServices(connectionString);
    }
}