using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;

namespace News.ViewModels
{
    /// <summary>
    /// Category news view model.
    /// </summary>
    public class CategoryNewsViewModel : BaseNewsViewModel
    {
        public string Title { get; }

        public Categories? Category { get; }

        public CategoryNewsViewModel(string title, Categories? category = null)
        {
            Title = title;
            Category = category;
        }

        protected async override Task<IEnumerable<ArticleViewModel>> FetchArticlesAsync()
        {
            System.Diagnostics.Debug.WriteLine($"{GetType().Name} FetchArticlesAsync for {Title} Category");
            var articles = await NewsDataService.FetchArticlesByCategory(Category);
            var result = articles.Select(a => new ArticleViewModel(a)).ToList();
            return result;
        }
    }
}