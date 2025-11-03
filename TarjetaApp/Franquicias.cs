using System;

namespace TarjetaApp
{
    internal class BoletoEducativo : Tarjeta
    {
        protected override string Franquicia => "Boleto Educativo Gratuito";
        protected override decimal Descuento => 1m; // Se cobra normal después de los viajes gratis
        protected override int ViajesGratisPorDia => 2;

        public BoletoEducativo(decimal saldoInicial) : base(saldoInicial) { }
    }

    internal class FranquiciaCompleta : Tarjeta
    {
        protected override string Franquicia => "Franquicia Completa";
        protected override decimal Descuento => 1m; // Se cobra normal después de los viajes gratis
        protected override int ViajesGratisPorDia => 2;

        public FranquiciaCompleta(decimal saldoInicial) : base(saldoInicial) { }
    }

    internal class MedioBoleto : Tarjeta
    {
        protected override string Franquicia => "Medio Boleto Estudiantil";
        protected override decimal Descuento => 0.5m;
        protected override int ViajesGratisPorDia => 0;
        protected override int MinutosEntreViajes => 5;

        public MedioBoleto(decimal saldoInicial) : base(saldoInicial) { }

        public override bool CobrarPasaje()
        {
            DateTime hoy = DateTime.Today;

            // Verificar tiempo mínimo entre viajes
            if ((DateTime.Now - ultimoViaje).TotalMinutes < MinutosEntreViajes)
            {
                Console.WriteLine($"Debe esperar al menos {MinutosEntreViajes} minutos entre viajes con medio boleto.");
                return false;
            }

            // Verificar límite de viajes con descuento
            if (!viajesPorDia.ContainsKey(hoy))
                viajesPorDia[hoy] = 0;

            // Si ya hizo 2 viajes hoy, aplicar tarifa completa
            if (viajesPorDia[hoy] >= 2)
            {
                // Usar el método base para cobro normal
                return base.CobrarPasaje();
            }

            // Para los primeros 2 viajes, usar la lógica con descuento
            return base.CobrarPasaje();
        }
    }
}