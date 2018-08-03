using Library.Service.Common;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Web.Mvc.Filters
{
    /// <summary>
    /// Only allow authenticated users to access this page.
    /// Optionally check if the user has a specifix role
    /// </summary>
    public class Filter : System.Web.Mvc.AuthorizeAttribute, IAuthorizationFilter
    {
        IPersonService _personService;
        public Filter()
        {
            _personService = DependencyResolver.Current.GetService<IPersonService>();
        }

        /// <summary>
        /// The default login url
        /// </summary>
        private const string DEFAULT_LOGIN_URL = "/Account/Login";

        /// <summary>
        /// The login url to use (instead of the default login url)
        /// </summary>
        public String LoginUrl { get; set; }

        #region Methods
        /// <summary>
        /// Perform a access control check on the currently logged in user
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            //Check if anonymous access is allowed on this action
            if (filterContext.ActionDescriptor.GetCustomAttributes(true).Any(a => a is AllowAnonymousAttribute)) return;

            //Get the request object
            var request = filterContext.HttpContext.Request;

            //Get the details about this request
            var url = request.Url;
            var path = url.PathAndQuery;	//e.g. /backoffice/products/edit?id=1

            //Generate the login url
            var loginUrl = String.IsNullOrEmpty(this.LoginUrl) ? DEFAULT_LOGIN_URL : this.LoginUrl;

            //Add the redirect to the login url
            loginUrl += "?redirectUrl=" + HttpUtility.UrlEncode(path);

            //Get the person
            String sessionId = null;
            if (!this.CheckAuthCookie(out sessionId)) //User must log in
            {
                filterContext.Result = new RedirectResult(loginUrl);
                return;
            }

            //Get the person
            Guid guid = Guid.Parse(sessionId);
            //TODO
            //var person =  this.PersonService.GetByGuid(guid);

            //Check for roles
            if (!String.IsNullOrWhiteSpace(this.Roles))
            {
                var roles = this.Roles.Split(',');
                //TODO: check roles
                //if (!SecurityService.IsInRoles(person.PersonId, roles))
                //{
                //    //User doesn't have required role
                //    filterContext.Result = new RedirectResult("/");
                //    return;
                //}

            }
        }

        public bool CheckAuthCookie(out String sessionId)
        {
            //By default the sessionId is null
            sessionId = null;

            //Ignore if there is no http context
            if (HttpContext.Current == null) return false;

            //Get the http context request and response streams
            var request = HttpContext.Current.Request;

            //Check if a user is logged in (or return the login view)
            HttpCookie cookie = request.Cookies["TrainingAuth"];
            if (cookie == null) return false;

            //Check if the user login is valid
            Guid guid = Guid.Empty;
            if (!Guid.TryParse(cookie.Value, out guid)) return false;

            //Find the user with the session id
            var person = _personService.GetByGuid(guid);
            if (person == null) return false;

            //OK
            sessionId = cookie.Value;
            return true;
        }

    }
    #endregion
}