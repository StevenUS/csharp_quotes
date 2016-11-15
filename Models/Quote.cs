using System;
using System.ComponentModel.DataAnnotations;

namespace quoteRedux.Models
{
    public class Quote : BaseEntity
    {
        public int id { get; set; }
        [Required]
        [MinLength(3)]
        public User user { get; set; }
        [Required]
        public string text { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}