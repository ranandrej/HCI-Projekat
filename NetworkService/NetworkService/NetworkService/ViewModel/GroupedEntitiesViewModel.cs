using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.ViewModel
{
    
        public class GroupedEntitiesViewModel
        {
            public EntityType Type { get; set; }
            public ObservableCollection<Entity> Entities { get; set; }

            public GroupedEntitiesViewModel()
            {
                Entities = new ObservableCollection<Entity>();
            }
       
    }
}
    

