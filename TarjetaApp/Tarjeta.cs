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

        private const decimal LIMITE_SALDO = 40000m;
        private const decimal SALDO_NEGATIVO_PERMITIDO = -1200m;

        private List<Boleto> historialViajes;

        public Tarjeta(decimal saldoInicial)
        {
            this.saldo = saldoInicial;
            this.historialViajes = new List<Boleto>();
        }

        public decimal getSaldo() => saldo;
        public List<Boleto> getHistorialViajes() => historialViajes;

        public void cargarSaldo(decimal monto)
        {
            if (cargasAceptadas.Contains(monto))
            {
                saldo += monto;
                if (saldo > LIMITE_SALDO)
                    saldo = LIMITE_SALDO;

                Console.WriteLine($"Se cargaron ${monto}. Saldo actual: ${saldo}.");
            }
            else
            {
                Console.WriteLine($"Monto no aceptado. Valores válidos: {string.Join(", ", cargasAceptadas)}.");
            }
        }

        public bool cobrarPasaje(decimal monto)
        {
            decimal nuevoSaldo = saldo - monto;

            if (nuevoSaldo >= SALDO_NEGATIVO_PERMITIDO)
            {
                saldo = nuevoSaldo;

                if (saldo < 0)
                    Console.WriteLine($"Saldo en negativo: ${saldo} (viaje plus utilizado)");

                return true;
            }
            else
            {
                Console.WriteLine($"No se puede realizar el viaje. Límite de saldo negativo alcanzado (${saldo}).");
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