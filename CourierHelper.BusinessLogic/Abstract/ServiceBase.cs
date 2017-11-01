using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHelper.BusinessLogic.Abstract
{
	public abstract class ServiceBase
	{
		static ServiceBase()
		{
			AutomapperConfigurator.Configure();
		}
	}
}
