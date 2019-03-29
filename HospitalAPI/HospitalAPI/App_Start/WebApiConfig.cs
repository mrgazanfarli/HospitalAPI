using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace HospitalAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "OneBlog",
                routeTemplate: "api/blogs/{slug}",
                defaults: new { slug = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "PostBlog",
                routeTemplate: "api/blogs"
                );
            config.Routes.MapHttpRoute(
                name: "GetBlogList",
                routeTemplate: "api/bloglist"
                );
            config.Routes.MapHttpRoute(
                name: "GetDepartmentNames",
                routeTemplate: "api/departmentnames"
                );
            config.Routes.MapHttpRoute(
                name: "GetDepartmentDetails",
                routeTemplate: "api/department/{*slug}",
                defaults: new { controller = "Departments", action = "GetDepartment", slug = RouteParameter.Optional }
                );

            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
