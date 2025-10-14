using System;
using System.Collections.Generic;
using System.Linq;

namespace TarjetaApp
{
    internal class Tarjeta
    {
        private decimal saldo { get; set; }
        private static decimal[] cargasAceptadas =
            { 2000m, 3000m, 4000m, 5000m, 8000m, 10000m, 15000m, 20000m, 25000m, 30000m };

        private List<Boleto> historialViajes;

        public Tarjeta(decimal saldo)
        {
            this.saldo = saldo;
            this.historialViajes = new List<Boleto>();
        }

        public decimal getSaldo() { return saldo; }

        public List<Boleto> getHistorialViajes() { return historialViajes; }

        public void cargarSaldo(decimal monto)
        {
            if (cargasAceptadas.Contains(monto))
            {
                saldo += monto;
                if (saldo > 40000m)
                    saldo = 40000m;

                Console.WriteLine($"Se cargaron ${monto}. Saldo actual: ${saldo}.");
            }
            else
            {
                Console.WriteLine($"Monto no aceptado. Valores válidos: {string.Join(", ", cargasAceptadas)}.");
            }
        }

        public bool cobrarPasaje(decimal monto)
        {
            if (saldo - monto >= 0m)
            {
                saldo -= monto;
                return true;
            }
            else
            {
                Console.WriteLine($"Saldo insuficiente (${saldo}). Precio del pasaje: ${monto}");
                return false;
            }
        }

        public void agregarBoleto(Boleto boleto)
        {
            if (boleto != null)
                historialViajes.Add(boleto);
        }
    }
}
