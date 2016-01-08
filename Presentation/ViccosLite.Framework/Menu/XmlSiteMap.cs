using System;
using System.IO;
using System.Linq;
using System.Web.Routing;
using System.Xml;
using ViccosLite.Core;
using ViccosLite.Core.Infrastructure;
using ViccosLite.Services.Security;

namespace ViccosLite.Framework.Menu
{
    public class XmlSiteMap
    {
        public XmlSiteMap()
        {
            RootNode = new SiteMapNode();
        }

        public SiteMapNode RootNode { get; set; }

        public virtual void LoadFrom(string physicalPath)
        {
            var webHelper = EngineContext.Current.Resolve<IWebHelper>();
            var filePath = webHelper.MapPath(physicalPath);
            var content = File.ReadAllText(filePath);

            if (string.IsNullOrEmpty(content))
                return;

            using (var str = new StringReader(content))
            {
                using (var xr = XmlReader.Create(str, new XmlReaderSettings
                {
                    CloseInput = true,
                    IgnoreWhitespace = true,
                    IgnoreComments = true,
                    IgnoreProcessingInstructions = true
                }))
                {
                    var doc = new XmlDocument();
                    doc.Load(xr);

                    if ((doc.DocumentElement == null) || !doc.HasChildNodes)
                        return;

                    var xmlRootNode = doc.DocumentElement.FirstChild;
                    Iterate(RootNode, xmlRootNode);
                }
            }
        }

        private static void Iterate(SiteMapNode siteMapNode, XmlNode xmlNode)
        {
            PopulateNode(siteMapNode, xmlNode);

            foreach (XmlNode xmlChildNode in xmlNode.ChildNodes)
            {
                if (!xmlChildNode.LocalName.Equals("siteMapNode", StringComparison.InvariantCultureIgnoreCase))
                    continue;

                var siteMapChildNode = new SiteMapNode();
                siteMapNode.ChildNodes.Add(siteMapChildNode);

                Iterate(siteMapChildNode, xmlChildNode);
            }
        }

        private static void PopulateNode(SiteMapNode siteMapNode, XmlNode xmlNode)
        {
            //titulo
            var softResource = GetStringValueFromAttribute(xmlNode, "softResource");
            if (!string.IsNullOrEmpty(softResource))
            {
                siteMapNode.Title = softResource;
            }
            else
            {
                siteMapNode.Title = GetStringValueFromAttribute(xmlNode, "title");
            }

            //routes, url
            var controllerName = GetStringValueFromAttribute(xmlNode, "controller");
            var actionName = GetStringValueFromAttribute(xmlNode, "action");
            var url = GetStringValueFromAttribute(xmlNode, "url");

            if (!string.IsNullOrEmpty(controllerName) && !string.IsNullOrEmpty(actionName))
            {
                siteMapNode.ControllerName = controllerName;
                siteMapNode.ActionName = actionName;

                //apply admin area as described here - http://www.nopcommerce.com/boards/t/20478/broken-menus-in-admin-area-whilst-trying-to-make-a-plugin-admin-page.aspx
                siteMapNode.RouteValues = new RouteValueDictionary { { "area", "Admin" } };
            }
            else if (!string.IsNullOrEmpty(url))
            {
                siteMapNode.Url = url;
            }

            //image URL
            siteMapNode.ImageUrl = GetStringValueFromAttribute(xmlNode, "ImageUrl");

            //permission name
            var permissionNames = GetStringValueFromAttribute(xmlNode, "PermissionNames");
            if (!string.IsNullOrEmpty(permissionNames))
            {
                var permissionService = EngineContext.Current.Resolve<IPermissionService>();
                siteMapNode.Visible = permissionNames.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Any(permissionName => permissionService.Authorize(permissionName.Trim()));
            }
            else
            {
                siteMapNode.Visible = true;
            }
        }

        private static string GetStringValueFromAttribute(XmlNode node, string attributeName)
        {
            if (node.Attributes == null || node.Attributes.Count <= 0)
                return null;

            var attribute = node.Attributes[attributeName];
            string value = null;

            if (attribute != null)
            {
                value = attribute.Value;
            }

            return value;
        }
    }
}