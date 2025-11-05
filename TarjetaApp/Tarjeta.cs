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

        // Nuevas variables para control de trasbordos
        private Boleto ultimoBoleto = null;
        private DateTime horaPrimerViajeTrasbordo = DateTime.MinValue;
        private int cantidadTrasbordos = 0;

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
        public Boleto GetUltimoBoleto() => ultimoBoleto; // Nuevo getter
        public int GetCantidadTrasbordos() => cantidadTrasbordos; // Nuevo getter

        // Constructores (sin cambios)
        public Tarjeta(decimal SaldoInicial) : this(SaldoInicial, new Tiempo()) { }

        public Tarjeta(decimal SaldoInicial, Tiempo tiempo)
        {
            this.saldo = SaldoInicial;
            this.historialViajes = new List<Boleto>();
            this.id = contadorId;
            this.tiempo = tiempo;
            this.mesActual = new DateTime(tiempo.Now().Year, tiempo.Now().Month, 1);
            contadorId++;
        }

        // Método para validar horario de franquicias (sin cambios)
        protected virtual bool ValidarHorarioFranquicia()
        {
            if (Franquicia == "Ninguna")
                return true;

            DateTime ahora = tiempo.Now();

            if (ahora.DayOfWeek == DayOfWeek.Saturday || ahora.DayOfWeek == DayOfWeek.Sunday)
                return false;

            TimeSpan horaActual = ahora.TimeOfDay;
            TimeSpan horaInicio = new TimeSpan(6, 0, 0);
            TimeSpan horaFin = new TimeSpan(22, 0, 0);

            return horaActual >= horaInicio && horaActual < horaFin;
        }

        // Nuevo método para validar horario de trasbordos
        protected virtual bool ValidarHorarioTrasbordo()
        {
            DateTime ahora = tiempo.Now();

            // Trasbordos válidos de lunes a sábado de 7:00 a 22:00
            if (ahora.DayOfWeek == DayOfWeek.Sunday)
                return false;

            TimeSpan horaActual = ahora.TimeOfDay;
            TimeSpan horaInicio = new TimeSpan(7, 0, 0);  // 7:00 AM
            TimeSpan horaFin = new TimeSpan(22, 0, 0);    // 10:00 PM

            return horaActual >= horaInicio && horaActual < horaFin;
        }

        // Nuevo método para verificar si es un trasbordo válido
        private bool EsTrasbordoValido(string lineaColectivo)
        {
            // Si no hay último boleto, no es trasbordo
            if (ultimoBoleto == null)
                return false;

            DateTime ahora = tiempo.Now();

            // Verificar que no haya pasado más de 1 hora desde el primer viaje del trasbordo
            if ((ahora - horaPrimerViajeTrasbordo).TotalMinutes > 60)
            {
                ReiniciarContadorTrasbordo();
                return false;
            }

            // Verificar horario de trasbordos
            if (!ValidarHorarioTrasbordo())
                return false;

            // Verificar que sea una línea diferente
            if (ultimoBoleto.GetLinea() == lineaColectivo)
                return false;

            return true;
        }

        // Nuevo método para reiniciar contador de trasbordos
        private void ReiniciarContadorTrasbordo()
        {
            cantidadTrasbordos = 0;
            horaPrimerViajeTrasbordo = DateTime.MinValue;
        }

        public virtual bool CobrarPasaje(decimal precioPasaje, string lineaColectivo = "")
        {
            DateTime hoy = tiempo.Today();

            // Verificar si es un trasbordo válido
            bool esTrasbordo = !string.IsNullOrEmpty(lineaColectivo) && EsTrasbordoValido(lineaColectivo);

            if (esTrasbordo)
            {
                // Trasbordo gratuito
                Console.WriteLine("Trasbordo aplicado - Viaje gratuito");

                // Crear boleto de trasbordo con monto 0
                var boletoTrasbordo = new Boleto(lineaColectivo, this, tiempo, 0m, true);
                AgregarBoleto(boletoTrasbordo);

                // Actualizar último boleto
                ultimoBoleto = boletoTrasbordo;
                cantidadTrasbordos++;

                return true;
            }

            // Si no es trasbordo, continuar con la lógica normal de cobro...

            // Verificar horario para franquicias
            if (!ValidarHorarioFranquicia())
            {
                Console.WriteLine($"Franquicia '{Franquicia}' no disponible fuera del horario permitido (Lunes a Viernes 6:00-22:00).");
                return false;
            }

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

                // Iniciar contador de trasbordo si es el primer viaje
                if (cantidadTrasbordos == 0)
                {
                    horaPrimerViajeTrasbordo = tiempo.Now();
                }

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

                // Iniciar contador de trasbordo si es el primer viaje
                if (cantidadTrasbordos == 0)
                {
                    horaPrimerViajeTrasbordo = tiempo.Now();
                }

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

        // Resto de los métodos permanecen igual...
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
            {
                this.historialViajes.Add(boleto);
                // Actualizar último boleto para control de trasbordos
                if (!boleto.EsTrasbordo())
                {
                    ultimoBoleto = boleto;
                }
            }
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
                Console.WriteLine($"Monto no aceptado. Valores v├ílidos: {string.Join(", ", CargasAceptadas)}.");
            }
        }
    }
}