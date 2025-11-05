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

        private int numeroViaje = 1;
        private DateTime mesActual;

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
        public int GetNumeroViaje() => numeroViaje;

        // Constructores
        public Tarjeta(decimal SaldoInicial) : this(SaldoInicial, new Tiempo()) { }

        public Tarjeta(decimal SaldoInicial, Tiempo tiempo)
        {
            this.saldo = SaldoInicial;
            this.historialViajes = new List<Boleto>();
            this.id = contadorId;
            this.tiempo = tiempo;
            this.mesActual = new DateTime(tiempo.Now().Year, tiempo.Now().Month, 1); // Primer día del mes
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

        public virtual bool CobrarPasaje(decimal precioPasaje)
        {
            DateTime hoy = tiempo.Today();

            // Verificar reinicio mensual
            VerificarReinicioMensual();

            // Verificar tiempo mínimo entre viajes (solo para franquicias Parciales)
            if (Franquicia == "Parcial" && MinutosEntreViajes > 0 && (tiempo.Now() - ultimoViaje).TotalMinutes < MinutosEntreViajes)
            {
                Console.WriteLine($"Debe esperar al menos {MinutosEntreViajes} minutos entre viajes.");
                return false;
            }

            // Verificar viajes gratis
            if (!viajesPorDia.ContainsKey(hoy))
                viajesPorDia[hoy] = 0;

            // Calcular descuentos
            decimal descuentoFranquicia = CalcularDescuentoFranquicia(viajesPorDia[hoy]);
            decimal descuentoUsoFrecuente = CalcularDescuentoUsoFrecuente();

            // Aplicar descuentos (el menor descuento aplica)
            decimal descuentoAplicar = Math.Min(descuentoFranquicia, descuentoUsoFrecuente);

            decimal montoACobrar = precioPasaje * descuentoAplicar;

            // Viajes gratis para franquicias Completas
            if (Franquicia == "Completa" && viajesPorDia[hoy] < ViajesGratisPorDia)
            {
                viajesPorDia[hoy]++;
                ultimoViaje = tiempo.Now();
                Console.WriteLine($"Viaje gratuito aplicado. Viajes gratis hoy: {viajesPorDia[hoy]}/{ViajesGratisPorDia}");

                if (viajesPorDia[hoy] == ViajesGratisPorDia)
                {
                    Console.WriteLine($"Límite diario de viajes gratis alcanzado.");
                }
                return true;
            }

            // Cobro normal
            decimal nuevoSaldo = this.saldo - montoACobrar;

            if (nuevoSaldo >= SaldoMinimo)
            {
                saldo = nuevoSaldo;
                viajesPorDia[hoy]++;
                ultimoViaje = tiempo.Now();

                // Mostrar información de descuentos aplicados
                MostrarInfoDescuentos(descuentoFranquicia, descuentoUsoFrecuente, montoACobrar);

                // INCREMENTAR CONTADOR DE VIAJES MENSUALES SOLO SI ES TARJETA NORMAL
                if (Franquicia == "Ninguna")
                {
                    numeroViaje++;
                }

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

        private void VerificarReinicioMensual()
        {
            DateTime hoy = tiempo.Today();
            DateTime primerDiaMesActual = new DateTime(hoy.Year, hoy.Month, 1);

            if (mesActual < primerDiaMesActual)
            {
                Console.WriteLine($"¡Nuevo mes! Reiniciando contador de viajes frecuentes. Viajes del mes anterior: {numeroViaje - 1}");
                numeroViaje = 1;
                mesActual = primerDiaMesActual;
            }
        }

        private decimal CalcularDescuentoFranquicia(int viajesHoy)
        {
            if (Franquicia == "Parcial" && viajesHoy < 2)
            {
                return 0.5m; // 50% descuento primeros 2 viajes
            }
            return 1m; // Tarifa completa
        }

        public decimal CalcularDescuentoUsoFrecuente(int numeroViajeTest = 0)
        {
            int viaje = numeroViajeTest > 0 ? numeroViajeTest : numeroViaje;

            if (Franquicia != "Ninguna")
                return 1m;

            return viaje switch
            {
                >= 30 and <= 59 => 0.8m,  // 20% descuento
                >= 60 and <= 80 => 0.75m, // 25% descuento
                _ => 1m                   // Tarifa normal
            };
        }

        private void MostrarInfoDescuentos(decimal descuentoFranquicia, decimal descuentoUsoFrecuente, decimal montoACobrar)
        {
            // Mostrar información de descuento de uso frecuente
            if (Franquicia == "Ninguna" && descuentoUsoFrecuente < 1m)
            {
                string rango = numeroViaje switch
                {
                    >= 30 and <= 59 => "20%",
                    >= 60 and <= 80 => "25%",
                    _ => "0%"
                };
                Console.WriteLine($"Boleto uso frecuente: Viaje #{numeroViaje} - {rango} descuento");
            }

            // Mostrar información de descuento de franquicia
            if (descuentoFranquicia < 1m)
            {
                Console.WriteLine($"Descuento de franquicia aplicado: ${montoACobrar}");
            }
        }

        public void AgregarBoleto(Boleto boleto)
        {
            if (boleto != null)
                this.historialViajes.Add(boleto);
        }
    }
}