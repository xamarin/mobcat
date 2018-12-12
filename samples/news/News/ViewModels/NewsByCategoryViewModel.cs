using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAPI.Models;

namespace News.ViewModels
{
    /// <summary>
    /// News by category view model.
    /// </summary>
    public class NewsByCategoryViewModel : BaseGrouppedNewsViewModel
    {
        protected override Task<IEnumerable<Article>> FetchArticlesAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}