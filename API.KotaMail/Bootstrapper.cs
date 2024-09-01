using API.Repository;
using API.RepositoryContract;
using API.Service;
using API.ServiceContract;

namespace API.KotaMail
{
    public class Bootstrapper
    {
        public static void SetupRepositories(IServiceCollection services)
        {
            services.AddScoped<IConnectionRepository, ConnectionRepository>();
            services.AddScoped<IConnectionListRepository, ConnectionListRepository>();
            services.AddScoped<IConnectionDetailRepository, ConnectionDetailRepository>();
        }

        public static void SetupServices(IServiceCollection services)
        {
            services.AddScoped<IConnectionService, ConnectionService>();
            services.AddScoped<IConnectionListService, ConnectionListService>();
            services.AddScoped<IConnectionDetailService, ConnectionDetailService>();
            services.AddScoped<IMailboxService, MailboxService>();
        }
    }
}
