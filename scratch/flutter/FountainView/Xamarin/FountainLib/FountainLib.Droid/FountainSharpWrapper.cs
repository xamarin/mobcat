using System;
using Android.Runtime;
using Java.Interop;

namespace FountainLib.Droid
{
    [Register("mono.embeddinator.android.FountainSharpWrapper")]
    public static class FountainSharpWrapper
    {
        [Export("ConvertToHtml")]
        public static string ConvertToHtml(string FountainText)
        {
            return FountainSharp.HtmlFormatter.TransformHtml(FountainText);
        }
    }
}
