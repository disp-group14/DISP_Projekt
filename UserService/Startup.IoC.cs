using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.DAL;
using static BankServiceGrpc.Protos.IBankService;
using static OwnershipServiceGrpc.Protos.IOwnershipService;

namespace UserService
{
    public partial class Startup
    {
        private void ConfigureIoC(IServiceCollection services)
        {
            // Data managers
            services.AddTransient<IUserDataManager, UserDataManager>();

            // Grpc Clients
            services.AddGrpcClient<IBankServiceClient>(client =>
            {
                client.Address = Configuration.GetValue<Uri>("BankServiceUri");
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                return handler;
            });

            services.AddGrpcClient<IOwnershipServiceClient>(client =>
            {
                client.Address = Configuration.GetValue<Uri>("OwnershipServiceUri");
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                return handler;
            });

        }
    }
}