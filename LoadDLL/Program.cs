using MyInterfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace LoadDLL
{
    public class ProxyObject<T> : MarshalByRefObject where T : class
    {
        private Type type;
        protected T Instance { get; private set; }
        private Assembly assembly;

        public ProxyObject(AppDomain inDomain, Assembly assembly, string typeName, object[] args = null)
        {
            this.assembly = assembly;
            this.type = this.assembly.GetType(typeName);
            this.Instance = Activator.CreateInstance(this.type, args) as T;
        }

        public object InvokeMethod(string methodName, object[] args = null)
        {
            var methodinfo = this.type.GetMethod(methodName);
            return methodinfo.Invoke(Instance, args);
        }
    }

    public class MyLibProxy : ProxyObject<IHasName>, IHasName
    {
        public MyLibProxy(AppDomain inDomain, Assembly assembly, string typeName, object[] args = null) :
            base(inDomain, assembly, typeName, args)
        {
        }

        public string Name => this.Instance.Name;
        public string GetName(string anotherPostfix) => this.Instance.GetName(anotherPostfix);
    }

    class Program
    {   
        static void Main(string[] args)
        {
            var appBase = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var dllBase = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(appBase))), "MyLib", "bin", "debug");
            var dllPath = Path.Combine(dllBase, "MyLib.dll");
            var className = "MyLib.MyLibClass";

            var newDomain = AppDomain.CreateDomain("MyDomain", null, new AppDomainSetup()
            {
                ApplicationBase = dllBase
            });

            var assembly = Assembly.LoadFrom(dllPath);

            var proxyObject = new MyLibProxy(newDomain, assembly, className);
            Console.WriteLine(proxyObject.GetName("Prefix"));

            Console.WriteLine("Types in loaded assembly");
            var interfaceType = typeof(IHasName);
            foreach (Type type in assembly.GetTypes().Where(t => interfaceType.IsAssignableFrom(t)))
            {
                var proxyObject2 = new MyLibProxy(newDomain, assembly, type.FullName);
                Console.WriteLine(proxyObject2.GetName($"{type.FullName}: "));
            }

            Console.ReadLine();
        }
    }
}



//var appDomain = AppDomain.CreateDomain("MyLibAppDomain", null, new AppDomainSetup
//{
//    ApplicationBase = appBase,
//    ApplicationName = "MyLibAppDomain",
//    //ShadowCopyFiles = "true",
//    //PrivateBinPath = appBase
//});