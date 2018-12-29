using System.Collections.Generic;
using NewsAPI.Models;

namespace News.Services.Abstractions
{
    public interface IFavoritesService
    {
        bool IsFavorite(Article article);
        void Add(Article article);
        void Remove(Article article);
        IEnumerable<Article> Get();
    }
}
