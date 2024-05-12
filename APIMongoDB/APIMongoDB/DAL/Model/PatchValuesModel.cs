using System;

namespace APIMongoDB.DAL.Model
{
    public class PatchValuesModel
    {
        public string NomConeixement { get; set; }
        public int[] Valors { get; set; }
        public DateTime[] DatesValors { get; set; }
    }
}
