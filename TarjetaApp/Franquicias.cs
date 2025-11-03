using System;
using System.Collections.Generic;

namespace TarjetaApp
{
    internal class BoletoEducativo : Tarjeta
    {
        protected override string Franquicia => "Boleto Educativo Gratuito";
        protected override decimal Descuento => 0m;

        public BoletoEducativo(decimal saldoInicial) : base(saldoInicial) { }

        private Dictionary<DateOnly, int> viajesPorDia = new();

        public override bool CobrarPasaje()
        {
            DateOnly hoy = DateOnly.FromDateTime(DateTime.Now);
            if (!viajesPorDia.ContainsKey(hoy))
                viajesPorDia[hoy] = 0;

            if (viajesPorDia[hoy] < 2)
            {
                viajesPorDia[hoy]++;
                Console.WriteLine("Viaje gratuito aplicado.");
                return true;
            }
            else
            {
                Console.WriteLine("Límite diario de viajes gratis alcanzado. Se cobrará tarifa normal.");
                return base.CobrarPasaje();
            }
        }
    }

    internal class FranquiciaCompleta : Tarjeta
    {
        protected override string Franquicia => "Franquicia Completa";
        protected override decimal Descuento => 0m;
        public FranquiciaCompleta(decimal saldoInicial) : base(saldoInicial) { }

        private Dictionary<DateOnly, int> viajesPorDia = new();

        public override bool CobrarPasaje()
        {
            DateOnly hoy = DateOnly.FromDateTime(DateTime.Now);
            if (!viajesPorDia.ContainsKey(hoy))
                viajesPorDia[hoy] = 0;

            if (viajesPorDia[hoy] < 2)
            {
                viajesPorDia[hoy]++;
                Console.WriteLine("Viaje gratuito aplicado.");
                return true;
            }
            else
            {
                Console.WriteLine("Límite diario de viajes gratis alcanzado. Se cobrará tarifa normal.");
                return base.CobrarPasaje();
            }
        }
    }

    internal class MedioBoleto : Tarjeta
    {
        protected override string Franquicia => "Medio Boleto Estudiantil";
        protected override decimal Descuento => 0.5m;
        public MedioBoleto(decimal saldoInicial) : base(saldoInicial) { }

        private Dictionary<DateOnly, int> viajesPorDia = new();
        private DateTime ultimoViaje = DateTime.MinValue;

        public override bool CobrarPasaje()
        {
            DateOnly hoy = DateOnly.FromDateTime(DateTime.Now);

            if ((DateTime.Now - ultimoViaje).TotalMinutes < 5)
            {
                Console.WriteLine("Debe esperar al menos 5 minutos entre viajes con medio boleto.");
                return false;
            }

            if (!viajesPorDia.ContainsKey(hoy))
                viajesPorDia[hoy] = 0;

            if (viajesPorDia[hoy] < 2)
            {
                if (base.CobrarPasaje())
                {
                    viajesPorDia[hoy]++;
                    ultimoViaje = DateTime.Now;
                    Console.WriteLine($"Medio boleto aplicado. Viajes con descuento hoy: {viajesPorDia[hoy]}/2");
                    return true;
                }
                return false;
            }
            else
            {
                Console.WriteLine("Límite de viajes con medio boleto alcanzado. Se cobrará tarifa completa.");
                decimal descuentoOriginal = this.Descuento;

                var resultado = this.saldo - Colectivo.PrecioPasajeBase >= SaldoMinimo;
                if (resultado)
                {
                    this.saldo -= Colectivo.PrecioPasajeBase;
                    if (this.saldo < 0)
                        Console.WriteLine($"Saldo en negativo: ${saldo} (viaje plus utilizado)");

                    if (saldoPendiente > 0m)
                        AcreditarCarga();

                    ultimoViaje = DateTime.Now;
                    viajesPorDia[hoy]++;
                }
                else
                {
                    Console.WriteLine($"No se puede realizar el viaje. Límite de Saldo negativo alcanzado (${saldo}).");
                }

                return resultado;
            }
        }
    }
}