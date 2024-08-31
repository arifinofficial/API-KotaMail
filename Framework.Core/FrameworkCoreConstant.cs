namespace Framework.Core
{
    public partial class FrameworkCoreConstant
    {
        public static class SortOrder
        {
            public const string Ascending = "asc";
            public const string Descending = "desc";
        }

        public static class DateSetting
        {
            public const string CompactShortDateFormat = "yyyyMMdd";
            public const string HumanReadableShortDateFormat = "MMM dd, yyyy";
            public const string HumanReadableLongDateFormat = "MMM dd, yyyy HH:mm";
            public const string MySqlShortDateFormat = "yyyy-MM-dd";
        }

        public static class CustomClaimTypes
        {
            public const string UserId = "https://fintastic.dev/claims/userid";
        }
    }
}
