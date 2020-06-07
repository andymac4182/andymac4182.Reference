using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace andymac4182.Reference.Tests.Conventions.Specifications
{
    public class AllActionsMustHaveHttpGetOrHttpPostAttribute : MethodConventionSpecification
    {
        protected override string FailureMessage=>
            "All MVC actions methods must have either a [HttpGet] or [HttpPost] attribute";

        public override IEnumerable<MethodInfo> GetMethods(Type type)
            => type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

        public override bool IsSatisfiedBy(MethodInfo method) =>
            HasAttribute<HttpGetAttribute>(method) ||
            HasAttribute<HttpPostAttribute>(method);
    }
}