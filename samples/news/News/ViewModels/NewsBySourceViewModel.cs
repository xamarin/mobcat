using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAPI.Models;

namespace News.ViewModels
{
    /// <summary>
    /// News by source view model.
    /// </summary>
    public class NewsBySourceViewModel : BaseGrouppedNewsViewModel
    {
        protected override Task<IEnumerable<Article>> FetchArticlesAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}