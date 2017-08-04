using System;
namespace API.Models
{
  public static class Region
  {
    public static GemStone.GemFire.Cache.Generic.IRegion<string, string> Initalize(string name, GemStone.GemFire.Cache.Generic.Cache cache){
      if(string.IsNullOrEmpty(name) || cache == null){
        throw new ArgumentNullException();
      }
			
			GemStone.GemFire.Cache.Generic.RegionFactory regionFactory = cache.CreateRegionFactory(GemStone.GemFire.Cache.Generic.RegionShortcut.PROXY);
			GemStone.GemFire.Cache.Generic.IRegion<string, string> region = regionFactory.Create<string, string>(name);

			return region;
    }
  }
}
