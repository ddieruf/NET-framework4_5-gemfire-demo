using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;
using System.ServiceModel.Activation;

namespace API.Controllers
{
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	public class Member: IMemberService
  {
    public API.Models.Member Get(string email)
    {
      Models.Member m = null;

			try {
        m = Models.Member.Get(Global.region,email);
      }catch (Models.GetMemberException ge){
        ThrowJson("Error getting member, " + ge.InnerException.Message, ge.InnerException.StackTrace);
      }catch (ArgumentNullException ar){
        ThrowJson("Missing argument", ar.StackTrace);
      }catch (NullReferenceException){
        ThrowJson("Member email \""+ email + "\" doesn't exist");
      }catch (Exception ex){
				ThrowJson(ex);
      }

      return m;
    }
    public Models.Member Add(Models.Member member)
    {
			Models.Member m = null;
			try {
				m = Models.Member.Add(Global.region, member, true);
			} catch (Models.AddMemberException ge) {
				ThrowJson("Error adding member, " + ge.InnerException.Message, ge.InnerException.StackTrace);
			} catch (ArgumentNullException ar) {
				ThrowJson("Missing argument", ar.StackTrace);
			} catch (MemberAccessException) {
				ThrowJson("Member email \"" + member.email + "\" already exist");
			} catch (Exception ex) {
				ThrowJson(ex);
			}

			return m;
    }
		
    private void ThrowJson(Exception ex){
      ThrowJson(ex.Message, ex.StackTrace);
    }
    private void ThrowJson(Exception ex, string staceTrace){
			ThrowJson(ex.Message, staceTrace);
    }
    private void ThrowJson(string message, string staceTrace = null){
			WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
      HttpContext.Current.Response.Write("{\"error\":{\"message\":\""+ message +"\",\"trace\":\""+ staceTrace +"\"}}");
      //WebOperationContext.Current.OutgoingResponse.SuppressEntityBody = false;
    }
  }
	
  [ServiceContract]
	public interface IMemberService
  {
    [OperationContract]
    [WebInvoke(Method = "GET", UriTemplate = "?email={email}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
    Models.Member Get(string email);

    [OperationContract]
    [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
    Models.Member Add(Models.Member member);
	}
}
