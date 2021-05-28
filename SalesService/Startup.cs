using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SalesService.DAL;
using SalesService.DAL.Context;
using static ShareBrokerServiceGrpc.Protos.IShareBrokerService;
using static OwnershipServiceGrpc.Protos.IOwnershipService;
using System;
using SalesService.SAL;

namespace SalesService
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddGrpc();

            // IoC
            services.AddTransient<ISaleRequestDataManger,SaleRequestDataManager>();
            // Share broker
            services.AddGrpcClient<IShareBrokerServiceClient>(client => {
                client.Address = Configuration.GetValue<Uri>("ShareBrokerServiceUri");
            })
            .ConfigurePrimaryHttpMessageHandler(() => {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                return handler;
            });
            // Ownership service
            services.AddGrpcClient<IOwnershipServiceClient>(client => {
                client.Address = Configuration.GetValue<Uri>("OwnershipServiceUri");
            })
            .ConfigurePrimaryHttpMessageHandler(() => {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                return handler;
            });
            services.AddSwaggerDocument();


            string connectionString = Configuration.GetValue<string>("DBConnection");
            services.AddDbContext<SalesServiceControlDbContext>(options => options.UseSqlServer(connectionString, builder => builder.CommandTimeout(300)));


            InitDatabase(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUi3();
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<SalesServiceManager>();
            });
        }
    }
}
