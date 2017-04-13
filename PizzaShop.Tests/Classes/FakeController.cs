using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PizzaShop.Tests.Classes
{
    public class FakeController
    {
        HttpContextBase _httpContext;
        HttpRequestBase _request;
        //HttpResponseBase _response;

        public FakeController()
        {
            _httpContext = Substitute.For<HttpContextBase>();
            _request = Substitute.For<HttpRequestBase>();
            //_response = Substitute.For<HttpResponseBase>();
        }

        public void PrepareFakeAjaxRequest()
        {
            _request.Headers.Returns(new WebHeaderCollection() { { "X-Requested-With", "XMLHttpRequest" } });
            _httpContext.Request.Returns(_request);
        }

        public void PrepareFakeRequest()
        {
            _httpContext.Request.Returns(_request);
        }

        public ControllerContext GetControllerContext<T>(RouteData route, T controller) where T: Controller
        {
            var context = new ControllerContext(_httpContext, route, controller);
            return context;
        }
    }
}
