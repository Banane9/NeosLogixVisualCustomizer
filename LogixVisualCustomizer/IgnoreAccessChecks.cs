using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: System.Runtime.CompilerServices.IgnoresAccessChecksTo("FrooxEngine")]
[assembly: System.Runtime.CompilerServices.IgnoresAccessChecksTo("BaseX")]
[assembly: System.Runtime.CompilerServices.IgnoresAccessChecksTo("CodeX")]

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    internal sealed class IgnoresAccessChecksToAttribute : Attribute
    {
        public string AssemblyName { get; }

        public IgnoresAccessChecksToAttribute(string assemblyName)
        {
            AssemblyName = assemblyName;
        }
    }
}