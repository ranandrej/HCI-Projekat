using NetworkService.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NetworkService.Model
{
    public class Entity:ValidationBase
    {
        private int id;
        public int ID
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("ID");
            }
        }


        private string naziv;
        public string Naziv
        {
            get { return naziv; }
            set
            {
                naziv = value;
                OnPropertyChanged("Naziv");
            }
        }
        private EntityType tip;
        public EntityType Tip
        {
            get { return tip; }
            set
            {
                tip = value;
                OnPropertyChanged("Tip");
            }
        }
        private double vrednost;
        public double Vrednost
        {
            get { return vrednost;}

            set
            {
                if(vrednost != value)
                {
                    vrednost = value;
                    OnPropertyChanged("Vrednost");
                    OnPropertyChanged("ReadingHistory");

                }

            }
        }
        private Visibility notValidValue = Visibility.Visible;
        public Visibility NotValidValue
        {
            get => notValidValue;
            set
            {
                if (value != notValidValue)
                {
                    notValidValue = value;
                    OnPropertyChanged("NotValidValue");
                }
            }
        }
        public Entity() { }
        public Entity(int iD, string naziv, EntityType tip)
        {
            ID = iD;
            Naziv = naziv;
            Tip = tip;
            Vrednost = 0;
        }
        public override string ToString()
        {
            return ID + "-" + Naziv;
        }
        public Entity CopyTo()
        {
            return new Entity(id, naziv, tip);
        }
        public void AddValue(int value)
        {
           
        }
        private bool ValidateType()
        {
            return Tip == null;
        }
        public bool ValidateValue()
        {
            return Vrednost >= 0.34 && Vrednost <= 2.73;
        }
        protected override void ValidateSelf()
        {
            if (ID < 1)
            {
                ValidationErrors["ID"] = "ID must be positive and greater than 0!";
            }
            else
            {
                ValidationErrors["ID"] = string.Empty;
            }
            if (string.IsNullOrWhiteSpace(Naziv))
            {
                ValidationErrors["Naziv"] = "Name is required!";
            }
            else
            {
                ValidationErrors["Naziv"] = string.Empty;
            }
            if (ValidateType())
            {
                ValidationErrors["Type"] = "Type is required!";
            }
            else
            {
                ValidationErrors["Type"] = string.Empty;
            }
        }
    }
}
