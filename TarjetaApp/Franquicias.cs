namespace TarjetaApp
{
    internal class BoletoEducativo : Tarjeta
    {
        protected override string Franquicia => "Boleto Educativo Gratuito";
        protected override decimal Descuento => 0m;

        public BoletoEducativo(decimal saldoInicial) : base(saldoInicial) { }
    }

    internal class FranquiciaCompleta : Tarjeta
    {
        protected override string Franquicia => "Franquicia Completa";
        protected override decimal Descuento => 0m;
        public FranquiciaCompleta(decimal saldoInicial) : base(saldoInicial) { }

    }

    internal class MedioBoleto : Tarjeta
    {
        protected override string Franquicia => "Medio Boleto Estudiantil";
        protected override decimal Descuento => 0.5m;
        public MedioBoleto(decimal saldoInicial) : base(saldoInicial) { }
    }
}