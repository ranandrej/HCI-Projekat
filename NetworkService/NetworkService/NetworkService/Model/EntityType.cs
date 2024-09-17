using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
    public class EntityType:BindableBase
    {
        private string nazivTipa;

        public string NazivTipa
        {
            get { return nazivTipa; }
            set
            {
                nazivTipa = value;
                OnPropertyChanged("NazivTipa");
            }
        }
        private string slikaTipa;
        public string SlikaTipa
        {
            get { return slikaTipa; }
            set
            {
                slikaTipa = value;
                OnPropertyChanged("SlikaTipa");
            }
        }
        public EntityType()
        {
            nazivTipa = string.Empty;
            slikaTipa = string.Empty;
        }
        public EntityType(string naziv,string slika)
        {
            NazivTipa = naziv;
            SlikaTipa = slika;
        }
        public override string ToString()
        {
            return NazivTipa;
        }
    }
}
