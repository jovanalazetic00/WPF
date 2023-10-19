using Classes;
using Klase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Projekat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static BindingList<Utakmica> utakmicas { get; set; }

        private DataIO serijalizacija = new DataIO();
        public MainWindow()
        {
            utakmicas = serijalizacija.DeSerializeObject<BindingList<Utakmica>>("utakmice.xml");
            
            if(utakmicas is null)   //provjeravam da li je prazna lista
            {
                utakmicas = new BindingList<Utakmica>();

            }
            
            DataContext = this;
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Procitaj_Click(object sender, RoutedEventArgs e)
        {
            Window prozorOpis = new ProzorOpis(DataGrid.SelectedIndex);

            prozorOpis.ShowDialog();
        }

        private void Izmjeni_Click(object sender, RoutedEventArgs e)
        {
            if (utakmicas.Count > 0)
            {
                Window izmijeni = new AddWindow(DataGrid.SelectedIndex);

                izmijeni.ShowDialog();
            }
            DataGrid.Items.Refresh();
        }

        private void Obrisi_Click(object sender, RoutedEventArgs e)
        {
            if (utakmicas.Count > 0)
            {
                System.IO.File.Delete(utakmicas[DataGrid.SelectedIndex].PathData);
                utakmicas.RemoveAt(DataGrid.SelectedIndex);  //da se brise po indeksu
                DataGrid.Items.Refresh();
            }
        }

        private void Izadji_Click(object sender, RoutedEventArgs e)
        {
            serijalizacija.SerializeObject<BindingList<Utakmica>>(utakmicas, "utakmice.xml");
            this.Close();
        }

        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow();
            addWindow.ShowDialog();
        }

       
    }
}
