using System;
using System.Collections.Generic;
using System.Linq;

namespace TarjetaApp
{
    internal class Tarjeta
    {
        private decimal Saldo { get; set; }
        public virtual string Franquicia { get; set; } = "Ninguna";
        private static readonly decimal[] CargasAceptadas =
            { 2000m, 3000m, 4000m, 5000m, 8000m, 10000m, 15000m, 20000m, 25000m, 30000m };

        private const decimal LimiteSaldo = 40000m;
        private const decimal SaldoNegativoPermitido = -1200m;

        public List<Boleto> historialViajes;

        public Tarjeta(decimal SaldoInicial)
        {
            this.Saldo = SaldoInicial;
            this.historialViajes = new List<Boleto>();
        }
        public decimal GetSaldo() => Saldo;
        public string GetFranquicia() => Franquicia;
        public List<Boleto> GetHistorialViajes() => historialViajes;

        public void CargarSaldo(decimal monto)
        {
            if (CargasAceptadas.Contains(monto))
            {
                Saldo += monto;
                if (Saldo > LimiteSaldo)
                    Saldo = LimiteSaldo;

                Console.WriteLine($"Se cargaron ${monto}. Saldo actual: ${Saldo}.");
            }
            else
            {
                Console.WriteLine($"Monto no aceptado. Valores válidos: {string.Join(", ", CargasAceptadas)}.");
            }
        }

        public bool CobrarPasaje(decimal monto)
        {
            decimal nuevoSaldo = Saldo - monto;

            if (nuevoSaldo >= SaldoNegativoPermitido)
            {
                Saldo = nuevoSaldo;

                if (Saldo < 0)
                    Console.WriteLine($"Saldo en negativo: ${Saldo} (viaje plus utilizado)");

                return true;
            }
            else
            {
                Console.WriteLine($"No se puede realizar el viaje. Límite de Saldo negativo alcanzado (${Saldo}).");
                return false;
            }
        }

        public void AgregarBoleto(Boleto boleto)
        {
            if (boleto != null)
                historialViajes.Add(boleto);
        }
    }
}
