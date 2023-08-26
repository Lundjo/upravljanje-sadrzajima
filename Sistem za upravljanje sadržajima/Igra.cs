using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sistem_za_upravljanje_sadržajima
{
    public class Igra
    {
        private int godina;
        private string naziv;
        private string slika;
        private string rtf;
        private DateTime dodavanje;
        private bool isChecked;

        public Igra()
        {
        }

        public Igra(int godina = 2000, string naziv = "", string slika = "", string rtf = "")
        {
            this.Godina = godina;
            this.Naziv = naziv;
            this.Slika = slika;
            this.Rtf = rtf;
            this.Dodavanje = DateTime.Now;
            this.isChecked = false;
        }
        public int Godina { get => godina; set => godina = value; }
        public string Naziv { get => naziv; set => naziv = value; }
        public string Slika { get => slika; set => slika = value; }
        public string Rtf { get => rtf; set => rtf = value; }
        public DateTime Dodavanje { get => dodavanje; set => dodavanje = value; }
        public bool IsChecked { get => isChecked; set => isChecked = value; }
    }
}
