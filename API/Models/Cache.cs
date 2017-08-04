using System;
using System.IO;

namespace API.Models
{
  public static class Cache
  {
		private const string baseDir = @"C:\inetpub\wwwroot";

		public static GemStone.GemFire.Cache.Generic.Cache Initalize(){
			if(!File.Exists(baseDir + @"\cache.xml")){
				throw new FileNotFoundException();
			}

			string pathToGemfireLib = @"C:\humana-gemfire-lib";
			string currentPath = Environment.GetEnvironmentVariable("Path");

			//C:\pivotal-gemfire-nativeclient-64bit-9.0.6-build.17\bin

			//remove any existing path settings, so we are sure to use the above location
			currentPath = currentPath.Replace(@"C:\pivotal-gemfire-nativeclient-64bit-9.0.6-build.17\bin", "")
																.Replace(";;",";");

			string newPath = pathToGemfireLib + ";" + currentPath.Trim();//here trimming need to bedone
			Environment.SetEnvironmentVariable("Path", newPath);

			GemStone.GemFire.Cache.Generic.Properties<string,string> cacheProps = new GemStone.GemFire.Cache.Generic.Properties<string,string>();
			cacheProps.Insert("cache-xml-file", baseDir + @"\cache.xml");
			cacheProps.Insert("log-level", "fine");
			cacheProps.Insert("log-file", @"c:\Logs\client.log");
			cacheProps.Insert("ssl-enabled", "true");
			cacheProps.Insert("ssl-truststore", baseDir + @"\mutual-ssl\ca.cert.pem");
			cacheProps.Insert("ssl-keystore", baseDir + @"\mutual-ssl\client.pem");
			cacheProps.Insert("ssl-keystore-password", "secretpassword");
			cacheProps.Insert("statistic-sampling-enabled", "false");
			cacheProps.Insert("connect-timeout", "10");

			GemStone.GemFire.Cache.Generic.CacheFactory cacheFactory = GemStone.GemFire.Cache.Generic.CacheFactory.CreateCacheFactory(cacheProps);

			GemStone.GemFire.Cache.Generic.Cache cache = cacheFactory.Create();

			return cache;
    }
	}
}
