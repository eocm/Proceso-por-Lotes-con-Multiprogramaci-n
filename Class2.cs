using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programa_1
{
    public class Lote
    {
        public Proceso proceso1 = new Proceso();
        public Proceso proceso2 = new Proceso();
        public Proceso proceso3 = new Proceso();
        public Lote(Proceso proceso1, Proceso proceso2, Proceso proceso3)
        {
            this.proceso1 = proceso1;
            this.proceso2 = proceso2;
            this.proceso3 = proceso3;
        }
        public Lote(){}
    }
}