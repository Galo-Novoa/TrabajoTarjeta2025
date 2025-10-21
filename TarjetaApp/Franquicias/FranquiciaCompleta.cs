namespace TarjetaApp.Franquicias
{
    internal class FranquiciaCompleta : Tarjeta
    {
        public override string Franquicia { get; set; }

        public FranquiciaCompleta(decimal SaldoInicial) : base(SaldoInicial)
        {
            this.Franquicia = "Franquicia Completa";
        }
    }
}