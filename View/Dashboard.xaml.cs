using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ThumbLedge.View
{
    /// <summary>
    /// Lógica de interacción para Dashboard.xaml
    /// </summary>
    public partial class Dashboard : UserControl
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        //Això s'ha de millorar
        private void backgroundVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (sender is MediaElement me)
            {
                me.Position = new TimeSpan(0, 0, 0, 0, 10);
            }
        }
    }
}
