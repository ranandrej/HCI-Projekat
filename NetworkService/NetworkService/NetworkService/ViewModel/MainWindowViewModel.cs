using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using NetworkService.Model;
using static NetworkService.ViewModel.GroupedEntitiesViewModel;

namespace NetworkService.ViewModel
{
    public class MainWindowViewModel:BindableBase
    {
        private int count = 15; // Inicijalna vrednost broja objekata u sistemu
                                // ######### ZAMENITI stvarnim brojem elemenata
                                //
                                //   zavisno od broja entitet
        private int id;
        private double value;
        private bool file = false;
        private string path;//           a u listi
        public MyICommand<string> NavCommand { get; private set; }
        public MyICommand UndoCommand { get; private set; }

        private TableViewModel tebleViewModel = new TableViewModel();
        private DragAndDropViewModel dragAndDropViewModel = new DragAndDropViewModel();
        private GraphViewModel graphViewModel = new GraphViewModel();

        private BindableBase currentViewModel;
        private BindableBase lastViewModel;

        public MyICommand CloseCommand { get; set; }
        public static ICommand UndoLastCommand { get; set; }


        private static ObservableCollection<Entity> entities = new ObservableCollection<Entity>();
        public ObservableCollection<GroupedEntitiesViewModel> GroupedEntitiesList { get; set; }

        public static ObservableCollection<Entity> Entities
        {
            get => entities;
            set
            {
                if (value != entities)
                {
                    entities = value;
                }
            }
        }
        public static List<EntityType> tipovi = new List<EntityType>(){
            new EntityType { NazivTipa = "IntervalMeter",SlikaTipa="C:/Users/Korisnik/Downloads/time-interval-meter2-small.png"  },
            new EntityType { NazivTipa = "SmartMeter",SlikaTipa="C:/Users/Korisnik/Downloads/smartmeter.png" }  };
        public static Dictionary<string, EntityType> tipoviDictionary = new Dictionary<string, EntityType>(
    tipovi.ToDictionary(entity => entity.NazivTipa));
        public static ObservableCollection<DragDropSlot> slots = new ObservableCollection<DragDropSlot>
            {
                new DragDropSlot(){Center= new Point(50 ,75)},
                new DragDropSlot(){Center= new Point(170,75)},
                new DragDropSlot(){Center= new Point(290,75)},
                new DragDropSlot(){Center= new Point(410,75)},
                new DragDropSlot(){Center= new Point(50 ,215)},
                new DragDropSlot(){Center= new Point(170,215)},
                new DragDropSlot(){Center= new Point(290,215)},
                new DragDropSlot(){Center= new Point(410,215)},
                new DragDropSlot(){Center= new Point(50 ,345)},
                new DragDropSlot(){Center= new Point(170,345)},
                new DragDropSlot(){Center= new Point(290,345)},
                new DragDropSlot(){Center= new Point(410,345)},
                new DragDropSlot(){Center= new Point(50 ,475)},
                new DragDropSlot(){Center= new Point(170,475)},
                new DragDropSlot(){Center= new Point(290,475)},
                new DragDropSlot(){Center= new Point(410,475)},
                new DragDropSlot(){Center= new Point(50 ,605)},
                new DragDropSlot(){Center= new Point(170,605)},
                new DragDropSlot(){Center= new Point(290,605)},
                new DragDropSlot(){Center= new Point(410,605)},
            };
        public static ObservableCollection<Line> lines = new ObservableCollection<Line>();
        public static ObservableCollection<Entity> treeViewEntities = new ObservableCollection<Entity>(Entities);
        public MainWindowViewModel()
        {
            path = Environment.CurrentDirectory + @"\Log.txt";
            NavCommand = new MyICommand<string>(OnNav);
            CurrentViewModel = tebleViewModel;//Povezivanje sa serverskom aplikacijom
            CloseCommand = new MyICommand(OnClose);
            UndoCommand = new MyICommand(Undo);

            GenerateData(5);
            treeViewEntities = new ObservableCollection<Entity>(entities);
            createListener();
           
        }
        private void GenerateData(int count)
        {
            Random r = new Random();
            for (int i = 0; i < count; i++)
            {
                entities.Add(new Entity { ID = i + 1, Naziv = $"Name {i + 1}", Tip = tipovi[r.Next(0, 2)] });
            }
        }
        private void Undo()
        {
            if (UndoLastCommand != null)
            {
                UndoLastCommand.Execute(null);
                UndoLastCommand = null;
            }
        }
        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set
            {
                SetProperty(ref currentViewModel, value);
            }
        }

