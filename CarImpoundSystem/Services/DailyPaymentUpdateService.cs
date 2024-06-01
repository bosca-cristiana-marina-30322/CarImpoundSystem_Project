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

                // Resolve AppDBContext directly from the service provider
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
                // Calculate the number of days the car has been sitting in the deposit
                var daysInDeposit = (DateTime.Now - record.date).Days;

                // Calculate the increase in payment amount based on the number of days
                double paymentIncrease = daysInDeposit * 10; //  Increase by 10 per day

                // Update the payment amount for the record
                record.payment += paymentIncrease;
            }

            await dbContext.SaveChangesAsync();
            _logger.LogInformation("Payment amounts updated for impoundment records.");
        }
    }
}
