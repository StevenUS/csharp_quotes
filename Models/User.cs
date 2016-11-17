using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace quoteRedux.Models
{
    //adding comment for no reason
    //create a class for all models to extend from
    public abstract class BaseEntity{}
    public class User : BaseEntity
    {
        //function for one to many quotes
        public User()
        {
            quotes = new List<Quote>();
        }
        public int id { get; set; }
        [Required]
        [MinLength(3)]
        public string first_name { get; set; }
        [Required]
        [MinLength(3)]
        public string last_name { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        [MinLength(8)]
        [Compare("passwordConf", ErrorMessage="password and confimation do not match")]
        public string password { get; set; }
        public string passwordConf {get; set; }

        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        //one user has many quotes, collection declared below
        public ICollection<Quote> quotes {get; set;}
    }
}