using System;
using System.Collections.Generic;
using System.Linq;
using Conventional;
using andymac4182.Reference.Tests.Conventions.Specifications;
using andymac4182.Reference.Web.Features.Error;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace andymac4182.Reference.Tests.Conventions
{
    public class MvcControllerConventions
    {
        [Fact]
        public void AllActionsMustHaveHttpGetOrHttpPostAttribute() => Controllers()
            .MustConformTo(new AllActionsMustHaveHttpGetOrHttpPostAttribute())
            .WithFailureAssertion(AssertEx.Fail);
        
        private IEnumerable<Type> Controllers() =>
            typeof(andymac4182.Reference.Web.Constants)
                .Assembly
                .GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type))
                .Except(new []
                {
                    typeof(Error)
                });
    }
}