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
using ThumbLedge.API;
using ThumbLedge.Model;
using ThumbLedge.ViewModel;

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

        private void backgroundVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (sender is MediaElement me)
            {
                me.Position = new TimeSpan(0, 0, 0, 0, 10);
            }
        }

        // Punt de l'últim clic del ratolí
        private Point _lastMouseDown;
        // Coneixement seleccionat
        TreeViewItem selectedKnowledge;

        // Esdeveniment quan es fa clic al ratolí a l'arbre
        private void TreeView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _lastMouseDown = e.GetPosition(tvParameters);
            }
        }

        // Esdeveniment quan es mou el ratolí a l'arbre
        private void TreeView_MouseMove(object sender, MouseEventArgs e)
        {
            // Coneixement seleccionat
            selectedKnowledge = PageModel.Instance.SelectedKnowledge;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentPosition = e.GetPosition(tvParameters);

                // Comprova si el moviment supera un llindar mínim
                if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 10.0) ||
                    (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 10.0))
                {
                    if (selectedKnowledge != null)
                    {
                        // Inicia el drag and drop
                        DragDrop.DoDragDrop(tvParameters, selectedKnowledge, DragDropEffects.Move);
                    }
                }
            }
        }

        // Esdeveniment quan es fa el drop a l'arbre
        private void TreeView_Drop(object sender, DragEventArgs e)
        {
            // Element on es fa el drop
            TreeViewItem TargetItem = GetNearestContainer(e.OriginalSource as UIElement);
            PageModel.Instance.DropKnowledge = TargetItem;

            if (TargetItem != null && selectedKnowledge != null)
            {
                if (TargetItem.Header != selectedKnowledge.Header)
                {
                    e.Effects = DragDropEffects.Move;

                    if (DataContext is DashboardVM viewModel && viewModel.DropCommand.CanExecute(null))
                    {
                        viewModel.DropCommand.Execute(null);
                    }
                }
            }
        }

        // Obté el contenidor més proper
        private TreeViewItem GetNearestContainer(UIElement element)
        {
            // Camina cap amunt per l'arbre d'elements fins al TreeViewItem més proper.
            TreeViewItem container = element as TreeViewItem;
            while ((container == null) && (element != null))
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as TreeViewItem;
            }
            return container;
        }

    }
}
