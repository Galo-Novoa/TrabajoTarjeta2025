using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarjetaApp
{
    internal class Boleto
    {
        public string Linea { get; private set; }
        public string Franquicia { get; private set; }
        public DateTime Fecha { get; private set; }
        public  decimal Monto { get; private set; }
        public int Id { get; private set; }
        public Boleto(string linea, Tarjeta tarjeta, decimal monto)
        {
            this.Linea = linea;
            this.Franquicia = tarjeta.Franquicia;
            this.Monto = monto;
            this.Id = tarjeta.Id;
            //Falta implementación de sistema de tiempo para Fecha.
        }

    }
}
