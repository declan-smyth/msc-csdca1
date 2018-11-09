using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BPCalculator.Pages
{
    public class AboutModel : PageModel
    {
        public string Module { get; set; }
        public string Assignment { get; set; }

        public string Author { get; set; }

        public void OnGet()
        {
            Module = "MSc. in Devops - Continuous Software Delivery";
            Assignment = "Continuous Assessment 1: Design and Develop a Continuious Delivery Pipeline";
            Author = "Declan Smyth";
        }
    }
}
