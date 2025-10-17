using System;

namespace Bluetech.Models
{
    public class Project
    {
        public string Title { get; set; } = string.Empty;
        public string Customer { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public DateTime LaunchDate { get; set; }
        public string? Link { get; set; }
        public string? ImagePath { get; set; }
        public string[] Tags { get; set; } = Array.Empty<string>();
    }
}
