using System;
using MyInterfaces;

namespace MyLib
{
    [Serializable]
    public class MyLibClass : IHasName
    {
        public string Name => "Mit Library 1";

        public MyLibClass()
        {
        }

        public string GetName(string anotherPostfix)
        {
            return anotherPostfix + this.Name;
        }
    }

    [Serializable]
    public class MyLibClass2 : IHasName
    {
        public string Name => "Mit Library 2";

        public MyLibClass2()
        {
        }

        public string GetName(string anotherPostfix)
        {
            return anotherPostfix + this.Name;
        }
    }
}
