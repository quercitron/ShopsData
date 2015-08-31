using System;
using System.Globalization;
using System.ServiceProcess;
using System.Threading;
using DataCollectorFramework;

namespace DataCollectorService
{
    public partial class ShopsDataCollector : ServiceBase
    {
        private Timer _timer;
        private GeneralDataCollector _collector;

        public ShopsDataCollector()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

            _collector = new GeneralDataCollector();
            _timer = new Timer(CollectData, null, TimeSpan.Zero, TimeSpan.FromMinutes(30));
        }

        private void CollectData(object state)
        {
            _collector.ProcessData();
        }

        protected override void OnStop()
        {
            _timer.Dispose();
        }
    }
}
