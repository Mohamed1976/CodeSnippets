using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace CodeSnippets.Enums
{
    public class Helper
    {
        public static string GetDescription(Enum genericEnum)
        {
            Type type = genericEnum.GetType();
            MemberInfo[] memInfo = type.GetMember(genericEnum.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return genericEnum.ToString();
        }

        //Enum provides the base class for enumerations
        //For example you could pass "OrderStatusEnum.Approved" to method, "GetColorEnums(Enum genericEnum)" 
        //and Enum.GetNames(typeof(genericEnum.GetType()))  
        public static string GetEnumNames(Type enumType)
        {
            var stringBuilder = new StringBuilder();
            foreach (string name in Enum.GetNames(enumType))
            {
                stringBuilder.Append(name + "|");
            }
            return stringBuilder.ToString();
        }

        public static string GetEnumNames<T>() where T : System.Enum
        {
            var stringBuilder = new StringBuilder();
            foreach (string name in Enum.GetNames(typeof(T)))
            {
                stringBuilder.Append(name + "|");
            }
            return stringBuilder.ToString();
        }

        public static Dictionary<string, int> EnumNamedValues<T>() where T : System.Enum
        {
            var result = new Dictionary<string, int>();
            var values = Enum.GetValues(typeof(T));

            //Foreach specified value in the enumeration, retrieve the corresponding name 
            foreach (int item in values)
            {
                result.Add(Enum.GetName(typeof(T), item), item);
            }

            return result;
        }
    }
}

//public enum Days { Sun, Mon, Tues, Wed, Thurs, Fri, Sat };
//private Dictionary<Days, LogMessage> dailyMessages = new Dictionary<Days, LogMessage>();
//foreach (Days day in Enum.GetValues(typeof(Days)))
//{
//    dailyMessages[day] = new LogMessage("Default message");
//}