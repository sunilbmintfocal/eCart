using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MintCart.Enum
{
    public static class EnumHelper
    {
        public static string GetDescription(System.Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }

        public static TEnum? TryParseEnum<TEnum>(string value, bool ignoreCase = true) where TEnum : struct, System.Enum
        {
            if (System.Enum.TryParse(typeof(TEnum), value, ignoreCase, out object? result))
            {
                return (TEnum?)result ?? default!;
            }
            return default!;
        }

        public static int? TryParseEnumToInt<TEnum>(string value, bool ignoreCase = true) where TEnum : struct, System.Enum
        {
            var enumValue = EnumHelper.TryParseEnum<TEnum>(value, ignoreCase);
            return enumValue.HasValue ? Convert.ToInt32(enumValue.Value) : (int?)null;
        }

    }
}
