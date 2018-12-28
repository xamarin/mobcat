using System;
using System.Text;
using Microsoft.MobCAT.MVVM;
using NewsAPI.Models;

namespace News.ViewModels
{
    /// <summary>
    /// Article view model.
    /// </summary>
    public class ArticleViewModel : BaseNavigationViewModel
    {
        public Article Article { get; }

        public string Title => Article.Title;

        public string UrlToImage => Article.UrlToImage;

        public string UrlToArticle => Article.Url;

        public string PublishedAgo
        {
            get
            {
                if (Article.PublishedAt == null)
                    return null;
                    
                var publishedHoursAgo = Math.Round((DateTime.UtcNow - Article.PublishedAt.Value).TotalHours);
                if (publishedHoursAgo <= 1)
                    return "now";

                if (publishedHoursAgo < 24)
                    return $"{publishedHoursAgo}h";

                return $"{publishedHoursAgo / 24:0}d";
            }
        }

        public string Footer
        {
            get
            {
                // Author includes source and author
                var footerBuilder = new StringBuilder(Article.Author);
                if (!string.IsNullOrWhiteSpace(Article.Source?.Name))
                {
                    if (footerBuilder.Length > 0)
                        footerBuilder.Append(" | ");

                    footerBuilder.Append(Article.Source.Name);
                }

                var when = PublishedAgo;
                if (!string.IsNullOrWhiteSpace(when))
                {
                    if (footerBuilder.Length > 0)
                        footerBuilder.Append(" | ");

                    footerBuilder.Append(when);
                }


                var result = footerBuilder.ToString();
                return result;
            }
        }

        // TODO: load and save from preferences
        private bool _isFavorite;
        public bool IsFavorite
        {
            get { return _isFavorite; }
            set { RaiseAndUpdate(ref _isFavorite, value); }
        }

        public Command SwitchFavoriteArticleCommand { get; }

        public ArticleViewModel(Article article)
        {
            if (article == null)
                throw new ArgumentNullException(nameof(article));

            Article = article;
            SwitchFavoriteArticleCommand = new Command(OnSwitchFavoriteArticleCommandExecuted);
        }


        private void OnSwitchFavoriteArticleCommandExecuted()
        {
            IsFavorite = !IsFavorite;
        }
    }
}