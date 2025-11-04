using System;

namespace TarjetaApp
{
    internal class BoletoEducativo : Tarjeta
    {
        protected override string Franquicia => "Boleto Educativo Gratuito";
        protected override decimal Descuento => 0m; // Cambiado a 0m para viajes gratis
        protected override int ViajesGratisPorDia => 2;

        public BoletoEducativo(decimal saldoInicial) : base(saldoInicial) { }
        public BoletoEducativo(decimal saldoInicial, Tiempo tiempo) : base(saldoInicial, tiempo) { }
    }

    internal class FranquiciaCompleta : Tarjeta
    {
        protected override string Franquicia => "Franquicia Completa";
        protected override decimal Descuento => 0m; // Cambiado a 0m para viajes gratis
        protected override int ViajesGratisPorDia => 2;

        public FranquiciaCompleta(decimal saldoInicial) : base(saldoInicial) { }
        public FranquiciaCompleta(decimal saldoInicial, Tiempo tiempo) : base(saldoInicial, tiempo) { }
    }

    internal class MedioBoleto : Tarjeta
    {
        protected override string Franquicia => "Medio Boleto Estudiantil";
        protected override decimal Descuento => 0.5m;
        protected override int ViajesGratisPorDia => 0;
        protected override int MinutosEntreViajes => 5;

        public MedioBoleto(decimal saldoInicial) : base(saldoInicial) { }
        public MedioBoleto(decimal saldoInicial, Tiempo tiempo) : base(saldoInicial, tiempo) { }
    }
}