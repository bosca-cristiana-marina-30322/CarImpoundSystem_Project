using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CarImpoundSystem.Data;
using CarImpoundSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CarImpoundSystem.Services
{
    public class DailyPaymentUpdateService : BackgroundService
    {
        private readonly ILogger<DailyPaymentUpdateService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public DailyPaymentUpdateService(ILogger<DailyPaymentUpdateService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Daily payment update service is running.");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
                    await UpdatePaymentsAsync(dbContext);
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // Wait for 24 hours before running again
            }
        }

        private async Task UpdatePaymentsAsync(AppDBContext dbContext)
        {
            var impoundRecords = await dbContext.impoundmentRecords.ToListAsync();

            foreach (var record in impoundRecords)
            {
                if (record.status == "in")
                {
                    var daysInDeposit = (DateTime.Now - record.date).Days;
                    double paymentIncrease = daysInDeposit * 10; // Increase by 10 per day
                    record.payment = 300 + paymentIncrease;
                }
                else if (record.status == "out")
                {
                    record.payment = 0;
                }
            }

            await dbContext.SaveChangesAsync();
            _logger.LogInformation("Payment amounts updated for impoundment records.");
        }
    }
}
