using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klase
{
    [Serializable]
    public class Utakmica
    {
        public string Utakmicaa { get; set; }
        public DateTime Datum { get; set; }
        public double BrPoena { get; set; }

        public string PathSlika { get; set; }
        public string PathData { get; set; }

        public int ID { get; set; }
        public Utakmica()
        {
           

        }

        public Utakmica(string utakmica, DateTime datum, double brPoena, string pathSlika, string pathData, int id)
        {
            Utakmicaa = utakmica;
            Datum = datum;
            BrPoena = brPoena;
            PathSlika = pathSlika;
            PathData = pathData;
            ID=id;
        }

     
    }
}
