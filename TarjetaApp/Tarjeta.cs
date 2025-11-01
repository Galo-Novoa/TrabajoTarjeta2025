using System;
using System.Collections.Generic;
using System.Linq;

namespace TarjetaApp
{
    internal class Tarjeta
    {
        private decimal saldo;
        protected string franquicia = "Ninguna";
        private static readonly decimal[] CargasAceptadas =
            { 2000m, 3000m, 4000m, 5000m, 8000m, 10000m, 15000m, 20000m, 25000m, 30000m };

        private readonly int id;
        private static int contadorId = 0;

        private const decimal LimiteSaldo = 40000m;
        private const decimal SaldoNegativoPermitido = -1200m;

        private readonly List<Boleto> historialViajes;

        // Getters
        public decimal GetSaldo() => saldo;
        public string GetFranquicia() => franquicia;
        public int GetId() => id;
        public List<Boleto> GetHistorialViajes() => historialViajes;

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
                if (saldo > LimiteSaldo)
                    saldo = LimiteSaldo;

                Console.WriteLine($"Se cargaron ${monto}. Saldo actual: ${saldo}.");
            }
            else
            {
                Console.WriteLine($"Monto no aceptado. Valores válidos: {string.Join(", ", CargasAceptadas)}.");
            }
        }

        public bool CobrarPasaje(decimal monto)
        {
            decimal nuevoSaldo = saldo - monto;

            if (nuevoSaldo >= SaldoNegativoPermitido)
            {
                saldo = nuevoSaldo;

                if (saldo < 0)
                    Console.WriteLine($"Saldo en negativo: ${saldo} (viaje plus utilizado)");

                return true;
            }
            else
            {
                Console.WriteLine($"No se puede realizar el viaje. Límite de Saldo negativo alcanzado (${saldo}).");
                return false;
            }
        }

        public void AgregarBoleto(Boleto boleto)
        {
            if (boleto != null)
                historialViajes.Add(boleto);
        }

        // Setter protegido para Franquicia (para clases hijas)
        protected void SetFranquicia(string nuevaFranquicia)
        {
            franquicia = nuevaFranquicia;
        }
    }
}