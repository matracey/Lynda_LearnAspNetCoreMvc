using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace ExploreCalifornia.Models
{
    public class Special
    {
        [Key]
        public long Id { get; set; }

        private string _key;

        [StringLength(100, MinimumLength = 5, ErrorMessage = "Key must be between 5 and 100 characters.")]
        [RegularExpression("^[-a-z0-9]+$", ErrorMessage = "Key must only contain lowercase or hyphen characters")]
        public string Key
        {
            get { return string.IsNullOrWhiteSpace(_key) ? (_key = Regex.Replace((Name ?? string.Empty).ToLower(), "[^a-z0-9]", "-")) : _key; }
            set { _key = value; }
        }

        [Required, StringLength(100, MinimumLength = 5, ErrorMessage = "Name must be between 5 and 100 characters.")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required, StringLength(100, MinimumLength = 5, ErrorMessage = "Type must be between 5 and 100 characters.")]
        [DataType(DataType.Text)]
        public string Type { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public int Price { get; set; }

        public DateTime Created { get; set; }

        public SpecialViewModel ToViewModel()
        {
            return new SpecialViewModel()
            {
                Id = Id,
                Key = Key,
                DeleteKey = Key,
                Name = Name,
                Type = Type,
                Price = Price,
                Image = new List<IFormFile>(),
                Created = Created
            };
        }
    }

    public class SpecialViewModel
    {
        [Key]
        public long Id { get; set; }
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Key must be between 5 and 100 characters.")]
        [RegularExpression("^[-a-z0-9]+$", ErrorMessage = "Key must only contain lowercase or hyphen characters")]
        public string Key { get; set; }
        public string DeleteKey { get; set; }
        [Required, StringLength(100, MinimumLength = 5, ErrorMessage = "Name must be between 5 and 100 characters.")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Required, StringLength(100, MinimumLength = 5, ErrorMessage = "Type must be between 5 and 100 characters.")]
        [DataType(DataType.Text)]
        public string Type { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public int Price { get; set; }
        public ICollection<IFormFile> Image { get; set; }
        public DateTime Created { get; set; }

        public Special ToSpecial()
        {
            return new Special()
            {
                Id = Id,
                Key = Key,
                Name = Name,
                Type = Type,
                Price = Price,
                Created = Created
            };
        }
    }
}
