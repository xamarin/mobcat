using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAPI.Constants;
using NewsAPI.Models;

namespace News.Services.Abstractions
{
    public interface INewsDataService
    {
        Task<IEnumerable<Article>> FetchArticlesByCategory(Categories? category = null);
        Task<IEnumerable<Article>> FetchArticlesBySource(string source);
        Task<IEnumerable<Article>> FetchArticlesBySearchQuery(string query);
    }
}
