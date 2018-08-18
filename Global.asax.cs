using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Isracard
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Get the error details
            HttpException lastErrorWrapper =
                Server.GetLastError() as HttpException;
            Models.BookmarkRequestStatus status = new Models.BookmarkRequestStatus();
            status.Status = false;
            status.StatusType = Models.BookmarkRequestStatus.BookmarkStatusType.RouteException;
            switch (lastErrorWrapper.GetHttpCode())
            {
                case 404:
                    status.Text = "No page found";
                    break;
                default:
                    status.Text = lastErrorWrapper.GetHtmlErrorMessage();
                    break;
            }
            Response.Write(status.toJson());
            Server.ClearError();
        }
    }
}
