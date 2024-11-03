using AwaraIT.Training.Plugins.PluginExtensions.Enums;
using Microsoft.Xrm.Sdk;
using System;

namespace AwaraIT.Training.Plugins.PluginExtensions.Attributes
{
    public static class Argument
    {
        [AssertionMethod]
        public static void NotNull([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] object value, [NotNull] string message)
        {
            if (value == null)
                throw new ArgumentNullException(message);
        }

        [AssertionMethod]
        public static void NotNullOrEmpty([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] string value, [NotNull] string message)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException(message);
        }

        [AssertionMethod]
        public static void Require([AssertionCondition(AssertionConditionType.IS_TRUE)] bool condition, [NotNull] string errorMessage)
        {
            if (!condition)
                throw new ArgumentException(errorMessage);
        }

        [AssertionMethod]
        public static void RequireField(
            Entity entity,
            string fieldName,
            string localizedFieldName,
            string localizedEntityName = null)
        {
            if (!entity.Contains(fieldName) || entity[fieldName] == null)
            {
                string str = string.Format("Не указано обязательное поле «{0}»", (object)localizedFieldName);
                throw new InvalidPluginExecutionException(string.IsNullOrEmpty(localizedEntityName) ? str + string.Format(" (код сущности: {0})", (object)entity.LogicalName) : str + string.Format(" для сущности {0}", (object)localizedEntityName));
            }
        }
    }
}
