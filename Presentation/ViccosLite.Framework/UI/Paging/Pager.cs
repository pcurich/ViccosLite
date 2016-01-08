using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ViccosLite.Framework.UI.Paging
{
    public class Pager : IHtmlString
    {
        protected readonly IPageableModel Model;
        private readonly ViewContext _viewContext;
        protected string PageQueryName = "page";
        protected bool showTotalSummary;
        protected bool showPagerItems = true;
        protected bool showFirst = true;
        protected bool showPrevious = true;
        protected bool showNext = true;
        protected bool showLast = true;
        protected bool showIndividualPages = true;
        protected int individualPagesDisplayedCount = 5;
        protected Func<int, string> UrlBuilder;
        protected IList<string> BooleanParameterNames;

        public Pager(IPageableModel model, ViewContext context)
        {
            Model = model;
            _viewContext = context;
            UrlBuilder = CreateDefaultUrl;
            BooleanParameterNames = new List<string>();
        }

        protected ViewContext ViewContext
        {
            get { return _viewContext; }
        }

        public Pager QueryParam(string value)
        {
            PageQueryName = value;
            return this;
        }

        public Pager ShowTotalSummary(bool value)
        {
            showTotalSummary = value;
            return this;
        }

        public Pager ShowPagerItems(bool value)
        {
            showPagerItems = value;
            return this;
        }

        public Pager ShowFirst(bool value)
        {
            showFirst = value;
            return this;
        }

        public Pager ShowPrevious(bool value)
        {
            showPrevious = value;
            return this;
        }

        public Pager ShowNext(bool value)
        {
            showNext = value;
            return this;
        }

        public Pager ShowLast(bool value)
        {
            showLast = value;
            return this;
        }

        public Pager ShowIndividualPages(bool value)
        {
            showIndividualPages = value;
            return this;
        }

        public Pager IndividualPagesDisplayedCount(int value)
        {
            individualPagesDisplayedCount = value;
            return this;
        }

        public Pager Link(Func<int, string> value)
        {
            UrlBuilder = value;
            return this;
        }

        //little hack here due to ugly MVC implementation
        //find more info here: http://www.mindstorminteractive.com/topics/jquery-fix-asp-net-mvc-checkbox-truefalse-value/
        public Pager BooleanParameterName(string paramName)
        {
            BooleanParameterNames.Add(paramName);
            return this;
        }

        public override string ToString()
        {
            return ToHtmlString();
        }

        public virtual string ToHtmlString()
        {
            if (Model.TotalItems == 0)
                return null;

            var links = new StringBuilder();
            if (showTotalSummary && (Model.TotalPages > 0))
            {
                links.Append("<li class=\"total-summary\">");
                links.Append(string.Format("Página {0} de {1} (Total: {2})", Model.PageIndex + 1, Model.TotalPages,
                    Model.TotalItems));
                links.Append("</li>");
            }
            if (showPagerItems && (Model.TotalPages > 1))
            {
                if (showFirst)
                {
                    //first page
                    if ((Model.PageIndex >= 3) && (Model.TotalPages > individualPagesDisplayedCount))
                    {
                        links.Append(CreatePageLink(1, "Primero", "first-page"));
                    }
                }
                if (showPrevious)
                {
                    //previous page
                    if (Model.PageIndex > 0)
                    {
                        links.Append(CreatePageLink(Model.PageIndex, "Anterior", "previous-page"));
                    }
                }
                if (showIndividualPages)
                {
                    //individual pages
                    int firstIndividualPageIndex = GetFirstIndividualPageIndex();
                    int lastIndividualPageIndex = GetLastIndividualPageIndex();
                    for (int i = firstIndividualPageIndex; i <= lastIndividualPageIndex; i++)
                    {
                        if (Model.PageIndex == i)
                        {
                            links.AppendFormat("<li class=\"current-page\"><span>{0}</span></li>", (i + 1));
                        }
                        else
                        {
                            links.Append(CreatePageLink(i + 1, (i + 1).ToString(), "individual-page"));
                        }
                    }
                }
                if (showNext)
                {
                    //next page
                    if ((Model.PageIndex + 1) < Model.TotalPages)
                    {
                        links.Append(CreatePageLink(Model.PageIndex + 2, "Siguiente", "next-page"));
                    }
                }
                if (showLast)
                {
                    //last page
                    if (((Model.PageIndex + 3) < Model.TotalPages) && (Model.TotalPages > individualPagesDisplayedCount))
                    {
                        links.Append(CreatePageLink(Model.TotalPages, "Ultimo", "last-page"));
                    }
                }
            }

            var result = links.ToString();
            if (!String.IsNullOrEmpty(result))
            {
                result = "<ul>" + result + "</ul>";
            }
            return result;
        }

        protected virtual int GetFirstIndividualPageIndex()
        {
            if ((Model.TotalPages < individualPagesDisplayedCount) ||
                ((Model.PageIndex - (individualPagesDisplayedCount/2)) < 0))
            {
                return 0;
            }
            if ((Model.PageIndex + (individualPagesDisplayedCount/2)) >= Model.TotalPages)
            {
                return (Model.TotalPages - individualPagesDisplayedCount);
            }
            return (Model.PageIndex - (individualPagesDisplayedCount/2));
        }

        protected virtual int GetLastIndividualPageIndex()
        {
            int num = individualPagesDisplayedCount/2;
            if ((individualPagesDisplayedCount%2) == 0)
            {
                num--;
            }
            if ((Model.TotalPages < individualPagesDisplayedCount) ||
                ((Model.PageIndex + num) >= Model.TotalPages))
            {
                return (Model.TotalPages - 1);
            }
            if ((Model.PageIndex - (individualPagesDisplayedCount/2)) < 0)
            {
                return (individualPagesDisplayedCount - 1);
            }
            return (Model.PageIndex + num);
        }

        protected virtual string CreatePageLink(int pageNumber, string text, string cssClass)
        {
            var liBuilder = new TagBuilder("li");
            if (!String.IsNullOrWhiteSpace(cssClass))
                liBuilder.AddCssClass(cssClass);

            var aBuilder = new TagBuilder("a");
            aBuilder.SetInnerText(text);
            aBuilder.MergeAttribute("href", UrlBuilder(pageNumber));

            liBuilder.InnerHtml += aBuilder;

            return liBuilder.ToString(TagRenderMode.Normal);
        }

        protected virtual string CreateDefaultUrl(int pageNumber)
        {
            var routeValues = new RouteValueDictionary();

            foreach (
                var key in _viewContext.RequestContext.HttpContext.Request.QueryString.AllKeys.Where(key => key != null))
            {
                var value = _viewContext.RequestContext.HttpContext.Request.QueryString[key];
                if (BooleanParameterNames.Contains(key, StringComparer.InvariantCultureIgnoreCase))
                {
                    //little hack here due to ugly MVC implementation
                    //find more info here: http://www.mindstorminteractive.com/topics/jquery-fix-asp-net-mvc-checkbox-truefalse-value/
                    if (!String.IsNullOrEmpty(value) &&
                        value.Equals("true,false", StringComparison.InvariantCultureIgnoreCase))
                    {
                        value = "true";
                    }
                }
                routeValues[key] = value;
            }

            if (pageNumber > 1)
            {
                routeValues[PageQueryName] = pageNumber;
            }
            else
            {
                //SEO. we do not render pageindex query string parameter for the first page
                if (routeValues.ContainsKey(PageQueryName))
                {
                    routeValues.Remove(PageQueryName);
                }
            }

            var url = UrlHelper.GenerateUrl(null, null, null, routeValues, RouteTable.Routes, _viewContext.RequestContext,
                true);
            return url;
        }
    }
}