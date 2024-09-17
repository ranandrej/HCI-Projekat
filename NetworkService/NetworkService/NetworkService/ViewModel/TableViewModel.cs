using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using NetworkService.Model;

namespace NetworkService.ViewModel
{
    public class TableViewModel : BindableBase
    {
        private ObservableCollection<Entity> entities;
        public MyICommand AddComand { get; private set; }
        public MyICommand DeleteCommand { get; private set; }


        public ObservableCollection<Entity> Entities
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Search))
                {
                    return entities;
                }
                else
                {
                    return new ObservableCollection<Entity>(entities.Where(r => FindAll(r)));
                }
            }
            set
            {
                if (value != entities)
                {
                    entities = value;
                    OnPropertyChanged("Entities");
                }
            }
        }
        public List<EntityType> types
        {
            get => MainWindowViewModel.tipovi;
        }
        private Entity current;

        public Entity Current
        {
            get => current;
            set
            {
                if (value != current)
                {
                    current = value;
                    OnPropertyChanged("Current");
                }
            }
        }
        public TableViewModel()
        {
            AddComand = new MyICommand(OnAdd);
            DeleteCommand = new MyICommand(OnDelete);

            Current = new Entity();

            entities = MainWindowViewModel.Entities;
            

        }
        private Entity Added = new Entity();

        private string duplicateID = string.Empty;
        public string DuplicateID
        {
            get => duplicateID;
            set
            {
                if (value != duplicateID)
                {
                    duplicateID = value;
                    OnPropertyChanged("DuplicateID");
                }
            }
        }

        private void OnAdd()
        {
            Current.Validate();
            if (entities.Where(x => x.ID == Current.ID).Any())
            {
                DuplicateID = "ID already exists!";
                OnPropertyChanged("DuplicateID");
                return;
            }
            if (!Current.IsValid)
            {
                return;
            }
            Added = Current.CopyTo();
            entities.Add(Current);
            MainWindowViewModel.treeViewEntities.Add(Current);

            Current = new Entity();
            MainWindowViewModel.UndoLastCommand = new MyICommand(UndoAdd);
            MessageBox.Show("Usoesno dodat entitet!");
            OnPropertyChanged("Entities");

        }   
        private void UndoAdd()
        {
            MainWindowViewModel.Entities.Remove(Added);
            MainWindowViewModel.treeViewEntities.Remove(Added);

        }
        private int selectedIndex = -1;
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                if (selectedIndex != value)
                {
                    selectedIndex = value;
                    OnPropertyChanged("SelectedIndex");
                    OnPropertyChanged("CanDelete");
                    OnPropertyChanged("Current");
                }
            }
        }
        public bool CanDelete
        {
            get => selectedIndex != -1;
        }
        private void OnDelete()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Added = Current.CopyTo();
                entities.Remove(Current);
                Current = new Entity();
                MainWindowViewModel.UndoLastCommand = new MyICommand(UndoDelete);
                MainWindowViewModel.treeViewEntities.Remove(Current);

                OnPropertyChanged("Entities");
            }
        }
        private void UndoDelete()
        {
            entities.Add(Added);
            MainWindowViewModel.treeViewEntities.Add(Added);

        }
        public bool isName { get; set; } = true;
        private string search = string.Empty;
        public string Search
        {
            get => search;
            set
            {
                if (value != search)
                {
                    search = value;
                    OnPropertyChanged("Entities");
                    OnPropertyChanged("Search");
                }
            }
        }
        private bool FindAll(Entity reactor)
        {
            Regex regex = new Regex(search);
            return isName ? regex.IsMatch(reactor.Naziv) : regex.IsMatch(reactor.Tip.NazivTipa);
        }

        

    }
}
