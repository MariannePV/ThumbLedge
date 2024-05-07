using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThumbLedge.Model
{
    public class PageModel
    {
        //Start page
        public Uri BackgroundURL { get; set; }

        //Dashboard
        public Uri BackgroundDashURL { get; set; }

        //General
        public object CurrentView { get; set; }
        public object MainView { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool LoggedIn { get; set; }
    }
}
