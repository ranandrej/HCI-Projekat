using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NetworkService.Model;
namespace NetworkService.ViewModel
{
    public class DragAndDropViewModel : BindableBase
    {
        public static ObservableCollection<GroupedEntitiesViewModel> GroupedEntitiesList { get; set; }
        private Dictionary<Entity, Canvas> _entityCanvasMap;

        public MyICommand<object> StartDragCommand { get; set; }
        public MyICommand<object> DropCommand { get; set; }
        public MyICommand<object> DeleteCommand { get; set; }
        public MyICommand<DragEventArgs> DragOverCommand { get; set; }
        private Point _dragStartPoint;
        private Entity _draggedEntity;
        private Canvas lastUsedCanvas = null;


        public ObservableCollection<DragDropSlot> Slots
        {
            get => MainWindowViewModel.slots;
        }
        

        public ObservableCollection<Line> Lines
        {
            get => MainWindowViewModel.lines;
        }
        private bool isDraggingTree = false;
        public bool IsDraggingTree
        {
            get => isDraggingTree;
            set
            {
                if (value != isDraggingTree)
                {
                    isDraggingTree = value;
                    OnPropertyChanged("IsDraggingTree");
                }
            }
        }
        private bool isDrawing = false;
        private Entity draggedItem = null;
        public Entity DraggedItem
        {
            get => draggedItem;
            set
            {
                if (value != draggedItem)
                {
                    draggedItem = value;
                    OnPropertyChanged("DraggedItem");
                }
            }
        }
        public DragAndDropViewModel()
        {
            StartDragCommand = new MyICommand<object>(OnStartDrag);
            DropCommand = new MyICommand<object>(OnDrop);
            DeleteCommand = new MyICommand<object>(OnDelete);
            DragOverCommand = new MyICommand<DragEventArgs>(OnDragOver);

            var groupedEntities = MainWindowViewModel.treeViewEntities
             .GroupBy(e => e.Tip)
             .Select(g => new GroupedEntitiesViewModel
             {
                 Type = g.Key,
                 Entities = new ObservableCollection<Entity>(g.ToList())
             })
             .ToList();

            GroupedEntitiesList = new ObservableCollection<GroupedEntitiesViewModel>(groupedEntities);
            _entityCanvasMap = new Dictionary<Entity, Canvas>();

        }

        private void OnStartDrag(object selectedItem)
        {
            if(selectedItem is TreeView treeView)
            {



                if (treeView.SelectedItem is Entity entity)
                {

                    var item = (Entity)treeView.SelectedItem;
                    if (item != null)
                    {
                        draggedItem = item;
                        DragDrop.DoDragDrop(treeView, item, DragDropEffects.Move);
                    }
                }
                
            }
        }
        private void OnDragOver(DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                e.Effects = DragDropEffects.Move;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }

            e.Handled = true;
        }
        private void OnDrop(object selectedItem)
        {
            Canvas canvas = (Canvas)selectedItem;
            TextBlock entityIdTextBlock = (TextBlock)canvas.Children[0];
            if (draggedItem != null)
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(draggedItem.Tip.SlikaTipa, UriKind.Relative);
                image.EndInit();
                canvas.Background = new ImageBrush(image);
                canvas.Resources.Add("taken", true);
                entityIdTextBlock.Text = draggedItem.ID.ToString();
                (canvas).Children[0] = entityIdTextBlock;
                
                    TextBlock textBox = canvas.Children[1] as TextBlock;
                    if (textBox != null)
                    {
                    if (draggedItem.ValidateValue())
                    {
                        textBox.Text = draggedItem.Vrednost.ToString();

                    }
                    else
                    {
                        textBox.Text = "Not valid!";
                    }
                }
                

                RemoveEntityFromGroup(draggedItem);
                _entityCanvasMap[draggedItem] = canvas;

                OnPropertyChanged("GroupedEntitiesList");
                MainWindowViewModel.UndoLastCommand = new MyICommand(UndoDrop);
               
            }
            lastUsedCanvas = canvas;
        }
        private void RemoveEntityFromGroup(Entity entity)
        {
            foreach (var group in GroupedEntitiesList)
            {
                var entityToRemove = group.Entities.FirstOrDefault(e => e.ID == entity.ID);
                if (entityToRemove != null)
                {
                    group.Entities.Remove(entityToRemove);
                    break;
                }
            }
        }
    private void OnDelete(object sender)
        {
            Canvas canvas = (Canvas)sender;
            TextBlock entityIdTextBlock = (TextBlock)canvas.Children[0];
            string entityId = entityIdTextBlock.Text;
            foreach (Entity entity in _entityCanvasMap.Keys)
            {
                if(entity.ID.ToString()== entityId)
                {
                    var group = GroupedEntitiesList.FirstOrDefault(g => g.Type == entity.Tip);
                    if(group != null)
                    {
                        group.Entities.Add(entity);
                        canvas.Background = new SolidColorBrush(Colors.Gray);
                        TextBlock textBlock = canvas.Children[0] as TextBlock;
                        if (textBlock != null)
                        {
                            textBlock.Text = "X";
                        }
                        TextBlock textBlockValue = canvas.Children[1] as TextBlock;
                        if (textBlockValue != null)
                        {
                            textBlockValue.Text = "0";
                        }
                        canvas.Resources.Remove("taken");
                    }

                }
            }
        }
    private void UndoDrop()
        {
            if (draggedItem != null && _entityCanvasMap.ContainsKey(draggedItem))
            {
                var canvas = _entityCanvasMap[draggedItem];
                var group = GroupedEntitiesList.FirstOrDefault(g => g.Type == draggedItem.Tip);
                if (group != null)
                {
                    group.Entities.Add(draggedItem);
                    draggedItem = null;
                    canvas.Background = new SolidColorBrush(Colors.Gray);

                    TextBlock textBlock = canvas.Children[0] as TextBlock;
                    if (textBlock != null)
                    {
                        textBlock.Text = "X";
                    }
                    TextBlock textBlockValue = canvas.Children[1] as TextBlock;
                    if (textBlockValue != null)
                    {
                        textBlockValue.Text = "0";
                    }

                    // Osvežavanje stanja Undo komande

                }
            }
            draggedItem = new Entity();
            lastUsedCanvas.Resources.Remove("taken");
        }
        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null)
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }
    }

}

