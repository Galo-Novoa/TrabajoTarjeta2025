namespace TarjetaApp
{
    internal class BoletoEducativo : Tarjeta
    {
        public BoletoEducativo(decimal SaldoInicial) : base(SaldoInicial)
        {
            this.franquicia = "Boleto Educativo Gratuito";
        }
    }
    internal class FranquiciaCompleta : Tarjeta
    {
        public FranquiciaCompleta(decimal SaldoInicial) : base(SaldoInicial)
        {
            this.franquicia = "Franquicia Completa";
        }
    }
    internal class MedioBoleto : Tarjeta
    {
        public MedioBoleto(decimal SaldoInicial) : base(SaldoInicial)
        {
            this.franquicia = "Medio Boleto Estudiantil";
        }
    }
}