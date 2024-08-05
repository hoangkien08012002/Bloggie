﻿using BloggieWeb.Models.Domain;

namespace BloggieWeb.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<BlogPost> Posts { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}
