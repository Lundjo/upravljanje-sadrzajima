using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistem_za_upravljanje_sadržajima
{
    public enum Tip { admin, posetilac, nista}

    public class Korisnik
    {
        public static Tip korisnik = Tip.nista;

    }
}
