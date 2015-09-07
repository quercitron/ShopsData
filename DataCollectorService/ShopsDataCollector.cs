using System.Globalization;
using System.ServiceProcess;

using DataCollectorFramework;

using Quartz;
using Quartz.Impl;

namespace DataCollectorService
{
    public partial class ShopsDataCollector : ServiceBase
    {
        private readonly IScheduler _scheduler;

        public ShopsDataCollector()
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
            ITrigger trigger = TriggerBuilder.Create()
              .WithIdentity("DataCollectionTrigger", "ShopsDataCollection")
              .WithCronSchedule("0 0/30 * * * ?")
              .Build();

            _scheduler.ScheduleJob(job, trigger);
        }

        protected override void OnStart(string[] args)
        {
            _scheduler.Start();
        }

        protected override void OnStop()
        {
            _scheduler.Shutdown();
        }
    }

    public class DataCollectionJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var collector = new GeneralDataCollector();
            collector.ProcessData();
        }
    }
}
