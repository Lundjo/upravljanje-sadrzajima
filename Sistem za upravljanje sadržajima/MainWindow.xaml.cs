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

namespace Sistem_za_upravljanje_sadržajima
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void IzlazDugme_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Logovanje_Click(object sender, RoutedEventArgs e)
        {
            if(KorisnickoIme.Text.Equals("admin") && Sifra.Password.Equals("admin"))
            {
                Korisnik.korisnik = Tip.admin;
                Glavni g = new Glavni();
                this.Close();
                g.Show();
            }
            else if(KorisnickoIme.Text.Equals("posetilac") && Sifra.Password.Equals("posetilac"))
            {
                Korisnik.korisnik = Tip.posetilac;
                Glavni g = new Glavni();
                this.Close();
                g.Show();
            }
            else if (KorisnickoIme.Text.Equals("admin") || KorisnickoIme.Text.Equals("posetilac"))
            {
                GreskaLogovanje gl = new GreskaLogovanje();
                gl.Greska.Text = "Greška: Pogrešna šifra";
                gl.ShowDialog();
            }
            else
            {
                GreskaLogovanje gl = new GreskaLogovanje();
                gl.Greska.Text = "Greška: Nepostojeći korisnik";
                gl.ShowDialog();
            }
        }
    }
}
