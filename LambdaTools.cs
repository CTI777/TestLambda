using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestLambda
{
    public static class LambdaTools
    {

        public static Func<Object, string, PropertyInfo?> GetObjectProperty = (o, n) => o?.GetType().GetProperty(n);
        public static Func<Object, string, string?> GetObjectPropertyValue = (o, n) => GetObjectProperty(o, n)?.GetValue(o)?.ToString();
        public static Action<Object, string, string> SetObjectPropertyValue = (o, n, v) => GetObjectProperty(o, n)?.SetValue(o, v);


        public static Func<Object, string, MethodInfo?> GetObjectMethod = (o, n) => o?.GetType().GetMethod(n, BindingFlags.Instance | BindingFlags.Public);
        public static Func<Object, string, Object?[]?, object?> InvokeObjectMethod = (o, n, pars) => GetObjectMethod(o, n)?.Invoke(o, pars);
        public static Func<Object, string, Func<string, string>?, object?> InvokeObjectMethodOnePar = (o, n, par) => GetObjectMethod(o, n)?.Invoke(o, par != null ? new Object[] { par } : null);
    }
}
