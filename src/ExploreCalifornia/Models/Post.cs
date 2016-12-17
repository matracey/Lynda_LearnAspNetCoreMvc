using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExploreCalifornia.Models
{
    public class Post
    {
        [Display(Name="Post Title")]
        [Required(ErrorMessage = "A title is required."), StringLength(100, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 100 characters long.")]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        public string Author { get; set; }

        [Required(ErrorMessage = "A post body is required."), MinLength(100, ErrorMessage = "Title must be at least 100 characters long.")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        public DateTime Posted { get; set; }
    }
}
