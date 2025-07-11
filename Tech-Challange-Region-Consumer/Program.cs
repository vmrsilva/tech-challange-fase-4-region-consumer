//using Tech_Challange_Region_Consumer;

//var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddHostedService<InsertRegionWorker>();

//var host = builder.Build();
//host.Run();


using MassTransit;
using Tech_Challange_Region_Consumer;
using TechChallange.Infrastructure.Context;
using TechChallange.Region.Domain.Region.Messaging;
using Microsoft.EntityFrameworkCore;
using TechChallange.Infrastructure.Repository.Region;
using TechChallange.Region.Domain.Region.Repository;
using TechChallange.Region.Domain.Region.Service;
using TechChallange.Infrastructure.Repository.Base;
using TechChallange.Region.Domain.Base.Repository;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        var fila = configuration.GetSection("MassTransit")["QueueCreateRegion"] ?? string.Empty;
        var servidor = configuration.GetSection("MassTransit")["Server"] ?? string.Empty;
        var usuario = configuration.GetSection("MassTransit")["User"] ?? string.Empty;
        var senha = configuration.GetSection("MassTransit")["Password"] ?? string.Empty;

        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        services.AddScoped<IRegionRepository, RegionRepository>();
        services.AddScoped<IRegionService, RegionService>();
        services.AddDbContext<TechChallangeContext>(options => options.UseSqlServer(configuration.GetConnectionString("Database")));

        services.AddMassTransit(x =>
        {
            x.AddConsumer<InsertRegionConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(servidor, "/", h =>
                {
                    h.Username(usuario);
                    h.Password(senha);
                });
            

                cfg.Message<RegionCreateMessageDto>(m =>
                {
                    m.SetEntityName("region-insert-exchange");
                });

                cfg.ReceiveEndpoint(fila, e =>
                {
                    e.ConfigureConsumer<InsertRegionConsumer>(context);
                });
            });

        });
    })
    .Build();



host.Run();
