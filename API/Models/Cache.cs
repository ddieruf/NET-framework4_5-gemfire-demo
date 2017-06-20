using System;

namespace API.Models
{
  public static class Cache
  {
    public static GemStone.GemFire.Cache.Generic.Cache Initalize(LocatorAddress[] locators){
      if (locators == null || locators.Length < 1){
        throw new ArgumentNullException();
      }

      string sLocators = ParseLocators(locators);

      GemStone.GemFire.Cache.Generic.Cache cache = null;
      GemStone.GemFire.Cache.Generic.CacheFactory cacheFactory = null;

			GemStone.GemFire.Cache.Generic.Properties<string,string> cacheProps = new GemStone.GemFire.Cache.Generic.Properties<string,string>();
			//prop.Insert("cache-xml-file", "cache.xml");
			//GemFire internal logging ('severe', 'error', 'warning', 'info', 'config', or 'fine') #
			cacheProps.Insert("log-level", "config");
			//('<addr1>[<port1>],<addr2>[<port2>]')
			cacheProps.Insert("locators", sLocators);
			cacheProps.Insert("mcast-port", "0");
      //sysProps.Insert("appdomain-enabled", "true");

      cacheFactory = GemStone.GemFire.Cache.Generic.CacheFactory.CreateCacheFactory(cacheProps);
      cache = cacheFactory.Create();

      return cache;
    }
    private static string ParseLocators(LocatorAddress[] locators){
      string ret = "";

      foreach(LocatorAddress locator in locators){
        ret += locator.serverAddress + "["+locator.serverPort.ToString()+"],";
      }

      ret = ret.Substring(0, ret.Length - 1);

      return ret;
    }
  }
}
