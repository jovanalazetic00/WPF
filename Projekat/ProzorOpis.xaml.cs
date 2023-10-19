using System;
using System.Collections.Generic;
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

namespace Projekat
{
    /// <summary>
    /// Interaction logic for ProzorOpis.xaml
    /// </summary>
    public partial class ProzorOpis : Window
    {

        public ProzorOpis(int indeks)
        {
            InitializeComponent();
            int id;
            id = MainWindow.utakmicas[indeks].ID;

            string path = "rtb" + id + ".rtf";
            TextRange range;
            FileStream fStream;

            

            range = new TextRange(Utakmica.Document.ContentStart, Utakmica.Document.ContentEnd);
            fStream = new FileStream(path, FileMode.Open);
            range.Load(fStream, DataFormats.Rtf);
            fStream.Close();
            Opis.Content = MainWindow.utakmicas[indeks].Utakmicaa;
           
            Slika.Source = new BitmapImage(new Uri(MainWindow.utakmicas[indeks].PathSlika));


        }



        private void Button_Click_izadji(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
