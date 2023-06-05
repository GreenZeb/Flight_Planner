using FlightPlanner.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using FlightPlanner.Core.Services;
using FlightPlanner.Services;
using FlightPlanner_ASPNET.Handlers;
using FlightPlanner.Services.Validators;
using FlightPlanner.Core.Models;
using FlightPlanner;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddAuthentication("BasicAuthentication")
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

        services.AddDbContext<FlightPlannerDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("FlightPlannerConnection")));

        services.AddScoped<IFlightPlannerDbContext>(provider => provider.GetService<FlightPlannerDbContext>());
        services.AddScoped<IDbService, DbService>();
        services.AddScoped<IEntityService<Flight>, EntityService<Flight>>();
        services.AddScoped<IEntityService<Airport>, EntityService<Airport>>();
        services.AddScoped<IFlightService, FlightService>();
        services.AddSingleton<IMapper>(AutoMapperConfig.CreateMapper());
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}