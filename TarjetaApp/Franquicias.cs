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
    }
}