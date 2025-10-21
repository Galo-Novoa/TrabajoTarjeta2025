namespace TarjetaApp.Franquicias
{
    internal class BoletoEducativo : Tarjeta
    {
        public override string Franquicia { get; set; }

        public BoletoEducativo(decimal SaldoInicial) : base(SaldoInicial)
        {
            this.Franquicia  = "Boleto Educativo Gratuito";
        }
    }
}