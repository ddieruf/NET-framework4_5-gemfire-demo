using System;
namespace API.Models
{
  public static class Region
  {
    public static GemStone.GemFire.Cache.Generic.IRegion<string,Member> Initalize(string name, GemStone.GemFire.Cache.Generic.Cache cache){
      if(string.IsNullOrEmpty(name) || cache == null){
        throw new ArgumentNullException();
      }

      GemStone.GemFire.Cache.Generic.RegionFactory regionFactory = null;
      GemStone.GemFire.Cache.Generic.IRegion<string, Member> region = null;

      regionFactory = cache.CreateRegionFactory(GemStone.GemFire.Cache.Generic.RegionShortcut.PROXY);
      region = regionFactory.Create<string, Member>(name);

      return region;
    }
  }
}
