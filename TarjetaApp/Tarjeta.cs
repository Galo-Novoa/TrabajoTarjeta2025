using System;
using System.Collections.Generic;
using System.Linq;

namespace TarjetaApp
{
    internal class Tarjeta
    {
        public decimal Saldo { get; private set; }
        public virtual string Franquicia { get; protected set; } = "Ninguna";
        private static readonly decimal[] CargasAceptadas =
            { 2000m, 3000m, 4000m, 5000m, 8000m, 10000m, 15000m, 20000m, 25000m, 30000m };

        public int Id { get; set; }
        private static int contadorId = 0;

        private const decimal LimiteSaldo = 40000m;
        private const decimal SaldoNegativoPermitido = -1200m;

        public List<Boleto> HistorialViajes { get; private set; }

        public Tarjeta(decimal SaldoInicial)
        {
            this.Saldo = SaldoInicial;
            this.HistorialViajes = new List<Boleto>();
            this.Id = contadorId;
            contadorId++;
        }

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
                HistorialViajes.Add(boleto);
        }
    }
}
