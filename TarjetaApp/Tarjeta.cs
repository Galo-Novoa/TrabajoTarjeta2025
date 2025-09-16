using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabajoTarjeta2025
{
    internal class Tarjeta
    {
        private decimal saldo { get; set; }
        private decimal[] valoresValidos = { 2000m, 3000m, 4000m, 5000m, 8000m, 10000m, 15000m, 20000m, 25000m, 30000m };
        public Tarjeta(decimal saldo) {
            this.saldo = saldo;
        }
        public decimal getSaldo() { return saldo; }

        public void cargarSaldo(Tarjeta tarjeta,  decimal monto)
        {
            if (valoresValidos.Contains(monto)) {
                tarjeta.saldo = tarjeta.saldo + monto;
            }
        }
        
    }
}
