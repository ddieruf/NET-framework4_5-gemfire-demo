using System;
namespace API.Models
{
  public class Member
  {
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string email { get; set; }
    public string password { get; set; }

    public static void Add(GemStone.GemFire.Cache.Generic.IRegion<string, Member> region, Member member, bool exceptionOnExists = true){
      if(region == null || member == null){
        throw new ArgumentNullException();
      }

      if(region.ContainsKey(member.email)){
        if(exceptionOnExists){
          throw new MemberAccessException();
        }

        return;
      }

      try{
        region[member.email] = member;
      }catch(Exception ex){
        throw new AddMemberException("Error adding member",ex);
      }

      return;
    }
    public static Member Get(GemStone.GemFire.Cache.Generic.IRegion<string, Member> region, string email){
      if(region == null || string.IsNullOrEmpty(email)){
        throw new ArgumentNullException();
      }

      Member m = null;

      if(!region.ContainsKey(email)){
        throw new NullReferenceException();
      }

      try{
        m = region[email];
      }catch(Exception ex){
        throw new GetMemberException("Error getting member", ex);
      }

      return m;
    }
    public static void Remove(GemStone.GemFire.Cache.Generic.IRegion<string, Member> region, string email){
      if(region == null || string.IsNullOrEmpty(email)){
        throw new ArgumentNullException();
      }

      if(!region.ContainsKey(email)){
        throw new NullReferenceException();
      }

      try{
        region.Remove(email);
      }catch(Exception ex){
        throw new RemoveMemberException("Error removing member", ex);
      }

      return;
    }
    public static void Update(GemStone.GemFire.Cache.Generic.IRegion<string, Member> region, string email, Member member){
      if(region == null || string.IsNullOrEmpty(email) || member == null){
        throw new ArgumentNullException();
      }

			if (!region.ContainsKey(email)){
				throw new NullReferenceException();
			}

      try{
        region[email] = member;
      }catch(Exception ex){
        throw new UpdateMemberException("Error updating member", ex);
      }

      return;
    }
  }
}
