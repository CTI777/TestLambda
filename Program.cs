// See https://aka.ms/new-console-template for more information
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using SolrNet.Utils;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace TestLambda
{
    public class MyObject
    {

        public string? name { get; set; }
        public string? value { get; set; }

        public void log()
        {
            Console.WriteLine($"MyObject: name={name}, value={value}");
        }
        public void Method1(Func<string?, string> fpar)
        {
            name = fpar(name);
        }

        public void Method2(Func<string?, string> fpar) {
            value = fpar(value);
        } 
    }



    class TestLambda
    {
        static void Main()
        {
            MyObject o = new MyObject()
            {   name = "My Name",
                value = "My Value",
            };
            o.log();


            PropertyInfo[] propInfos = Tools.GetObjectProperties(o);
            Tools.DisplayPropertyInfo(propInfos);

            string n = "name";
            string? v = Tools.GetObjectPropertyValue(o,n);
            Console.WriteLine($"GetObjectPropertyValue {n} => {v}");

            Tools.SetObjectPropertyValue(o, "name", "New Name");
            Tools.SetObjectPropertyValue(o, "value", "New Value");
            o.log();

            //Lambda
            n = "name";
            v=LambdaTools.GetObjectPropertyValue(o,n);
            Console.WriteLine($"Lambda.GetObjectPropertyValue {n} => {v}");

            LambdaTools.SetObjectPropertyValue(o, "name", "New Lambda Name");
            LambdaTools.SetObjectPropertyValue(o, "value", "New Lambda Value");
            o.log();

            //Tests
            int i;
            Func<int,int> x2 = x => x*x;
            Func<int, Func<int, int>, int> ff = (x,f) => f(x);
            i = ff(3,x2);
            Console.WriteLine($"i={i}");

    
            Func<string,string> upper = x => x.ToUpper();
            Func<string, string> lower = x => x.ToLower();

            Action<object, string, Func<string, string>> callFunction = (o, n, fpar) => LambdaTools.InvokeObjectMethodOnePar(o, n, fpar);
            callFunction(o, "Method1", lower);
            callFunction(o, "Method2", upper);
            o.log();


            Action<object, string, Func<string, string>> callFunctions;
            callFunctions = (o,n,fpar) => Tools.InvokeObjectMethod(o,n,fpar);
            callFunctions += (o, n,fpar) => Tools.InvokeObjectMethod(o, n,fpar);

            i = 1;
            foreach (var function in callFunctions.GetInvocationList())
            {
                var item= (Action<object, string, Func<string, string>>)function;
                switch (i) {
                    case 1: item(o, "Method" + i, upper);break;
                    case 2: item(o, "Method" + i, lower); break;
                }
                
                i++;
            }
            o.log();

        }
    }


}
