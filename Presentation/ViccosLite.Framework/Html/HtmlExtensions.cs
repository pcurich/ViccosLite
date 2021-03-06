﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using ViccosLite.Framework.Mvc;

namespace ViccosLite.Framework.Html
{
    public static class HtmlExtensions
    {
        #region Extensiones para el area de Admin

        public static MvcHtmlString Hint(this HtmlHelper helper, string value)
        {
            // creamos el tag
            var builder = new TagBuilder("img");

            // Agregamos el atributo
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var url = MvcHtmlString.Create(urlHelper.Content("~/Content/images/ico-help.gif")).ToHtmlString();

            builder.MergeAttribute("src", url);
            builder.MergeAttribute("alt", value);
            builder.MergeAttribute("title", value);

            // Render tag
            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString DeleteConfirmation<T>(this HtmlHelper<T> helper, string buttonsSelector)
            where T : BaseSoftEntityModel
        {
            return DeleteConfirmation(helper, "", buttonsSelector);
        }

        public static MvcHtmlString DeleteConfirmation<T>(this HtmlHelper<T> helper, string actionName,
            string buttonsSelector) where T : BaseSoftEntityModel
        {
            if (String.IsNullOrEmpty(actionName))
                actionName = "Delete";

            var modalId =
                MvcHtmlString.Create(helper.ViewData.ModelMetadata.ModelType.Name.ToLower() + "-delete-confirmation")
                    .ToHtmlString();

            var deleteConfirmationModel = new DeleteConfirmationModel
            {
                Id = helper.ViewData.Model.Id,
                ControllerName = helper.ViewContext.RouteData.GetRequiredString("controller"),
                ActionName = actionName,
                WindowId = modalId
            };

            var window = new StringBuilder();
            window.AppendLine(string.Format("<div id='{0}' style='display:none;'>", modalId));
            window.AppendLine(helper.Partial("Delete", deleteConfirmationModel).ToHtmlString());
            window.AppendLine("</div>");
            window.AppendLine("<script>");
            window.AppendLine("$(document).ready(function() {");
            window.AppendLine(string.Format("$('#{0}').click(function (e) ", buttonsSelector));
            window.AppendLine("{");
            window.AppendLine("e.preventDefault();");
            window.AppendLine(string.Format("var window = $('#{0}');", modalId));
            window.AppendLine("if (!window.data('kendoWindow')) {");
            window.AppendLine("window.kendoWindow({");
            window.AppendLine("modal: true,");
            window.AppendLine(string.Format("title: '{0}',", "Está seguro?"));
            window.AppendLine("actions: ['Close']");
            window.AppendLine("});");
            window.AppendLine("}");
            window.AppendLine("window.data('kendoWindow').center().open();");
            window.AppendLine("});");
            window.AppendLine("});");
            window.AppendLine("</script>");

            return MvcHtmlString.Create(window.ToString());
        }

        public static MvcHtmlString SoftLabelFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression, bool displayHint = true)
        {
            var result = new StringBuilder();
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var hintResource = string.Empty;
            object value;
            if (metadata.AdditionalValues.TryGetValue("SoftResourceDisplayName", out value))
            {
                var resourceDisplayName = value as SoftResourceDisplayName;
                if (resourceDisplayName != null && displayHint)
                {
                    hintResource = ""; //no hay ayuda
                    result.Append(helper.Hint(hintResource).ToHtmlString());
                }
            }
            result.Append(helper.LabelFor(expression, new {title = hintResource}));
            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString OverrideStoreCheckboxFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, bool>> expression,
            Expression<Func<TModel, TValue>> forInputExpression,
            int activeStoreScopeConfiguration)
        {
            var dataInputIds = new List<string> {helper.FieldIdFor(forInputExpression)};
            return OverrideStoreCheckboxFor(helper, expression, activeStoreScopeConfiguration, null,
                dataInputIds.ToArray());
        }

        public static MvcHtmlString OverrideStoreCheckboxFor<TModel, TValue1, TValue2>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, bool>> expression,
            Expression<Func<TModel, TValue1>> forInputExpression1,
            Expression<Func<TModel, TValue2>> forInputExpression2,
            int activeStoreScopeConfiguration)
        {
            var dataInputIds = new List<string>
            {
                helper.FieldIdFor(forInputExpression1),
                helper.FieldIdFor(forInputExpression2)
            };
            return OverrideStoreCheckboxFor(helper, expression, activeStoreScopeConfiguration, null,
                dataInputIds.ToArray());
        }

