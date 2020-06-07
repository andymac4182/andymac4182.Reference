using System;
using System.Linq;
using System.Reflection;

namespace andymac4182.Reference.Tests.Conventions
{
    public static class MyAssemblies
    {
        private static readonly Assembly[] Assemblies = {Web.Constants.WebAssembly, Constants.CoreAssembly, Service.Constants.ServiceAssembly, andymac4182.Reference.MessageContracts.Constants.MessageContractsAssembly, andymac4182.Reference.DbUp.Constants.DbUpAssembly};

        public static readonly Type[] ExportedTypes = Assemblies.SelectMany(a => a.GetExportedTypes()).ToArray();
    }
}