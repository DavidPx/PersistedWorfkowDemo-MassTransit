using MassTransit;
using MassTransit.Definition;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using persistedworkflowdemo_masstransit.Consumers;
using persistedworkflowdemo_masstransit.StateMachine;
using persistedworkflowdemo_masstransit.StateMachine.Activities;

namespace persistedworkflowdemo_masstransit
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "persistedworkflowdemo_masstransit", Version = "v1" });
            });

            services
                .AddTransient<ApprovalActivity>()
                .AddTransient<DenialActivity>();

            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
            services
                .AddMassTransit(cfg =>
                {
                    cfg.AddConsumer<CreateWorkItemConsumer>();
                    cfg.AddConsumer<ApproveWorkItemConsumer>();
                    cfg
                        .AddSagaStateMachine<WorkItemStateMachine, WorkItemState>()
                        .InMemoryRepository()
                        
                        .Endpoint(e =>
                        {
                            e.Name = $"queue:{KebabCaseEndpointNameFormatter.Instance.Consumer<CreateWorkItemConsumer>()}";
                        })
                        .Endpoint(e =>
                        {
                            e.Name = $"queue:{KebabCaseEndpointNameFormatter.Instance.Consumer<ApproveWorkItemConsumer>()}";
                        });
                    
                    cfg.UsingInMemory((context, cfg) =>
                    {
                        cfg.UseInMemoryScheduler();
                        cfg.ConfigureEndpoints(context);
                        cfg.UseInMemoryOutbox();
                    });

                });

            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "persistedworkflowdemo_masstransit v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
