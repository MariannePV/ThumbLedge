using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThumbLedge.Model
{
    //Model per actualitzar els valors i les seves dates d'un coneixement
    public class PatchModel
    {
        public string NomConeixement { get; set; }
        public int[] Valors { get; set; }
        public DateTime[] DatesValors { get; set; }
    }
}
