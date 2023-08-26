using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Shapes;

namespace Sistem_za_upravljanje_sadržajima
{
    /// <summary>
    /// Interaction logic for Glavni.xaml
    /// </summary>
    
    public enum Radnja { dodavanje, izmena}
    
    public partial class Glavni : Window
    {
        private DataIO serializer= new DataIO();
        public static BindingList<Igra> igre { get; set; }
        public static BindingList<Igra> brisanje = new BindingList<Igra>();


        public static  Radnja radnja = Radnja.dodavanje;
        public Glavni()
        {
            igre = serializer.DeSerializeObject<BindingList<Igra>>("igra.xml");
            if(igre == null)
            {
                igre = new BindingList<Igra>();
            }

            InitializeComponent();

            foreach (Igra i in igre)
            {
                if (i.Naziv.Equals("") || i.Naziv.Equals(string.Empty))
                {
                    igre.Remove(i);
                }
            }

            dataGridIgre.ItemsSource = igre;

            this.DataContext = this;

            if (Korisnik.korisnik == Tip.admin)
            {
                Vrsta.Text = "Administrator";
                Brisanje.Visibility = Visibility.Visible;
                Dodavanje.Visibility = Visibility.Visible; 
                CHBOX.Visibility = Visibility.Visible;
            }
            else
            {
                Vrsta.Text = "Posetilac";
                Brisanje.Visibility = Visibility.Hidden;
                Dodavanje.Visibility = Visibility.Hidden;
                CHBOX.Visibility = Visibility.Hidden;
            }

            foreach (Igra i in igre)
            {
                if (i.Naziv.Equals("") || i.Naziv.Equals(string.Empty))
                {
                    igre.Remove(i);
                }
            }
            serializer.SerializeObject<BindingList<Igra>>(igre, "igra.xml");
        }

        private void Izlaz_Click(object sender, RoutedEventArgs e)
        {
            serializer.SerializeObject<BindingList<Igra>>(igre, "igra.xml");
            MainWindow mw = new MainWindow();

            this.Close();
            mw.Show();
        }

        private void Brisanje_Click(object sender, RoutedEventArgs e)
        {
            
            {
                IReadOnlyList<Igra> brisanje = igre.Where(p => (p.IsChecked == true)).ToList();
                foreach(Igra i in brisanje)
                {
                    igre.Remove(i);
                    File.Delete(i.Rtf);
                }
                dataGridIgre.Items.Refresh();
                serializer.SerializeObject(igre, "podaci.xml");
                serializer.SerializeObject<BindingList<Igra>>(igre, "igra.xml");
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            igre[dataGridIgre.SelectedIndex].IsChecked = true;
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            igre[dataGridIgre.SelectedIndex].IsChecked = true;
        }

        private void Dodavanje_Click(object sender, RoutedEventArgs e)
        {
            radnja = Radnja.dodavanje;
            Dodavanje d = new Dodavanje();
            d.ShowDialog();
            dataGridIgre.Items.Refresh();
            serializer.SerializeObject<BindingList<Igra>>(igre, "igra.xml");
            Glavni gg = new Glavni();
            Close();
            gg.Show();
        }

        private void Link_Event(object sender, RoutedEventArgs e)
        {
            radnja = Radnja.izmena;

            Igra i = igre[dataGridIgre.SelectedIndex];
            Dodavanje d = new Dodavanje();

            if (Korisnik.korisnik == Tip.posetilac)
            {
                d.NazivIgre.IsEnabled = false;
                d.GodinaIzdavanja.IsEnabled = false;
                d.MainToolbar.Visibility = Visibility.Hidden;
                d.rtbEditor.IsEnabled = false;
                d.BtnSlika.Visibility = Visibility.Hidden;
                d.BtnSacuvaj.Visibility = Visibility.Hidden;
                d.BtnOtkazi.Content = "OK";
                d.BtnOtkazi.Margin = new Thickness(0, 10, 10, 10);
                d.NazivIgre.FontSize = 17;
                d.GodinaIzdavanja.FontSize = 17;
                d.BtnOtkazi.Background = Brushes.LightBlue;
            }

            d.editMode = true;
            d.idx = dataGridIgre.SelectedIndex;

            d.NazivIgre.Text = i.Naziv;
            d.GodinaIzdavanja.Text = i.Godina.ToString();
            d.Slika.Source = new BitmapImage(new Uri(i.Slika));
            d.rtbUcitaj(i.Rtf);
            
            d.ShowDialog();
            dataGridIgre.Items.Refresh();
            serializer.SerializeObject<BindingList<Igra>>(igre, "igra.xml");
            Glavni gg = new Glavni();
            Close();
            gg.Show();
        }

    }
}
