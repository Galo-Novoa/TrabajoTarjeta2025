namespace TarjetaApp.Franquicias
{
	internal class MedioBoleto : Tarjeta
	{
        public override string Franquicia { get; protected set; }

        public MedioBoleto(decimal SaldoInicial) : base(SaldoInicial)
        {
            this.Franquicia = "Medio Boleto Estudiantil";
        }
    }
}