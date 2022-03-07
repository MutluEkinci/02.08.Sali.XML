using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    [Serializable]//bu bir serileştirilebir sınıftır
    class Personel
    {
        public int PerID { get; set; }
        public string PerAd { get; set; }
        public string Adres { get; set; }
    }
}
