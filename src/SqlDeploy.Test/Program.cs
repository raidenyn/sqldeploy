using System;
using System.Linq;
using System.Reflection;
using NUnit.Common;
using NUnitLite;

namespace SqlDeploy.Test
{
    class Program
    {
        public static void Main(string[] args)
        {
            var writter = new ExtendedTextWrapper(Console.Out);
            new AutoRun(typeof(Program).GetTypeInfo().Assembly).Execute(args, writter, Console.In);
        }
    }
}
