using System;
namespace FountainLib.iOS
{
    public static class FountainSharpWrapper
    {
        public static string ConvertToHtml(string FountainText)
        {
            return FountainSharp.HtmlFormatter.TransformHtml(FountainText);
        }
    }
}
