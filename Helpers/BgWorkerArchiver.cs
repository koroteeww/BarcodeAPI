using BarcodeAPI.DB;
using BarcodeAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BarcodeAPI.Helpers
{
    public class BgWorkerArchiver : BackgroundService
    {
        private ILogger<BgWorkerArchiver> _logger;
        private int repeatDelayMillisec;
        private int _delayMin;
        private DateTime _lastUsage;
        /// <summary>
        /// scope factory instead of db context
        /// </summary>
        private readonly IServiceScopeFactory scopeFactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            bool stop = false;


            while (!stoppingToken.IsCancellationRequested && !stop)
            {
                //RunWorkerArchiveInCycle();
                try
                {
                    await Task.Delay(repeatDelayMillisec, stoppingToken);
                    _lastUsage = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    //_logger.LogCritical("Исключение TASK {exception} {stacktrace} {stoppingToken}", ex, ex.StackTrace, stoppingToken);
                }
            }
        }
        public void RunWorkerArchiveInCycle()
        {
            //archive
            //Ненужные записи должны переносится из основной таблицы в таблицу архивных записей.Способы:
            //    Принудительно после окончания действия идентификатора, для которого генерировался ШК(на совести разработчика)
            //    Автоматически после истечения 90 дней существования ШК.
            //    Входные данные:
            //    Действия: перенос строки, удовлетворяющую условию, из db_BarCode в db_BarCodeHistory
            //    Выходные данные:
            DateTime dtNow = DateTime.UtcNow;
            List<BarcodeHistory> toArchive = new List<BarcodeHistory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BarcodeDbContext>();
                //sql
                //bool created = dbContext.Database.EnsureCreated();
                //dbContext.Database.Migrate();

                var allBarcodes = dbContext.Barcode.ToList();
                foreach (var item in allBarcodes)
                {
                    //check dt
                    if ( (dtNow - item.DateTimeBarCodeRequest).TotalDays > 90)
                    {
                        //archive
                        BarcodeHistory hist = new BarcodeHistory(item);
                        toArchive.Add(hist);
                    }
                }
                //process archive
                foreach (var archItem in toArchive)
                {
                    dbContext.BarcodeHistory.Add(archItem);
                    dbContext.Barcode.Remove(archItem.barcodeItem);
                }

                if (toArchive.Count > 0)
                {
                    dbContext.SaveChanges();
                }
            }   
            
        }
        public BgWorkerArchiver(ILogger<BgWorkerArchiver> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _delayMin = 1440;//24h
            repeatDelayMillisec = (_delayMin) * 60 * 1000;
            _lastUsage = DateTime.UtcNow;
            scopeFactory = serviceScopeFactory;
            //_dbcontext = context;
        }
    }
}
