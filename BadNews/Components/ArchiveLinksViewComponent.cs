using System;
using BadNews.Repositories.News;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BadNews.Components
{
    public class ArchiveLinksViewComponent : ViewComponent
    {
        private readonly INewsRepository newsRepository;
        private readonly IMemoryCache memoryCache;

        public ArchiveLinksViewComponent(INewsRepository newsRepository, IMemoryCache memoryCache)
        {
            this.newsRepository = newsRepository;
            this.memoryCache = memoryCache;
        }

        public IViewComponentResult Invoke()
        {
            var cacheKey = nameof(ArchiveLinksViewComponent);
            if (!memoryCache.TryGetValue(cacheKey, out var years))
            {
                years = newsRepository.GetYearsWithArticles();
                if (years != null)
                    memoryCache.Set(cacheKey, years, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                    });
            }

            return View(years);
        }
    }
}