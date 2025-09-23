using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarjetaApp
{
    internal class Tarjeta
    {
        private decimal saldo { get; set; }
        private static decimal[] cargasAceptadas = { 2000m, 3000m, 4000m, 5000m, 8000m, 10000m, 15000m, 20000m, 25000m, 30000m };
        public Tarjeta(decimal saldo) {
            this.saldo = saldo;
        }
        public decimal getSaldo() { return saldo; }

        public void cargarSaldo(decimal monto)
        {
            if (cargasAceptadas.Contains(monto))
            {
                this.saldo = this.saldo + monto;
                if (saldo > 40000m)
                    saldo = 40000m;
                Console.WriteLine($"Se cargaron ${monto}. Saldo: ${this.saldo}.");
            }
            else
            {
                Console.WriteLine($"Valor de carga no aceptado, por favor ingrese uno de los siguientes: {string.Join(", ", cargasAceptadas)}.");
            }
        }

        public bool cobrarPasaje(decimal monto)
        {
            if (this.saldo - monto >= 0m)
            {
                this.saldo -= monto;
                return true;
            }
            else
            {
                Console.WriteLine($"Saldo insuficiente: {this.saldo}. Precio del pasaje: {monto}");
                return false;
            }
        }
        
    }
}