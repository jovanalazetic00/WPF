using Klase;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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


namespace Projekat
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        string putanja = "";
        bool slika = false;
        int indeks;
        bool _noediting = true;
       // int brojRTB = -1;
        int brojacRijeci = 0;
        bool init = false;
        public AddWindow()
        {
            InitializeComponent();
            
            FontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            FontSize.ItemsSource = new List<double> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
            init = true;
        }

        public AddWindow(int indeks)
        {
            InitializeComponent();
            init = true;
            this.indeks = indeks;
            
            Dodaj.Content = "Izmijeni";

            FontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            FontSize.ItemsSource = new List<double> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };

            TekstUtakmica.Text = MainWindow.utakmicas[indeks].Utakmicaa;
            TekstBrBodova.Text = MainWindow.utakmicas[indeks].BrPoena.ToString();
            Datum.SelectedDate = MainWindow.utakmicas[indeks].Datum;
            Slika.Source = new BitmapImage(new Uri(MainWindow.utakmicas[indeks].PathSlika));


            string pathToRtb = "./rtb" + MainWindow.utakmicas[indeks].ID + ".rtf";

            TextRange range;
            FileStream fStream;
            range = new TextRange(RTB.Document.ContentStart, RTB.Document.ContentEnd);
            fStream = new FileStream(pathToRtb, FileMode.Open);
            range.Load(fStream, DataFormats.Rtf);
            fStream.Close();

           
            

            _noediting = false;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }


        private bool validate()
        {
            bool isValid = true;

            if (TekstUtakmica.Text.Trim().Equals(""))
            {
                isValid = false;
                TekstUtakmica.BorderBrush = Brushes.Silver;
                TekstUtakmica.BorderThickness = new Thickness(3);
                labelGreskaUtakmica.Content = "Unesite utakmicu";
            }
            else
            {
                TekstUtakmica.BorderBrush = Brushes.Black;
                labelGreskaUtakmica.Content = String.Empty;

            }

            if (TekstBrBodova.Text.Trim().Equals(""))
            {
                isValid = false;
                TekstBrBodova.BorderBrush = Brushes.Red;
                TekstBrBodova.BorderThickness = new Thickness(2);
                labelGreskaBrBodova.Content = "Unesite bodove";
            }

            else
            {
                labelGreskaBrBodova.Content = String.Empty;

                int i = 0;

                try
                {
                    i=Int32.Parse(TekstBrBodova.Text.Trim());
                }
                catch (Exception exc)
                {
                    TekstBrBodova.BorderBrush = Brushes.Red;
                    TekstBrBodova.BorderThickness = new Thickness(2);

                    labelGreskaBrBodova.Content = "Mora biti broj";
                    Console.WriteLine(exc.Message);
                    isValid = false;
                }

                if (i < 0)
                {
                    isValid = false;
                    TekstBrBodova.BorderBrush = Brushes.Red;
                    TekstBrBodova.BorderThickness = new Thickness(2);
                    labelGreskaBrBodova.Content = "Nije pozitivan broj";
                }
            }

            if (Datum.SelectedDate is null)
            {
                labelGreskaDatum.Content = "Unesite datum";
                isValid = false;
            }
            else
            { 
                labelGreskaDatum.Content = "";
            }


            if (Slika.Source == null)
            {

                labelGreskaSlika.Content = "Ubacite sliku";
                isValid = false;
            }
            else
                labelGreskaSlika.Content = "";


            if (brojacRijeci == 0)
            {
                labelGOpis.Content = "Opis je prazan";
                isValid = false;
            }
            else
            {


                labelGOpis.Content = "";
            }

            return isValid;

        }
        private void X_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Pretrazi_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Izaberite sliku.";
            fileDialog.Filter = "All suported grapichs|*.jpg;*.jpeg;*.png| " + "JPEG(*.jpg;*.jpeg;)|*.jpg ;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";
            
            if (fileDialog.ShowDialog() == true)
            {
                Slika.Source = new BitmapImage(new Uri(fileDialog.FileName));
                putanja = fileDialog.FileName;
                slika = true;
            }
        }

        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            if (validate())
            {
               
                if (_noediting)
                {
                    int id;
                    if (MainWindow.utakmicas.Count > 0)
                    {
                        id = MainWindow.utakmicas[MainWindow.utakmicas.Count - 1].ID + 1;
                    }
                    else
                    {
                        id = 0;
                    }

                    // int ordinaryNumberOfRTB = MainWindow.PhoneList.Count;
                    string pathToRtb = "./rtb" + id + ".rtf";
                    TextRange range;
                    FileStream fStream;
                    range = new TextRange(RTB.Document.ContentStart, RTB.Document.ContentEnd);
                    fStream = new FileStream(pathToRtb, FileMode.Create);
                    range.Save(fStream, DataFormats.Rtf);
                    fStream.Close();

                    MainWindow.utakmicas.Add(new Utakmica(TekstBrBodova.Text, (DateTime)Datum.SelectedDate, Convert.ToDouble(TekstBrBodova.Text), putanja, pathToRtb, id));
                }

               
                
                   
                else
                {
                    int id = MainWindow.utakmicas[indeks].ID;

                    string pathToRtb = "./rtb" + id + ".rtf";
                    TextRange range;
                    FileStream fStream;
                    range = new TextRange(RTB.Document.ContentStart, RTB.Document.ContentEnd);
                    fStream = new FileStream(pathToRtb, FileMode.Create);
                    range.Save(fStream, DataFormats.Rtf);
                    fStream.Close();

                    MainWindow.utakmicas[indeks].Utakmicaa = TekstUtakmica.Text;
                    MainWindow.utakmicas[indeks].BrPoena = Convert.ToDouble(TekstBrBodova.Text);
                    MainWindow.utakmicas[indeks].Datum = (DateTime)Datum.SelectedDate;

                    if (slika)
                    {
                        MainWindow.utakmicas[indeks].PathSlika = putanja;
                    }
                }
                this.Close();
            }

          

        }




        private void RTB_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = RTB.Selection.GetPropertyValue(Inline.FontWeightProperty);
            BOLD.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));

            temp = RTB.Selection.GetPropertyValue(Inline.FontStyleProperty);
            ITALIC.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));

            temp = RTB.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            UNDERLINE.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = RTB.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            FontFamily.SelectedItem = temp;

            temp = RTB.Selection.GetPropertyValue(Inline.FontSizeProperty);
            FontSize.SelectedItem = temp;
        }

        private void FontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
                if (FontFamily.SelectedItem != null && !RTB.Selection.IsEmpty)
                {
                    RTB.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, FontFamily.SelectedItem);
                }
        }

        private void FontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontSize.SelectedItem != null && !RTB.Selection.IsEmpty)
            {
                RTB.Selection.ApplyPropertyValue(Inline.FontSizeProperty, FontSize.SelectedItem);
            }
        }

       


        private void RTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextRange textRange = new TextRange(RTB.Document.ContentStart, RTB.Document.ContentEnd);
            if (init)
            {
                brojacRijeci = Regex.Matches(textRange.Text, @"[\S]+").Count;
                Broj_rijeci.Text = "Brojac: " + brojacRijeci.ToString();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();

            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!RTB.Selection.IsEmpty)
                {
                     RTB.Selection.ApplyPropertyValue(Inline.ForegroundProperty, new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B)));
                }
            }
        }
    }
}
