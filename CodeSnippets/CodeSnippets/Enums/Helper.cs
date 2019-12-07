using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Linq;

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

        public static string GetDescription2(System.Enum value)
        {
            var enumMember = value.GetType().GetMember(value.ToString()).FirstOrDefault();
            var descriptionAttribute =
                enumMember == null
                    ? default(DescriptionAttribute)
                    : enumMember.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
            return
                descriptionAttribute == null
                    ? value.ToString()
                    : descriptionAttribute.Description;
        }

        public static string GetDescription3(Enum value)
        {
            return
                value
                    .GetType()
                    .GetMember(value.ToString())
                    .FirstOrDefault()
                    ?.GetCustomAttribute<DescriptionAttribute>()
                    ?.Description
                ?? value.ToString();
        }

        public static T GetEnumValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException();
            FieldInfo[] fields = type.GetFields();
            var field = fields
                            .SelectMany(f => f.GetCustomAttributes(
                                typeof(DescriptionAttribute), false), (
                                    f, a) => new { Field = f, Att = a })
                            .Where(a => ((DescriptionAttribute)a.Att)
                                .Description == description).SingleOrDefault();
            return field == null ? default(T) : (T)field.Field.GetRawConstantValue();
        }

        public static Dictionary<int, string> GetEnumNamedValues<T>() where T : System.Enum
        {
            var result = new Dictionary<int, string>();
            var values = Enum.GetValues(typeof(T));

            foreach (int item in values)
                result.Add(item, Enum.GetName(typeof(T), item));
            return result;
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