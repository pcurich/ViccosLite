using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ViccosLite.Core;
using ViccosLite.Core.Infrastructure;
using ViccosLite.Framework.Kendoui;
using ViccosLite.Services.Helpers;

namespace ViccosLite.Framework.Html
{
    public static class Extensions
    {
        public static IEnumerable<T> PagedForCommand<T>(this IEnumerable<T> current, DataSourceRequest command)
        {
            // (2-1)*10 -> desde los 10 toma los siguinetes 10  -> tomo los 10 de la siguiente pagina 
            return current.Skip((command.Page - 1) * command.PageSize).Take(command.PageSize);
        }

        public static SelectList ToSelectList<TEnum>(this TEnum enumObj,
            bool markCurrentAsSelected = true, int[] valuesToExclude = null) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum) throw new ArgumentException("Un tipo de enumaracion es requerido.", "enumObj");

            var workContext = EngineContext.Current.Resolve<IWorkContext>();

            var values = from TEnum enumValue in Enum.GetValues(typeof(TEnum))
                         where valuesToExclude == null || !valuesToExclude.Contains(Convert.ToInt32(enumValue))
                         select
                             new
                             {
                                 ID = Convert.ToInt32(enumValue),
                                 Name = CommonHelper.ConvertEnum(enumValue.ToString())
                             };
            object selectedValue = null;
            if (markCurrentAsSelected)
                selectedValue = Convert.ToInt32(enumObj);
            return new SelectList(values, "ID", "Name", selectedValue);
        }

        public static string RelativeFormat(this DateTime source)
        {
            return RelativeFormat(source, string.Empty);
        }

        public static string RelativeFormat(this DateTime source, string defaultFormat)
        {
            return RelativeFormat(source, false, defaultFormat);
        }

        public static string RelativeFormat(this DateTime source,
            bool convertToUserTime, string defaultFormat)
        {
            var result = "";

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - source.Ticks);
            var delta = ts.TotalSeconds;

            if (delta > 0)
            {
                if (delta < 60) // 60 (seconds)
                {
                    result = ts.Seconds == 1 ? "hace un segundo" : ts.Seconds + " segundos atras";
                }
                else if (delta < 60 * 2) //2 (minutes) * 60 (seconds)
                {
                    result = "un minuto atras";
                }
                else if (delta < 60 * 45) // 45 (minutes) * 60 (seconds)
                {
                    result = ts.Minutes + " minutos atras";
                }
                else if (delta < 60 * 90) // 90 (minutes) * 60 (seconds)
                {
                    result = "una hora atras";
                }
                else if (delta < 60 * 60 * 24) // 24 (hours) * 60 (minutes) * 60 (seconds)
                {
                    var hours = ts.Hours;
                    if (hours == 1)
                        hours = 2;
                    result = hours + " horas atras";
                }
                else if (delta < 60 * 60 * 48) // 48 (hours) * 60 (minutes) * 60 (seconds)
                {
                    result = "ayer";
                }
                else if (delta < 60 * 60 * 24 * 30) // 30 (days) * 24 (hours) * 60 (minutes) * 60 (seconds)
                {
                    result = ts.Days + " dias atras";
                }
                else if (delta < 60 * 60 * 24 * 30 * 12) // 12 (months) * 30 (days) * 24 (hours) * 60 (minutes) * 60 (seconds)
                {
                    var months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                    result = months <= 1 ? "un mes atras" : months + " meses atras";
                }
                else
                {
                    var years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                    result = years <= 1 ? "un año atras" : years + " años atras";
                }
            }
            else
            {
                var tmp1 = source;
                if (convertToUserTime)
                {
                    tmp1 = EngineContext.Current.Resolve<IDateTimeHelper>().ConvertToUserTime(tmp1, DateTimeKind.Utc);
                }
                //formato default
                result = !String.IsNullOrEmpty(defaultFormat)
                    ? tmp1.ToString(defaultFormat)
                    : tmp1.ToString(CultureInfo.InvariantCulture);
            }
            return result;
        }
    }
}