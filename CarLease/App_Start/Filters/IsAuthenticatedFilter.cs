using CarLease.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarLease.App_Start.Filters
{
    public class IsAuthenticatedAttribute : FilterAttribute, IAuthorizationFilter
    {
        public bool IsAdmin { get; set; }
        public IsAuthenticatedAttribute(bool isAdmin = false)
        {
            IsAdmin = isAdmin;
        }

        void IAuthorizationFilter.OnAuthorization(AuthorizationContext filterContext)
        {
            string returnQS = "?returnUrl=" + HttpUtility.UrlEncode(filterContext.HttpContext.Request.RawUrl);

            if (filterContext.HttpContext.Session["user"] == null)
                filterContext.Result = new RedirectResult("/auth/login" + returnQS);
            else
            {
                Customers curUser = (Customers) filterContext.HttpContext.Session["user"];
                if (this.IsAdmin && !curUser.IsAdmin)
                {
                    filterContext.Result = new RedirectResult("/auth/login" + returnQS);
                }
                else
                {
                    filterContext.Controller.ViewBag.currentUsername = curUser.UserName;
                }
            }

        }
    }
}