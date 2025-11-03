using System;
using System.Collections.Generic;
using System.Linq;
namespace TarjetaApp
{
    internal class Tarjeta
    {
        private decimal saldo;
        protected virtual string Franquicia { get; set; } = "Ninguna";
        protected virtual decimal Descuento { get; set; } = 1m;
        private decimal saldoPendiente = 0m;

        private static readonly decimal[] CargasAceptadas =
            { 2000m, 3000m, 4000m, 5000m, 8000m, 10000m, 15000m, 20000m, 25000m, 30000m };

        private DateTime? ultimoViaje = null;
        private Dictionary<DateOnly, int> viajesPorDia = new();

        private readonly int id;
        private static int contadorId = 0;

        public const decimal SaldoMaximo = 56000m;
        public const decimal SaldoMinimo = -1200m;

        private readonly List<Boleto> historialViajes;

        // Getters
        public decimal GetSaldo() => saldo;
        public string GetFranquicia() => Franquicia;
        public int GetId() => id;
        public List<Boleto> GetHistorialViajes() => historialViajes;

        public decimal GetSaldoPendiente() => saldoPendiente;

        public Tarjeta(decimal SaldoInicial)
        {
            this.saldo = SaldoInicial;
            this.historialViajes = new List<Boleto>();
            this.id = contadorId;
            contadorId++;
        }

        public void CargarSaldo(decimal monto)
        {
            if (CargasAceptadas.Contains(monto))
            {
                saldo += monto;
                if (this.saldo > SaldoMaximo)
                {
                    saldoPendiente += this.saldo - SaldoMaximo;
                    this.saldo = SaldoMaximo;
                }
                Console.WriteLine($"Se cargaron ${monto}. Saldo actual: ${this.saldo}.");
            }
            else
            {
                Console.WriteLine($"Monto no aceptado. Valores válidos: {string.Join(", ", CargasAceptadas)}.");
            }
        }

        public virtual bool CobrarPasaje()
        {
            decimal nuevoSaldo = this.saldo - (Colectivo.PrecioPasajeBase * this.Descuento);

            if (nuevoSaldo >= SaldoMinimo)
            {
                saldo = nuevoSaldo;

                if (this.saldo < 0)
                    Console.WriteLine($"Saldo en negativo: ${saldo} (viaje plus utilizado)");

                if (saldoPendiente > 0m)
                    AcreditarCarga();

                return true;
            }
            else
            {
                Console.WriteLine($"No se puede realizar el viaje. Límite de Saldo negativo alcanzado (${saldo}).");
                return false;
            }
        }
        public void AcreditarCarga()
        {
            if (this.saldoPendiente > SaldoMaximo - this.saldo)
            {
                saldoPendiente -= SaldoMaximo - this.saldo;
                this.saldo = SaldoMaximo;
            }
            else
            {
                this.saldo += saldoPendiente;
                saldoPendiente = 0m;
            }
            Console.WriteLine($"Saldo pendiente acreditado. Queda por acreditar: {this.saldoPendiente}.");
        }

        public void AgregarBoleto(Boleto boleto)
        {
            if (boleto != null)
                this.historialViajes.Add(boleto);
        }
    }
}