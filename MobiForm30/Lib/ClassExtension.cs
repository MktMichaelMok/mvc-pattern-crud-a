using MvcPaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace MobiForm30.Lib
{
    public static class ClassExtension
    {
        public static IEnumerable<string> GetAllField(this object o)
        {
            var t = o.GetType();
            foreach (var item in t.GetProperties(System.Reflection.BindingFlags.Public))
            {
                if (item != null)
                    yield return item.Name;
            }
        }

        public static IEnumerable<SelectListItem> GetSelectListByStringList(this List<string> o)
        {
            foreach (var item in o.GetAllField().ToList())
            {
                yield return new SelectListItem() { Text = item, Value = item };
            }
        }
    }

    public static class LabelExtensions
    {
        public static MvcHtmlString DisplaySortColumnStyle(this HtmlHelper helper, string columnName, string displayName, string actionName)
        {
            string result = columnName;
            var routeDic = new RouteValueDictionary();
            var currentSortType = (helper.ViewBag.SortType as string) == "asc" ? "desc" : "asc";

            if (columnName.Equals(helper.ViewBag.SortColumnName, StringComparison.OrdinalIgnoreCase)) {
                //string displayColumnName = string.Format("<span style=\"color: red;\">{0} {1}</span>",
                //    columnName,
                //    helper.ViewBag.SortType.Equals("asc", StringComparison.OrdinalIgnoreCase) ? "▲" : "▼");                

                string displayColumnName = string.Format("{0} {1}",
                   displayName,
                   currentSortType.Equals("asc", StringComparison.OrdinalIgnoreCase) ? "▼" : "▲");
                result = displayColumnName;
            }
            else
                result = displayName;

            routeDic.Add("Page", helper.ViewBag.page);
            routeDic.Add("SortCol", columnName);
            routeDic.Add("SortType", currentSortType);
            
            return helper.ActionLink(result, actionName, routeDic);            
        }

        
        public static MvcHtmlString DisplaySortColumnStyle(this AjaxHelper helper, string columnName, string displayName, string actionName, AjaxOptions ajaxOption)
        {
            string result = columnName;
            var routeDic = new RouteValueDictionary();
            var currentSortType = (helper.ViewBag.SortType as string) == "asc" ? "desc" : "asc";

            if (columnName.Equals(helper.ViewBag.SortCol, StringComparison.OrdinalIgnoreCase)) {
                //string displayColumnName = string.Format("<span style=\"color: red;\">{0} {1}</span>",
                //    columnName,
                //    helper.ViewBag.SortType.Equals("asc", StringComparison.OrdinalIgnoreCase) ? "▲" : "▼");                

                string displayColumnName = string.Format("{0} {1}",
                   displayName,
                   currentSortType.Equals("asc", StringComparison.OrdinalIgnoreCase) ? "▼" : "▲");
                result = displayColumnName;
            }
            else
                result = displayName;

            routeDic.Add("Page", helper.ViewBag.page);
            routeDic.Add("SortCol", columnName);
            routeDic.Add("SortType", currentSortType);

            return helper.ActionLink(result, actionName, routeDic, ajaxOption);
        }      

    }
}