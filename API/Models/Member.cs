using System;

namespace API.Models
{
  public class Member
  {
    public string fullName { get; set; }
		public string email { get; set; }

		public static Member Add(GemStone.GemFire.Cache.Generic.IRegion<string, string> region , Member member , bool exceptionOnExists = true){
      if(region == null || member == null){
        throw new ArgumentNullException();
      }

			try {
				string tmp;
				/*if (region.TryGetValue(member.email, out tmp)) {
					if (exceptionOnExists) {
						throw new MemberAccessException();
					}

					return Get(region, member.email);
				}*/

				region[member.email] = member.fullName;
			} catch (Exception ex) {
				throw new AddMemberException("Error add member", ex);
			}

      return Get(region, member.email);
    }
    public static Member Get(GemStone.GemFire.Cache.Generic.IRegion<string, string> region, string email){
      if(region == null || string.IsNullOrEmpty(email)){
        throw new ArgumentNullException();
      }

			string name = null;
			
			try {
				if (!region.TryGetValue(email, out name))
					throw new NullReferenceException("Key does not exist");
			} catch (Exception ex) {
				throw new GetMemberException("Error getting member", ex);
			}

			if(string.IsNullOrEmpty(name))
				throw new NullReferenceException("Key does not exist");

			Member m = new Member() {
				fullName = name,
				email = email
			};

      return m;
    }
    /*public static void Remove(GemStone.GemFire.Cache.Generic.IRegion<string, string> region, string email){
      if(region == null || string.IsNullOrEmpty(email)){
        throw new ArgumentNullException();
      }

			try {
				string tmp;
				if (!region.TryGetValue(email, out tmp)) {
					throw new NullReferenceException();
				}

				region.Remove(email);
			} catch (Exception ex) {
				throw new RemoveMemberException("Error getting member", ex);
			}

      return;
    }
    public static void Update(GemStone.GemFire.Cache.Generic.IRegion<string, string> region, string email, Member member){
      if(region == null || string.IsNullOrEmpty(email) || member == null){
        throw new ArgumentNullException();
      }

			try {
				string tmp;
				if (!region.TryGetValue(email, out tmp)) {
					throw new NullReferenceException();
				}

				region[email] = member.fullName;
			} catch (Exception ex) {
				throw new RemoveMemberException("Error getting member", ex);
			}

      return;
    }*/

		}
}
