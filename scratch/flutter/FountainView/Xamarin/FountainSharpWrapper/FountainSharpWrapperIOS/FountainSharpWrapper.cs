using System;
namespace FountainSharpWrapperIOS
{
    public static class FountainSharpWrapper
    {
        public static string ConvertToHtml(string FountainText)
        {
            return FountainSharp.HtmlFormatter.TransformHtml(FountainText);
        }
    }
}
