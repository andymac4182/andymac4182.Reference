using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Conventional;
using Conventional.Conventions;

namespace andymac4182.Reference.Tests.Conventions.Specifications
{
    public abstract class MethodConventionSpecification : ConventionSpecification
    {
        public abstract IEnumerable<MethodInfo> GetMethods(Type type);
        public abstract bool IsSatisfiedBy(MethodInfo method);

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var failures = GetMethods(type).Where(method => !IsSatisfiedBy(method)).ToList();

            if (failures.Any())
            {
                var details = failures.Aggregate(string.Empty, (s, method) =>
                    $"{s}\t- {FullMethodName(method)}{Environment.NewLine}");

                var failureMessage = BuildFailureMessage(details);

                return ConventionResult.NotSatisfied(type.FullName, failureMessage);
            }

            return ConventionResult.Satisfied(type.FullName);
        }

        private string FullMethodName(MethodInfo method)
        {
            var fullTypeName = $"{method.DeclaringType?.FullName}.{method.Name}";

            var methodParameters = method.GetParameters()
                .Select(p => $"{TypeNameIncludingNested(p.ParameterType)}")
                .Join(",");

            return $"{fullTypeName}({methodParameters})";
        }

        public bool HasAttribute<TAttribute>(MethodInfo subject) where TAttribute : Attribute
            => subject.GetCustomAttributes(typeof(TAttribute), false).Any();

        public string TypeNameIncludingNested(Type type)
        {
            if (type.DeclaringType == null)
                return type.Name;

            return TypeNameIncludingNested(type.DeclaringType) + "." + type.Name;
        }

        public bool IsSimpleType(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return IsSimpleType(type.GetGenericArguments()[0]);
            }

            return type.IsPrimitive
                   || type.IsEnum
                   || type == typeof(string)
                   || type == typeof(decimal);
        }
    }
}