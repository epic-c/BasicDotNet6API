using System.Diagnostics.CodeAnalysis;

namespace BasicDotNet6API.Exceptions
{
    public class YourBussException : Exception
    {
        public string Type { get; } = "http://basicapi.com.tw/rfc7807/businessexception";
        public string Title { get; } = "A business validation exception occurred.";
        public object ExtendedInfo { get; } // this will be used to add more info to the returned object

        public YourBussException(
            [NotNull] string message,
            object extendedInfo = null,
            string type = null,
            string title = null)
            : base(message)
        {
            ExtendedInfo = extendedInfo ?? new { };
            Type = type ?? Type;
            Title = title ?? Title;
        }
    }
}
