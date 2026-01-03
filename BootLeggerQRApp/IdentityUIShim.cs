using System;

namespace Microsoft.AspNetCore.Identity.UI
{
    // Shim for projects where the Microsoft.AspNetCore.Identity.UI package
    // adds an assembly attribute referencing UIFrameworkAttribute which isn't
    // available for this target/framework/package combination.
    [AttributeUsage(AttributeTargets.Assembly)]
    internal sealed class UIFrameworkAttribute : Attribute
    {
        public UIFrameworkAttribute(string framework)
        {
            Framework = framework;
        }

        public string Framework { get; }
    }
}
