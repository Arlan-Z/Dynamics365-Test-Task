using System;

namespace AwaraIT.Training.Plugins.PluginExtensions.Enums
{
    public static class CrmMessage
    {
        public static readonly string Assign = nameof(Assign);
        public static readonly string Create = nameof(Create);
        public static readonly string Update = nameof(Update);
        public static readonly string Delete = nameof(Delete);
        public static readonly string Associate = nameof(Associate);
        public static readonly string Disassociate = nameof(Disassociate);
        public static readonly string Retrieve = nameof(Retrieve);
        public static readonly string Recalculate = nameof(Recalculate);
        public static readonly string AddToQueue = nameof(AddToQueue);
        public static readonly string QualifyLead = nameof(QualifyLead);
        [Obsolete("После события SetState все равно идет Update. Используйте только если на 146% уверены в своих действиях.")]
        public static readonly string SetState = nameof(SetState);
    }
}
