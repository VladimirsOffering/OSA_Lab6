using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSA_Lab6.Models
{
    public class States
    {
        public List<List<double>> State { get; set; }

        public States(int count)
        {
            State = new List<List<double>>();
        }
    }
}
