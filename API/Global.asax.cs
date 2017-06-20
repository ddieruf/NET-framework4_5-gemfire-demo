using System;
using System.Web;
using System.Web.Routing;
using System.ServiceModel.Activation;
using System.Configuration;

namespace API
{
	public class Global : System.Web.HttpApplication
	{
    public static GemStone.GemFire.Cache.Generic.Cache cache = null;
    public static GemStone.GemFire.Cache.Generic.Region<string,Models.Member> region = null;

		protected void Application_Start(object sender, EventArgs e)
		{
			RegisterRoutes();

      string locatorAddress = ConfigurationManager.AppSettings["locatorAddress"];
      string regionName = ConfigurationManager.AppSettings["regionName"];
      int locatorPort = 0;

      if(string.IsNullOrEmpty(locatorAddress))
        throw new MissingFieldException("locatorAddress");

      if(string.IsNullOrEmpty(regionName))
        throw new MissingFieldException("regionName");

			if (!int.TryParse(ConfigurationManager.AppSettings["locatorPort"], out locatorPort))
				throw new MissingFieldException("locatorPort");
      
      //start client cache
      try{
	      cache = Models.Cache.Initalize(new Models.LocatorAddress[]{
	        new Models.LocatorAddress(){
	           serverAddress = locatorAddress,
	           serverPort = locatorPort
	        }
        });
      }catch(Exception ex){
        throw new Exception("Error initializing cache, " + ex.Message);
      }

      //initialize reference to region
      try{
        region = (GemStone.GemFire.Cache.Generic.Region<string, Models.Member>)Models.Region.Initalize(regionName, cache);
			}catch(GemStone.GemFire.Cache.Generic.RegionExistsException re){
        throw new Exception("Region already exists, " + re.Message);
      }catch(GemStone.GemFire.Cache.Generic.CacheClosedException cc){
        throw new Exception("Region error cache closed, " + cc.Message);
      }catch(GemStone.GemFire.Cache.Generic.OutOfMemoryException om){
        throw new Exception("Region error out of memory, " + om.Message);
      }catch(GemStone.GemFire.Cache.Generic.RegionCreationFailedException rc){
        throw new Exception("Region failed, " + rc.Message);
      }catch(GemStone.GemFire.Cache.Generic.InitFailedException fe){
        throw new Exception("Init region error, " + fe.Message);
      }catch(GemStone.GemFire.Cache.Generic.UnknownException ue){
        throw new Exception("Init region error, " + ue.Message);
      }catch(Exception ex){
        throw new Exception("General region error, " + ex.Message);
      }

      if (cache == null)
        throw new NullReferenceException("cache");

			if (region == null)
				throw new NullReferenceException("region");

      /*Models.Member.Add(region,new Models.Member(){
        email = "no@email.com",
        firstName = "first",
        lastName = "last",
        password = "123asd"
      });*/
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
			HttpApplication application = (HttpApplication)sender;
			HttpContext httpContext = application.Context;
			
			if (httpContext.Request.HttpMethod.Equals("OPTIONS")){
				httpContext.Response.AppendHeader("Access-Control-Allow-Origin", httpContext.Request.Headers["Origin"]);
				httpContext.Response.AppendHeader("Access-Control-Allow-Methods", httpContext.Request.Headers["Access-Control-Request-Method"]);
				httpContext.Response.AppendHeader("Access-Control-Allow-Headers", "content-type,accept,streampay,origin,XSRF-TOKEN,StreamPayApi,StreamPayLogin");
				httpContext.Response.End();
				return;
			}
			
			//The reason for this is because IIS will do a 302 redirect if the endpoint address does not have a querystring and does not have a trailing slash. This is a problem because behind the load balancer
			//IIS is not set up for HTTPS (port 443) so the redirect is going to HTTP (port 80), which is not open on the load balancer, thus you get an error saying "Endpoint does not exist"
			/*if ( string.Compare(HttpContext.Current.Request.Url.PathAndQuery, HttpContext.Current.Request.Url.AbsolutePath, true) == 0 ) {//there's nothing in the querystring
				string lastChar = HttpContext.Current.Request.Url.AbsolutePath.Substring(HttpContext.Current.Request.Url.AbsolutePath.Length-1);
				int tmp;

				//if the endpoint address does not have a querystring value and it does not end in digit(s) then make sure it is formatted correctly
				if ( !lastChar.Equals("/") && !int.TryParse(lastChar, out tmp))
					HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.AbsolutePath + "/");
			}*/
	
			//Reset headers
			httpContext.Response.ClearHeaders();
			httpContext.Response.AppendHeader("Access-Control-Allow-Origin", httpContext.Request.Headers["Origin"]);
			httpContext.Response.AppendHeader("Access-Control-Expose-Headers", "content-type,StreamPayApi");

      Console.WriteLine("Request authenticated");
		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{

		}

		protected void Application_Error(object sender, EventArgs e)
		{
			if (cache != null && !cache.IsClosed)
				cache.Close();
		}

		protected void Session_End(object sender, EventArgs e)
		{

		}

		protected void Application_End(object sender, EventArgs e)
		{
      if(cache != null && !cache.IsClosed)
        cache.Close();
		}
		protected void Application_EndRequest(object sender, EventArgs e)
		{
		}
	}
}