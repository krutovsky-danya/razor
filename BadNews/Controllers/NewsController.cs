using System;
using BadNews.ModelBuilders.News;
using Microsoft.AspNetCore.Mvc;

namespace BadNews.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsModelBuilder newsModelBuilder;

        public NewsController(INewsModelBuilder newsModelBuilder)
        {
            this.newsModelBuilder = newsModelBuilder;
        }

        public IActionResult Index([FromQuery] int pageIndex = 0)
        {
            var model = newsModelBuilder.BuildIndexModel(pageIndex, false, null);
            return View(model);
        }

        // [HttpGet("/news/fullarticle/{id}")]
        public IActionResult FullArticle([FromRoute] Guid id)
        {
            var model = newsModelBuilder.BuildFullArticleModel(id);
            if (model == null)
                return NotFound();
            return View(model);
        }
    }
}