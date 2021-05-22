using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShareBrokerService.SAL;
using static SalesServiceGrpc.Protos.ISalesService;
using static PurchaseServiceGrpc.Protos.IPurchaseService;
using static TransactionService.Protos.ITransactionService;

namespace ShareBroker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // IoC
            services.AddGrpc();
            
            // Sales Service
            services.AddGrpcClient<ISalesServiceClient>(client => {
                client.Address = Configuration.GetValue<Uri>("SalesServiceUri");
            })
            .ConfigurePrimaryHttpMessageHandler(() => {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                return handler;
            });

            // Purchase Service
            services.AddGrpcClient<IPurchaseServiceClient>(client => {
                client.Address = Configuration.GetValue<Uri>("PurchaseServiceUri");
            })
            .ConfigurePrimaryHttpMessageHandler(() => {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                return handler;
            });

            // Transaction service
            services.AddGrpcClient<ITransactionServiceClient>(client => {
                client.Address = Configuration.GetValue<Uri>("TransactionServiceUri");
            })
            .ConfigurePrimaryHttpMessageHandler(() => {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                return handler;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<ShareBrokerServiceManager>();
            });
        }
    }
}
