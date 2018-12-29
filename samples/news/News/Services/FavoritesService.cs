using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using News.Helpers;
using News.Services;
using News.Services.Abstractions;
using NewsAPI.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(FavoritesService))]
namespace News.Services
{
    public class FavoritesService : IFavoritesService
    {
        private readonly ConcurrentDictionary<string, Article> _favorites = new ConcurrentDictionary<string, Article>();

        public FavoritesService()
        {
            LoadFavorites();
        }

        public bool IsFavorite(Article article)
        {
            var key = BuildArticleKey(article);
            return _favorites.ContainsKey(key);
        }

        public void Add(Article article)
        {
            var key = BuildArticleKey(article);
            if (_favorites.TryAdd(key, article))
            {
                SaveFavorites();
            }
        }

        public void Remove(Article article)
        {
            var key = BuildArticleKey(article);
            if (_favorites.TryRemove(key, out Article removed))
            {
                SaveFavorites();
            }
        }

        public IEnumerable<Article> Get()
        {
            // TODO: save the `add` date and sort by it
            return _favorites.Values.ToList();
        }

        private string BuildArticleKey(Article article)
        {
            if (string.IsNullOrWhiteSpace(article?.Url))
                return null;

            return article.Url;
        }

        private void SaveFavorites()
        {
            var state = JsonConvert.SerializeObject(_favorites.Values);
            Preferences.Set(GetType().FullName, state);
        }

        private void LoadFavorites()
        {
            var state = Preferences.Get(GetType().FullName, null);
            if (string.IsNullOrWhiteSpace(state))
                return;

            var favoritesState = JsonConvert.DeserializeObject<List<Article>>(state);
            if (!favoritesState.IsNullOrEmpty())
            {
                _favorites.Clear();
                favoritesState.ForEach(Add);
            }
        }
    }
}
