using System;

namespace AwaraIT.Training.Plugins.PluginExtensions.Attributes
{
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Delegate)]
    public sealed class StringFormatMethodAttribute : Attribute
    {
        public StringFormatMethodAttribute(string formatParameterName)
        {
            this.FormatParameterName = formatParameterName;
        }

        public string FormatParameterName { get; private set; }
    }
}
