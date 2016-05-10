using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace MvcContractorShareApplication.Enumerations
{
    public static class EnumHelper
    {
        public static string GetDescription(this Enum Enumeration)
        {
            string Value = Enumeration.ToString();
            Type EnumType = Enumeration.GetType();
            var DescAttribute = (DescriptionAttribute[])EnumType
                .GetField(Value)
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return DescAttribute.Length > 0 ? DescAttribute[0].Description : Value;
        }
    }
}