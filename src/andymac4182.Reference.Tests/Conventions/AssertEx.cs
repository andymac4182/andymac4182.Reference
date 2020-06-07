using System;
using Xunit;

namespace andymac4182.Reference.Tests.Conventions
{
    public static class AssertEx
    {
        public static readonly Action<string> Fail = s => Assert.False(true, s);
    }
}