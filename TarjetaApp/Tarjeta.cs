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
        protected decimal saldoPendiente = 0m;

        // Variables para control de viajes diarios
        protected Dictionary<DateTime, int> viajesPorDia = new();
        protected DateTime ultimoViaje = DateTime.MinValue;
        protected virtual int ViajesGratisPorDia => 0;
        protected virtual int MinutosEntreViajes => 0;

        private static readonly decimal[] CargasAceptadas =
            { 2000m, 3000m, 4000m, 5000m, 8000m, 10000m, 15000m, 20000m, 25000m, 30000m };

        private readonly int id;
        private static int contadorId = 0;

        public const decimal SaldoMaximo = 56000m;
        public const decimal SaldoMinimo = -1200m;

        private readonly List<Boleto> historialViajes;
        protected Tiempo tiempo;

        // Getters
        public decimal GetSaldo() => saldo;
        public string GetFranquicia() => Franquicia;
        public int GetId() => id;
        public List<Boleto> GetHistorialViajes() => historialViajes;
        public decimal GetSaldoPendiente() => saldoPendiente;

        // Constructores
        public Tarjeta(decimal SaldoInicial) : this(SaldoInicial, new Tiempo()) { }

        public Tarjeta(decimal SaldoInicial, Tiempo tiempo)
        {
            this.saldo = SaldoInicial;
            this.historialViajes = new List<Boleto>();
            this.id = contadorId;
            this.tiempo = tiempo;
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
            DateTime hoy = tiempo.Today();

            // Verificar tiempo mínimo entre viajes (solo para franquicias Parciales)
            if (Franquicia == "Parcial" && MinutosEntreViajes > 0 && (tiempo.Now() - ultimoViaje).TotalMinutes < MinutosEntreViajes)
            {
                Console.WriteLine($"Debe esperar al menos {MinutosEntreViajes} minutos entre viajes.");
                return false;
            }

            // Verificar viajes gratis
            if (!viajesPorDia.ContainsKey(hoy))
                viajesPorDia[hoy] = 0;

            decimal descuentoAplicar = Descuento;

            // LÓGICA PARA FRANQUICIAS PARCIALES: solo 2 viajes con 50% de descuento
            if (Franquicia == "Parcial" && viajesPorDia[hoy] >= 2)
            {
                descuentoAplicar = 1m; // Precio completo después de 2 viajes
                Console.WriteLine("Límite de viajes con descuento alcanzado. Se cobrará tarifa completa.");
            }

            decimal montoACobrar = Colectivo.PrecioPasajeBase * descuentoAplicar;

            // Viajes gratis para franquicias Completas (solo 2 por día)
            if (Franquicia == "Completa" && viajesPorDia[hoy] < ViajesGratisPorDia)
            {
                // Viaje gratis
                viajesPorDia[hoy]++;
                ultimoViaje = tiempo.Now();
                Console.WriteLine($"Viaje gratuito aplicado. Viajes gratis hoy: {viajesPorDia[hoy]}/{ViajesGratisPorDia}");
                return true;
            }

            // Cobro normal
            decimal nuevoSaldo = this.saldo - montoACobrar;

            if (nuevoSaldo >= SaldoMinimo)
            {
                saldo = nuevoSaldo;
                viajesPorDia[hoy]++;
                ultimoViaje = tiempo.Now();

                if (this.saldo < 0)
                    Console.WriteLine($"Saldo en negativo: ${saldo} (viaje plus utilizado)");

                if (saldoPendiente > 0m)
                    AcreditarCarga();

                // Mensaje específico para franquicias Parciales
                if (Franquicia == "Parcial" && viajesPorDia[hoy] <= 2)
                {
                    Console.WriteLine($"Descuento aplicado. Viajes con descuento hoy: {viajesPorDia[hoy]}/2");
                }

                if (Franquicia == "Completa" && viajesPorDia[hoy] == ViajesGratisPorDia)
                {
                    Console.WriteLine($"Límite diario de viajes gratis alcanzado. Se cobrará tarifa normal.");
                }

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