        public static MvcHtmlString OverrideStoreCheckboxFor<TModel, TValue1, TValue2, TValue3>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, bool>> expression,
            Expression<Func<TModel, TValue1>> forInputExpression1,
            Expression<Func<TModel, TValue2>> forInputExpression2,
            Expression<Func<TModel, TValue3>> forInputExpression3,
            int activeStoreScopeConfiguration)
        {
            var dataInputIds = new List<string>
            {
                helper.FieldIdFor(forInputExpression1),
                helper.FieldIdFor(forInputExpression2),
                helper.FieldIdFor(forInputExpression3)
            };
            return OverrideStoreCheckboxFor(helper, expression, activeStoreScopeConfiguration, null,
                dataInputIds.ToArray());
        }

        public static MvcHtmlString OverrideStoreCheckboxFor<TModel>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, bool>> expression,
            string parentContainer,
            int activeStoreScopeConfiguration)
        {
            return OverrideStoreCheckboxFor(helper, expression, activeStoreScopeConfiguration, parentContainer);
        }

        private static MvcHtmlString OverrideStoreCheckboxFor<TModel>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, bool>> expression,
            int activeStoreScopeConfiguration,
            string parentContainer = null,
            params string[] datainputIds)
        {
            if (String.IsNullOrEmpty(parentContainer) && datainputIds == null)
                throw new ArgumentException("Specify at least one selector");

            var result = new StringBuilder();
            if (activeStoreScopeConfiguration > 0)
            {
                //render only when a certain store is chosen
                const string CSS_CLASS = "multi-store-override-option";
                var dataInputSelector = "";
                if (!String.IsNullOrEmpty(parentContainer))
                {
                    dataInputSelector = "#" + parentContainer + " input, #" + parentContainer + " textarea, #" +
                                        parentContainer + " select";
                }
                if (datainputIds != null && datainputIds.Length > 0)
                {
                    dataInputSelector = "#" + String.Join(", #", datainputIds);
                }
                var onClick = string.Format("checkOverriddenStoreValue(this, '{0}')", dataInputSelector);
                result.Append(helper.CheckBoxFor(expression, new Dictionary<string, object>
                {
                    {"class", CSS_CLASS},
                    {"onclick", onClick},
                    {"data-for-input-selector", dataInputSelector}
                }));
            }
            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString RenderSelectedTabIndex(this HtmlHelper helper, int currentIndex, int indexToSelect)
        {
            if (helper == null)
                throw new ArgumentNullException("helper");

            //ensure it's not negative
            if (indexToSelect < 0)
                indexToSelect = 0;

            //required validation
            return indexToSelect == currentIndex
                ? new MvcHtmlString(" class='k-state-active'")
                : new MvcHtmlString("");
        }

        #endregion

        #region Extensiones comunes

        public static MvcHtmlString RequiredHint(this HtmlHelper helper, string additionalText = null)
        {
            // Create tag builder
            var builder = new TagBuilder("span");
            builder.AddCssClass("required");
            var innerText = "*";
            //add additional text if specified
            if (!String.IsNullOrEmpty(additionalText))
                innerText += " " + additionalText;
            builder.SetInnerText(innerText);
            // Render tag
            return MvcHtmlString.Create(builder.ToString());
        }

        public static string FieldNameFor<T, TResult>(this HtmlHelper<T> html, Expression<Func<T, TResult>> expression)
        {
            return html.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
        }

        public static string FieldIdFor<T, TResult>(this HtmlHelper<T> html, Expression<Func<T, TResult>> expression)
        {
            var id = html.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            // because "[" and "]" aren't replaced with "_" in GetFullHtmlFieldId
            return id.Replace('[', '_').Replace(']', '_');
        }

        public static MvcHtmlString DatePickerDropDowns(this HtmlHelper html,
            string dayName, string monthName, string yearName,
            int? beginYear = null, int? endYear = null,
            int? selectedDay = null, int? selectedMonth = null, int? selectedYear = null)
        {
            var daysList = new TagBuilder("select");
            var monthsList = new TagBuilder("select");
            var yearsList = new TagBuilder("select");

            daysList.Attributes.Add("name", dayName);
            monthsList.Attributes.Add("name", monthName);
            yearsList.Attributes.Add("name", yearName);

            var days = new StringBuilder();
            var months = new StringBuilder();
            var years = new StringBuilder();

            const string DAY_LOCALE = "Día";
            const string MONTH_LOCALE = "Mes";
            const string YEAR_LOCALE = "Año";

            days.AppendFormat("<option value='{0}'>{1}</option>", "0", DAY_LOCALE);
            for (var i = 1; i <= 31; i++)
                days.AppendFormat("<option value='{0}'{1}>{0}</option>", i,
                    (selectedDay.HasValue && selectedDay.Value == i) ? " selected=\"selected\"" : null);


            months.AppendFormat("<option value='{0}'>{1}</option>", "0", MONTH_LOCALE);
            for (var i = 1; i <= 12; i++)
            {
                months.AppendFormat("<option value='{0}'{1}>{2}</option>",
                    i,
                    (selectedMonth.HasValue && selectedMonth.Value == i) ? " selected=\"selected\"" : null,
                    CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(i));
            }


            years.AppendFormat("<option value='{0}'>{1}</option>", "0", YEAR_LOCALE);

            if (beginYear == null)
                beginYear = DateTime.UtcNow.Year - 100;
            if (endYear == null)
                endYear = DateTime.UtcNow.Year;

            if (endYear > beginYear)
            {
                for (var i = beginYear.Value; i <= endYear.Value; i++)
                    years.AppendFormat("<option value='{0}'{1}>{0}</option>", i,
                        (selectedYear.HasValue && selectedYear.Value == i) ? " selected=\"selected\"" : null);
            }
            else
            {
                for (var i = beginYear.Value; i >= endYear.Value; i--)
                    years.AppendFormat("<option value='{0}'{1}>{0}</option>", i,
                        (selectedYear.HasValue && selectedYear.Value == i) ? " selected=\"selected\"" : null);
            }

            daysList.InnerHtml = days.ToString();
            monthsList.InnerHtml = months.ToString();
            yearsList.InnerHtml = years.ToString();

            return MvcHtmlString.Create(string.Concat(daysList, monthsList, yearsList));
        }

        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes, string suffix)
        {
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var resolvedLabelText = metadata.DisplayName ?? (metadata.PropertyName ?? htmlFieldName.Split('.').Last());
            if (string.IsNullOrEmpty(resolvedLabelText))
            {
                return MvcHtmlString.Empty;
            }
            var tag = new TagBuilder("label");
            tag.Attributes.Add("for",
                TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName)));
            if (!String.IsNullOrEmpty(suffix))
            {
                resolvedLabelText = String.Concat(resolvedLabelText, suffix);
            }
            tag.SetInnerText(resolvedLabelText);

            var dictionary = ((IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            tag.MergeAttributes(dictionary, true);

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        #endregion
    }
}