using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestLambda
{
    public static class Tools
    {
        public static String GetVisibility(MethodInfo accessor)
        {
            if (accessor.IsPublic)
                return "Public";
            else if (accessor.IsPrivate)
                return "Private";
            else if (accessor.IsFamily)
                return "Protected";
            else if (accessor.IsAssembly)
                return "Internal/Friend";
            else
                return "Protected Internal/Friend";
        }

        public static void DisplayPropertyInfo(PropertyInfo[] propInfos)
        {
            // Display information for all properties.
            foreach (var propInfo in propInfos)
            {
                bool readable = propInfo.CanRead;
                bool writable = propInfo.CanWrite;

                Console.WriteLine("   Property name: {0}", propInfo.Name);
                Console.WriteLine("   Property type: {0}", propInfo.PropertyType);
                Console.WriteLine("   Read-Write:    {0}", readable & writable);
                if (readable)
                {
                    MethodInfo getAccessor = propInfo.GetMethod;
                    Console.WriteLine("   Visibility:    {0}",
                                      GetVisibility(getAccessor));
                }
                if (writable)
                {
                    MethodInfo setAccessor = propInfo.SetMethod;
                    Console.WriteLine("   Visibility:    {0}",
                                      GetVisibility(setAccessor));
                }
                Console.WriteLine();
            }
        }

        public static PropertyInfo[]? GetObjectProperties(Object o)
        {
            var type = o?.GetType();

            return type?.GetProperties(); //BindingFlags.Public|BindingFlags.Instance
        }


        public static PropertyInfo? GetObjectProperty(object o, string name)
        {
            return o?.GetType().GetProperty(name);
        }

        public static string? GetObjectPropertyValue(object o, string n)
        {
            return GetObjectProperty(o, n)?.GetValue(o)?.ToString();
        }

        public static void SetObjectPropertyValue(Object o, string n, Object v)
        {
            GetObjectProperty(o, n)?.SetValue(o, v);
        }

        public static MethodInfo? GetObjectMethod(object o, string name)
        {
            return o?.GetType().GetMethod(name,
                BindingFlags.Instance | BindingFlags.Public);
        }


        public static object? InvokeObjectMethod(object o, string name, Object?[]? parameters)
        {
            var methodInfo = GetObjectMethod(o, name);

            //if (methodInfo == null) return null;
            // if (!method.ReturnType.Equals(typeof(string))) return null;

            return methodInfo?.Invoke(o, parameters);
        }

        public static object? InvokeObjectMethod(object o, string name, Func<string, string> par)
        {
            return InvokeObjectMethod(o, name, new object[] { par });
        }

    }
}
