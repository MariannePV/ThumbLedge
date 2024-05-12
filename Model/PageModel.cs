using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ThumbLedge.Entities;

namespace ThumbLedge.Model
{
    public class PageModel
    {
        //Instància singelton
        private static PageModel instance;

        private PageModel() { }

        public static PageModel Instance
        {
            get
            {
                //Si instance és null, creem una nova instància
                if (instance == null)
                {
                    instance = new PageModel();
                }

                return instance;
            }
        }

        //PROPIETATS
        //Start page
        public Uri BackgroundURL { get; set; }

        //Dashboard
        public Uri BackgroundDashURL { get; set; }
        public string ImageSource { get; set; }

        //Content controls
        public object CurrentView { get; set; }
        public object MainView { get; set; }
        public object ThumbLedgeView { get; set; }

        //User data
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        //Knowledges
        public string KnowledgeName { get; set; }
        public TreeViewItem SelectedKnowledge { get; set; }
        public TreeViewItem DropKnowledge { get; set; }

        //Intelligences
        public string IntelligenceName { get; set; }
    }
}
