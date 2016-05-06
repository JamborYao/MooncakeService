using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooncakeTool.Models
{
    public class CardModel
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public List<string> Platforms { get; set; }

        public string Link { get; set; }
        public List<string> Products { get; set; }
        public string Author { get; set; }
    }
}