using CourierHelper.BusinessLogic.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.BusinessLogic
{
	[TestClass]
	public class TestOrders
	{
		private const string connectionString = "Data Source=DESKTOP-FK4EC54;Initial Catalog=CourierHelperDbTest;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
		private OrderService _orderService;

		public TestOrders()
		{
			_orderService = new OrderService(connectionString);
		}

		[TestMethod]
		public void GetOrderById_CorrectId_ReturnOrder()
		{

		}
	}
}
