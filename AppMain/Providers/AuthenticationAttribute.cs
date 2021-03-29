
namespace AppMain.Providers
{
	using System.Configuration;
	using System.Net;
	using System.Net.Http;
	using System.Web.Http.Filters;

	public class AppAuthentication : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                // Gets header parameters  
                string authenticationString = actionContext.Request.Headers.Authorization.Parameter;
                if (!this.ValidateHash(authenticationString))
                {
                    // returns unauthorized error  
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }

            base.OnAuthorization(actionContext);
        }

        public bool ValidateHash(string hashKey)
        {
			string authCode = ConfigurationManager.AppSettings["AuthCode"];
	        if (authCode==hashKey)
	        {
				return true;
	        }
			return false;
	       
        }
    }

}