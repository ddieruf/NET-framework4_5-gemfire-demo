using System;
using System.Web;
using System.Web.Routing;
using System.ServiceModel.Activation;
using System.Configuration;

namespace API
{
	public class Global : System.Web.HttpApplication
	{
		private GemStone.GemFire.Cache.Generic.Cache cache;
    
		public static GemStone.GemFire.Cache.Generic.IRegion<string, string> region;
		
		protected void Application_Start(object sender, EventArgs e)
		{
			if (!Environment.Is64BitOperatingSystem){
				throw new Exception("Operating system must be 64bit.");
			}

			RegisterRoutes();
		}

		private void RegisterRoutes()
		{
      WebServiceHostFactory factory = new WebServiceHostFactory();
			RouteTable.Routes.Add(new ServiceRoute("members", factory, typeof(Controllers.Member)));
		}

		protected void Session_Start(object sender, EventArgs e)
		{
			
		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{
			cache = Models.Cache.Initalize();

			if (cache == null)
				throw new ArgumentNullException("cache");

			region = Models.Region.Initalize("memberRegion", cache);

			if (region == null)
				throw new ArgumentNullException("region");
		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{

		}

		protected void Application_Error(object sender, EventArgs e)
		{
		}

		protected void Session_End(object sender, EventArgs e)
		{
			
		}

		protected void Application_End(object sender, EventArgs e)
		{
			
		}
		protected void Application_EndRequest(object sender, EventArgs e)
		{
			cache.Close();
			cache.Dispose();
		}
	}
}