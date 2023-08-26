using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Dodavanje.xaml
    /// </summary>
    public partial class Dodavanje : Window
    {
        private DataIO serializer = new DataIO();
        public bool editMode = false;
        public int idx = -1;
        string slika = "";

        public Dodavanje()
        {
            InitializeComponent();

            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);

            Type t = typeof(Colors);

            cmbBoje.ItemsSource = t.GetProperties();
            cmbBoje.SelectedItem = t.GetProperty("Black");

            for (int i = 2; i < 64; i += 2)
            {
                cmbVelicina.Items.Add(i);
            }
            cmbVelicina.SelectedIndex = 5;
        }

        private void rtbSacuvaj(string putanja)
        {
            TextRange tr = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            FileStream fs = new FileStream(putanja, FileMode.Create);
            tr.Save(fs, DataFormats.Rtf);
            fs.Close();
        }
        
        public void rtbUcitaj(string putanja)
        {
            if (File.Exists(putanja))
            {
                FileStream fs = new FileStream(putanja, FileMode.Open);
                TextRange tr = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                tr.Load(fs, DataFormats.Rtf);
                fs.Close();
            }
            
        }

        private void BtnSlika_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog otvaranjeFajla = new OpenFileDialog();
            otvaranjeFajla.Filter = "Slike (*.png;*.jpeg;*.bmp;*.jpg;)|*.png;*.jpeg;*.bmp;*.jpg;";
            if (otvaranjeFajla.ShowDialog() == true)
            {
                Slika.Source = new BitmapImage(new Uri(otvaranjeFajla.FileName));
                slika = otvaranjeFajla.FileName;
            }
            else
            {
                slika = "";
            }
        }

        private void BtnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            Glavni g = new Glavni();
            bool postoji = false;
            int res;

                if (NazivIgre.Text == "" || GodinaIzdavanja.Text == "")
                {
                    MessageBox.Show("Nisu popunjena sva polja");
                }
                else if(int.TryParse(GodinaIzdavanja.Text, out res))
            {
                if (int.Parse(GodinaIzdavanja.Text) < 1953 || int.Parse(GodinaIzdavanja.Text) > 2026)
                {
                    MessageBox.Show("Igra mora biti izmedju godina 1953 i 2026");
                }
                else if (slika.Equals(""))
                {
                    MessageBox.Show("Nije odabrana slika");
                }
                else
                {
                    foreach (Igra i in Glavni.igre)
                    {
                        if (i.Godina == int.Parse(GodinaIzdavanja.Text) && i.Naziv.Equals(NazivIgre.Text))
                        {
                            MessageBox.Show("Igra već postoji u listi");
                            postoji = true;
                        }
                    }

                    if (!postoji && !editMode)
                    {
                        Glavni.igre.Add(new Igra(int.Parse(GodinaIzdavanja.Text), NazivIgre.Text, Slika.Source.ToString(), NazivIgre.Text + ".rtf"));
                        serializer.SerializeObject<BindingList<Igra>>(Glavni.igre, "igra.xml");
                        string putanja = Directory.GetCurrentDirectory() + "/" + NazivIgre.Text + ".rtf";
                        rtbSacuvaj(putanja);
                        this.Close();
                        g.dataGridIgre.Items.Refresh();
                    }
                    else if (!postoji && editMode)
                    {
                        var izmena = Glavni.igre[idx];
                        File.Delete(Directory.GetCurrentDirectory() + "/" + izmena.Naziv + ".rtf");
                        izmena.Naziv = NazivIgre.Text;
                        izmena.Godina = int.Parse(GodinaIzdavanja.Text);
                        izmena.Slika = Slika.Source.ToString();
                        string putanja = Directory.GetCurrentDirectory() + "/" + NazivIgre.Text + ".rtf";
                        rtbSacuvaj(putanja);

                        this.Close();
                        g.dataGridIgre.Items.Refresh();
                    }
                }

            }
            else
            {
                MessageBox.Show("Mora se uneti broj");
            }
            
         
            
        }

        private void BtnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp1 = rtbEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
            object temp2 = rtbEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
            object temp3 = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnBold.IsChecked = (temp1 != DependencyProperty.UnsetValue) && (temp1.Equals(FontWeights.Bold));
            btnItalic.IsChecked = (temp2 != DependencyProperty.UnsetValue) && (temp2.Equals(FontStyles.Italic));
            btnUnderline.IsChecked = (temp3 != DependencyProperty.UnsetValue) && (temp3.Equals(TextDecorations.Underline));

            temp1 = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp1;
            temp2 = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp2;
            temp3 = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            cmbFontFamily.SelectedItem = temp3;
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null && !rtbEditor.Selection.IsEmpty)
            {
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
            }
        }

        private void cmbVelicina_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmbVelicina.SelectedItem != null && !rtbEditor.Selection.IsEmpty)
            {
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, Convert.ToDouble(cmbVelicina.SelectedItem));
            }
        }

        private void cmbBoje_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RichTextBox rtb = rtbEditor;

            if (rtb != null && rtb.Selection is TextSelection)
            {
                try
                {
                    var trenutna_boja = cmbBoje.SelectedValue;
                    SolidColorBrush new_color = new SolidColorBrush((Color)((PropertyInfo)trenutna_boja).GetValue(null, null));
                    rtb.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, new_color);

                    rtb.Focus();
                }
                catch { }
            }
        }

        private void rtbEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextRange docContent = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);

            brrc.Content = "  Broj reči: " + (Regex.Matches(docContent.Text, @"[\S]+").Count).ToString();
        }
    }
}
