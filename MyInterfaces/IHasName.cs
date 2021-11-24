using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyInterfaces
{
    public interface IHasName
    {
        string Name { get; }
        string GetName(string anotherPostfix);
    }
}
