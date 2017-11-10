using CourierHelper.BusinessLogic.Infrastructure;
using System.Configuration;
using System.ServiceProcess;
using System.Timers;

namespace CourierHelper.TaskManagerService
{
	public partial class Service : ServiceBase
	{
		private Timer _ordersQueueTimer;
		private Timer _ordersProcessingTimer;
		private OrderManager _orderManager;

		public Service()
		{
			InitializeComponent();
			string connectionString = ConfigurationManager.ConnectionStrings["CourierHelperDb"].ConnectionString;
			string mapBoxToken = ConfigurationManager.AppSettings["Mapbox:AccessToken"];
			_orderManager = new OrderManager(connectionString, mapBoxToken);
		}

		protected override void OnStart(string[] args)
		{
			_ordersQueueTimer = new Timer();
			_ordersQueueTimer.Interval = 8 * 60 * 1000;
			_ordersQueueTimer.Elapsed += UpdateData;
			_ordersQueueTimer.Enabled = true;

			_ordersProcessingTimer = new Timer();
			_ordersProcessingTimer.Interval = 4 * 60 * 1000;
			_ordersProcessingTimer.Elapsed += ProceedData;
			_ordersProcessingTimer.Enabled = true;
		}

		private void UpdateData(object sender, ElapsedEventArgs e)
		{
			_orderManager.UpdateQueue();
		}
		private void ProceedData(object sender, ElapsedEventArgs e)
		{
			_orderManager.ProceedNextOrderAsync().Wait();
		}

		protected override void OnStop()
		{
			_ordersQueueTimer.Enabled = false;
			_ordersProcessingTimer.Enabled = false;
		}
	}
}
