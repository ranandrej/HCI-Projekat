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
using NetworkService.Model;
namespace NetworkService.Views
{
    /// <summary>
    /// Interaction logic for DragAndDropView.xaml
    /// </summary>
    public partial class DragAndDropView : UserControl
    {
        private bool isDragging;
        private Point startPoint;
        private Entity draggedItem;
        public DragAndDropView()
        {
            InitializeComponent();
        }
       private void TreeView_PreviewMouseLeftButtonDown(object sender,MouseButtonEventArgs e)
        {
            if(e.OriginalSource is FrameworkElement element && element.DataContext is Entity entity)
            {
                draggedItem = entity;
                DragDrop.DoDragDrop(element, entity, DragDropEffects.Move);
            }
            
        }
        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            Canvas canvas = (Canvas)sender;

            if (canvas.Resources["taken"] != null)
            {
                e.Effects = DragDropEffects.None;
            }
            else
            {
                e.Effects = DragDropEffects.Move;
            }
            e.Handled = true;
        }
        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            Canvas canvas = (Canvas)sender;
            if(draggedItem != null)
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(draggedItem.Tip.SlikaTipa, UriKind.Relative);
                image.EndInit();
                canvas.Background = new ImageBrush(image);
                canvas.Resources.Add("taken", true);

            }
            e.Handled = true;



        }
    }
}
    