        /// <summary>
        /// Koji se view prikazuje u glavnom prozoru
        /// </summary>
        /// <param name="destination"></param>

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "table":
                    lastViewModel = CurrentViewModel;
                    CurrentViewModel = tebleViewModel;
                    break;
                case "dragAndDrop":
                    lastViewModel = CurrentViewModel;

                    CurrentViewModel = dragAndDropViewModel;
                    break;
                case "graph":
                    lastViewModel = CurrentViewModel;
                    CurrentViewModel = graphViewModel;
                    break;
                
            }
            UndoLastCommand = new MyICommand(UndoNav);

        }
        private void UndoNav()
        {
            CurrentViewModel = lastViewModel;
        }
        private void OnClose()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to exit?", "Confirm closing application.",
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.MainWindow.Close();
            }
        }
        private void createListener()
        {
            var tcp = new TcpListener(IPAddress.Any, 25675);
            tcp.Start();

            var listeningThread = new Thread(() =>
            {
                while (true)
                {
                    var tcpClient = tcp.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(param =>
                    {
                        //Prijem poruke
                        NetworkStream stream = tcpClient.GetStream();
                        string incomming;
                        byte[] bytes = new byte[1024];
                        int i = stream.Read(bytes, 0, bytes.Length);
                        //Primljena poruka je sacuvana u incomming stringu
                        incomming = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                        //Ukoliko je primljena poruka pitanje koliko objekata ima u sistemu -> odgovor
                        if (incomming.Equals("Need object count"))
                        {
                            //Response
                            /* Umesto sto se ovde salje count.ToString(), potrebno je poslati 
                             * duzinu liste koja sadrzi sve objekte pod monitoringom, odnosno
                             * njihov ukupan broj (NE BROJATI OD NULE, VEC POSLATI UKUPAN BROJ)
                             * */
                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(Entities.Count.ToString());
                            stream.Write(data, 0, data.Length);
                        }
                        else
                        {
                            //U suprotnom, server je poslao promenu stanja nekog objekta u sistemu
                            Console.WriteLine(incomming); //Na primer: "Entitet_1:272"
                            string[] data = incomming.Trim().Split(':');
                            id = int.Parse(data[0].Split('_')[1].Trim());
                            value = Double.Parse(data[1].Trim());
                            Console.WriteLine($"ID:{id} Value:{value}");
                            if (id != -1 && id < entities.Count())
                            {
                                entities[id].Vrednost = value;
                                Upis();

                            }
                        }

                    
                    }, null);
                }
            });

            listeningThread.IsBackground = true;
            listeningThread.Start();
        }
        private void Upis()
        {
            if (!file)
            {
                StreamWriter writer;
                using (writer = new StreamWriter(path))
                {
                    DateTime dateTime = DateTime.Now;
                    string formattedString = $"{dateTime.ToString("M/d/yyyy h:mm:ss")} {Entities[id].Tip.NazivTipa}, {value}";

                    writer.WriteLine(formattedString);
                }
                file = true;
            }
            else
            {
                StreamWriter writer;
                using (writer = new StreamWriter(path, true))
                {
                    DateTime dateTime = DateTime.Now;
                    string formattedString = $"{dateTime.ToString("M/d/yyyy h:mm:ss")} {Entities[id].Tip.NazivTipa}, {value}";

                    writer.WriteLine(formattedString);


                }
            }
            file = true;
        }
    }
}
