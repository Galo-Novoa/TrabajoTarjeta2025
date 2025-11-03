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
        protected override decimal Descuento => 0.5m; // Siempre 50% de descuento
        protected override int ViajesGratisPorDia => 0; // No tiene viajes gratis
        protected override int MinutosEntreViajes => 5; // 5 minutos entre viajes

        public MedioBoleto(decimal saldoInicial) : base(saldoInicial) { }
    }
}