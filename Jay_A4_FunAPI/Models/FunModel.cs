using System;
using System.ComponentModel.DataAnnotations;

namespace Jay_A4_FunAPI.Models
{
    public class FunModel
    {
        [Key]
        public int FunId { get; set; }
        public DateTime SaveDate { get; set; }
        public string GifImageLink { get; set; }
        public string Joke { get; set; }
    }
}
