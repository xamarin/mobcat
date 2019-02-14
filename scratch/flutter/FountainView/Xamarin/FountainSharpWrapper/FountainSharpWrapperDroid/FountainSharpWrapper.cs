using Android.Runtime;
using Java.Interop;

namespace FountainSharpWrapperDroid
{
    
    public static class FountainSharpWrapper
    {
        
        public static string ConvertToHtml(string FountainText)
        {
            return FountainSharp.HtmlFormatter.TransformHtml(FountainText);
        }
    }
}
