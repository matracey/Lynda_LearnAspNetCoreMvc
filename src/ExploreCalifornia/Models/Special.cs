using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExploreCalifornia.Models
{
    public class Special
    {
        public long Id { get; set; }

        private string _key;

        [StringLength(100, MinimumLength = 5, ErrorMessage = "Key must be between 5 and 100 characters.")]
        [RegularExpression("[-a-z0-9]", ErrorMessage = "Key must only contain lowercase or hyphen characters")]
        public string Key
        {
            get { return string.IsNullOrWhiteSpace(_key) ? (_key = Regex.Replace((Name ?? string.Empty).ToLower(), "[^a-z0-9]", "-")) : _key; }
            set { _key = value; }
        }

        [Required, StringLength(100, MinimumLength = 5, ErrorMessage = "Name must be between 5 and 100 characters.")]
        public string Name { get; internal set; }

        [Required, StringLength(100, MinimumLength = 5, ErrorMessage = "Type must be between 5 and 100 characters.")]
        public string Type { get; internal set; }

        [Required]
        public int Price { get; internal set; }

        public DateTime Created { get; set; }
    }
}
