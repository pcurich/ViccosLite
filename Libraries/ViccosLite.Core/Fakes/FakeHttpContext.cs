﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.SessionState;

namespace ViccosLite.Core.Fakes
{
    public class FakeHttpContext : HttpContextBase
    {
        private IPrincipal _principal;
        private HttpRequestBase _request;
        private HttpResponseBase _response;
        private readonly HttpCookieCollection _cookies;
        private readonly NameValueCollection _formParams;
        private readonly Dictionary<object, object> _items;
        private readonly string _method;
        private readonly NameValueCollection _queryStringParams;
        private readonly string _relativeUrl;
        private readonly NameValueCollection _serverVariables;
        private readonly SessionStateItemCollection _sessionItems;

        public FakeHttpContext(string relativeUrl, string method)
            : this(relativeUrl, method, null, null, null, null, null, null)
        {
        }

        public FakeHttpContext(string relativeUrl)
            : this(relativeUrl, null, null, null, null, null, null)
        {
        }

        public FakeHttpContext(string relativeUrl,
            IPrincipal principal, NameValueCollection formParams,
            NameValueCollection queryStringParams, HttpCookieCollection cookies,
            SessionStateItemCollection sessionItems, NameValueCollection serverVariables)
            : this(relativeUrl, null, principal, formParams, queryStringParams, cookies, sessionItems, serverVariables)
        {
        }

        public FakeHttpContext(string relativeUrl, string method,
            IPrincipal principal, NameValueCollection formParams,
            NameValueCollection queryStringParams, HttpCookieCollection cookies,
            SessionStateItemCollection sessionItems, NameValueCollection serverVariables)
        {
            _relativeUrl = relativeUrl;
            _method = method;
            _principal = principal;
            _formParams = formParams;
            _queryStringParams = queryStringParams;
            _cookies = cookies;
            _sessionItems = sessionItems;
            _serverVariables = serverVariables;

            _items = new Dictionary<object, object>();
        }

        public override HttpRequestBase Request
        {
            get
            {
                return _request ??
                       new FakeHttpRequest(_relativeUrl, _method, _formParams, _queryStringParams, _cookies,
                           _serverVariables);
            }
        }

        public override HttpResponseBase Response
        {
            get { return _response ?? new FakeHttpResponse(); }
        }

        public override IPrincipal User
        {
            get { return _principal; }
            set { _principal = value; }
        }

        public override HttpSessionStateBase Session
        {
            get { return new FakeHttpSessionState(_sessionItems); }
        }

        public override IDictionary Items
        {
            get { return _items; }
        }

        public override bool SkipAuthorization { get; set; }

        public static FakeHttpContext Root()
        {
            return new FakeHttpContext("~/");
        }

        public void SetRequest(HttpRequestBase request)
        {
            _request = request;
        }

        public void SetResponse(HttpResponseBase response)
        {
            _response = response;
        }

        public override object GetService(Type serviceType)
        {
            return null;
        }
    }
}