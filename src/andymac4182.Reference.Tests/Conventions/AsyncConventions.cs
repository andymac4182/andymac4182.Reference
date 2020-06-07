using System;
using System.Linq;
using Conventional;
using MassTransit;
using Xunit;

namespace andymac4182.Reference.Tests.Conventions
{
    public class AsyncConventions
    {
        
        [Fact]
        public void AsyncMethodsMustNotBeVoid()
        {
            MyAssemblies.ExportedTypes
                .MustConformTo(Convention.VoidMethodsMustNotBeAsync)
                .WithFailureAssertion(AssertEx.Fail);
        }

        [Fact]
        public void AsyncMethodsNamesMustEndInAsync()
        {
            _exportedTypesExcludingConsumersAndMain
                .Except(new []{typeof(andymac4182.Reference.Web.Program), typeof(andymac4182.Reference.Service.Program)})
                .MustConformTo(Convention.AsyncMethodsMustHaveAsyncSuffix)
                .WithFailureAssertion(AssertEx.Fail);
        }

        private readonly Type[] _exportedTypesExcludingConsumersAndMain =
            MyAssemblies.ExportedTypes
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConsumer<>)) == false && t != typeof(Web.Program))
                .ToArray();
    }
}