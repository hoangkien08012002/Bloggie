﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace BloggieWeb.Models.ViewModels
{
    public class AddBlogPostRequest
    {
        public string? Heading { get; set; }

        public string? PageTitle { get; set; }

        public string? Content { get; set; }

        public string? ShortDescription { get; set; }

        public string? FearturedImageUrl { get; set; }

        public string? UrlHandle { get; set; }

        public DateTime PublishedDate { get; set; }

        public string? Author { get; set; }

        public bool Visible { get; set; }

        // Displaytag
        public IEnumerable<SelectListItem> Tags { get; set; }

        //Collect tag
        public string[] SelectTags { get; set; } = Array.Empty<string>();
    }
}
