using Android.Runtime;
using Java.Interop;

namespace FountainSharpWrapperDroid
{
    [Register("FountainSharpWrapper")]
    public static class FountainSharpWrapper
    {
        [Export("ConvertToHtml")]
        public static string ConvertToHtml(string FountainText)
        {
            return FountainSharp.HtmlFormatter.TransformHtml(FountainText);
        }
    }
}
