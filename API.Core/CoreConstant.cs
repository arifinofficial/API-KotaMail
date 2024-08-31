using Framework.Core;

namespace API.Core
{
    public class CoreConstant : FrameworkCoreConstant
    {
        public static class ConnectionType
        {
            public const string Default = "DEFAULT";
            public const string AppPassword = "APP_PASSWORD";
        }

        public static class FilterMailboxParameter
        {
            public const string From = "FROM";
            public const string Subject = "SUBJECT";
        }
    }
}
