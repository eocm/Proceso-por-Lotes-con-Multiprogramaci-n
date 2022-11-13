using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Programa_1
{
    public class Proceso
    {
        public int ID;
        public int time;
        public float op1;
        public float op2;
        public string opera;
        public Proceso(int ID, int time, float op1, float op2, string opera)
        {
            this.ID = ID;
            this.time = time;
            this.op1 = op1;
            this.op2 = op2;
            this.opera = opera;
        }
        public Proceso(){}
    }
}
