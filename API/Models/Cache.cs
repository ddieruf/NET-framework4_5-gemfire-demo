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

			GemStone.GemFire.Cache.Generic.Properties<string,string> cacheProps = new GemStone.GemFire.Cache.Generic.Properties<string,string>();
			cacheProps.Insert("cache-xml-file", baseDir + @"\cache.xml");
			cacheProps.Insert("log-level", "fine");
			cacheProps.Insert("log-file", @"c:\Logs\client.log");

			GemStone.GemFire.Cache.Generic.CacheFactory cacheFactory = GemStone.GemFire.Cache.Generic.CacheFactory.CreateCacheFactory(cacheProps);

			GemStone.GemFire.Cache.Generic.Cache cache = cacheFactory.Create();

			return cache;
    }
	}
}
