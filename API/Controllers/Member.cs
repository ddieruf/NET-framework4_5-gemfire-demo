using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;

namespace API.Controllers
{
  public class Member: IMemberService
  {
    public API.Models.Member Get(string email)
    {
      Models.Member m = null;

      try{
        m = Models.Member.Get(Global.region,email);
      }catch (Models.GetMemberException ge){
        ThrowJson("Error getting member, " + ge.InnerException.Message, ge.InnerException.StackTrace);
      }catch (ArgumentNullException ar){
        ThrowJson("Missing argument", ar.StackTrace);
      }catch (NullReferenceException){
        ThrowJson("Member email doesn't exist");
      }catch (Exception ex){
				ThrowJson(ex);
      }

      return m;
    }
    public void Add(API.Models.Member member)
    {
      try{
        Models.Member.Add(Global.region,member,true);
      }catch (Models.AddMemberException ge){
        ThrowJson("Error adding member, " + ge.InnerException.Message, ge.InnerException.StackTrace);
      }catch (ArgumentNullException ar){
        ThrowJson("Missing argument", ar.StackTrace);
      }catch (MemberAccessException){
        ThrowJson("Member email already exists");
      }catch (Exception ex){
        ThrowJson(ex);
      }

      return;
    }
    public void Remove(string email)
    {
      try{
        Models.Member.Remove(Global.region,email);
      }catch (Models.RemoveMemberException ge){
        ThrowJson("Error removing member, " + ge.InnerException.Message, ge.InnerException.StackTrace);
      }catch (ArgumentNullException ar){
        ThrowJson("Missing argument", ar.StackTrace);
      }catch (NullReferenceException){
        ThrowJson("Member email doesn't exist");
      }catch (Exception ex){
        ThrowJson(ex);
      }

      return;
    }
    public void Update(string email, Models.Member member)
    {
      try{
        Models.Member.Update(Global.region, email, member);
      }catch (Models.UpdateMemberException ge){
        ThrowJson("Error updating member, " + ge.InnerException.Message, ge.InnerException.StackTrace);
      }catch (ArgumentNullException ar){
        ThrowJson("Missing argument", ar.StackTrace);
      }catch (NullReferenceException){
        ThrowJson("Member email doesn't exist");
      }catch (Exception ex){
        ThrowJson(ex);
      }

      return;
    }
    private void ThrowJson(Exception ex){
      ThrowJson(ex.Message, ex.StackTrace);
    }
    private void ThrowJson(Exception ex, string staceTrace){
			ThrowJson(ex.Message, staceTrace);
    }
    private void ThrowJson(string message, string staceTrace = null){
      HttpContext.Current.Response.Write("{\"error\":{\"message\":\""+ message +"\",\"trace\":\"\"+ staceTrace +\"\"}");
      WebOperationContext.Current.OutgoingResponse.SuppressEntityBody = false;
    }
  }

  [ServiceContract]
  public interface IMemberService
  {
    [OperationContract]
    [WebInvoke(Method = "GET", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
    Models.Member Get(string email);

    [OperationContract]
    [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
    void Add(Models.Member member);

		[OperationContract]
		[WebInvoke(Method = "DELETE", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
		void Remove(string email);
  }
}
