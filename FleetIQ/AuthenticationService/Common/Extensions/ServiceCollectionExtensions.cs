using Microsoft.Extensions.DependencyInjection;
using AuthenticationService.Application.Services;
using AuthenticationService.Infrastructure.Repositories;
using AuthenticationService.Application.Validators;
using FluentValidation;

namespace AuthenticationService.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers application-specific services and dependencies.
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register Services
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ICompanyService, CompanyService>();

            // Register Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();

            // Register Validators
            services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateCompanyValidator>();

            return services;
        }
    }
}
