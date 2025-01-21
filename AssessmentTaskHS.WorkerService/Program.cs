using AssessmentTaskHS.Repository;
using AssessmentTaskHS.Repository.Interfaces;
using AssessmentTaskHS.Services;
using AssessmentTaskHS.WorkerService.Jobs;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));

        services.AddHangfire(config =>
            config.UseSqlServerStorage(context.Configuration.GetConnectionString("DefaultConnection")));
        services.AddHangfireServer();

        services.AddHttpClient<AlphaVantageService>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddScoped<StockUpsertJob>();
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    recurringJobManager.AddOrUpdate<StockUpsertJob>(
        "StockUpsertJob",
        job => job.ExecuteAsync(),
        Cron.Hourly // Executar a cada hora
    );

    var backgroundJobClient = scope.ServiceProvider.GetRequiredService<IBackgroundJobClient>();
    backgroundJobClient.Enqueue<StockUpsertJob>(job => job.ExecuteAsync());
}

app.Run();
