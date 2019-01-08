using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.ViewModels
{
    /// <summary>
    /// Source news view model.
    /// </summary>
    public class SourceNewsViewModel : BaseNewsViewModel
    {
        public string Title { get; }

        public string Source { get; }

        public SourceNewsViewModel(string title, string source)
        {
            Title = title;
            Source = source;
        }

        protected async override Task<IEnumerable<ArticleViewModel>> FetchArticlesAsync()
        {
            System.Diagnostics.Debug.WriteLine($"{GetType().Name} FetchArticlesAsync for {Title} Source");
            var articles = await NewsDataService.FetchArticlesBySource(Source);
            var result = articles.Select(a => new ArticleViewModel(a)).ToList();
            return result;
        }
    }
}