﻿using AwaraIT.Training.Plugins.PluginExtensions.Enums;
using System;

namespace AwaraIT.Training.Plugins.PluginExtensions.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class AssertionConditionAttribute : Attribute
    {
        public AssertionConditionAttribute(AssertionConditionType conditionType)
        {
            this.ConditionType = conditionType;
        }

        public AssertionConditionType ConditionType { get; private set; }
    }
}
