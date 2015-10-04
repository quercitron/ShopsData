using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.ServiceProcess;

using DataCollectorFramework;
using DataCollectorFramework.Logger;
using Quartz;
using Quartz.Impl;

namespace DataCollectorService
{
    public partial class ShopsDataCollectorService : ServiceBase
    {
        private readonly Logger _logger = new Logger(typeof(ShopsDataCollectorService));

        private readonly IScheduler _scheduler;

        public ShopsDataCollectorService()
        {
            log4net.Config.XmlConfigurator.Configure();
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

            InitializeComponent();

            // construct a scheduler factory
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            // get a scheduler
            _scheduler = schedFact.GetScheduler();

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<DataCollectionJob>()
                .WithIdentity("DataCollectionJob", "ShopsDataCollection")
                .Build();

            // Trigger the job to run now, and then every 40 seconds
            var schedule = ConfigurationManager.AppSettings["schedule"];

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("DataCollectionTrigger", "ShopsDataCollection")
                .WithCronSchedule(
                    schedule,
                    x => x.WithMisfireHandlingInstructionDoNothing())
                .Build();

            _scheduler.ScheduleJob(job, trigger);
        }

        protected override void OnStart(string[] args)
        {
            _scheduler.Start();
            _logger.Info("Start Data Collector service.");
        }

        protected override void OnStop()
        {
            _scheduler.Shutdown();
            _logger.Info("Stop Data Collector service.");
        }
    }

    public class DataCollectionJob : IJob
    {
        private readonly Logger _logger = new Logger(typeof (DataCollectionJob));

        public void Execute(IJobExecutionContext context)
        {
            _logger.Info("Data collecting started.");
            var stopwatch = Stopwatch.StartNew();
            var collector = new GeneralDataCollector();
            collector.CollectData();
            stopwatch.Stop();
            _logger.InfoFormat("Data collecting complete in {0} ms.", stopwatch.ElapsedMilliseconds);
        }
    }
}
