using System;

namespace TarjetaApp
{
    internal class Colectivo
    {
        public static decimal PrecioPasajeBase => 1580m;
        public virtual decimal PrecioPasaje => PrecioPasajeBase;
        public string linea;
        private readonly Tiempo tiempo;

        public Colectivo(string linea) : this(linea, new Tiempo()) { }

        public Colectivo(string linea, Tiempo tiempo)
        {
            this.linea = linea;
            this.tiempo = tiempo;
        }

        public bool PagarCon(Tarjeta tarjeta)
        {
            if (tarjeta.CobrarPasaje(this.PrecioPasaje))
            {
                var boleto = new Boleto(this.linea, tarjeta, tiempo, this.PrecioPasaje);
                tarjeta.AgregarBoleto(boleto);
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    internal class Interurbano : Colectivo
    {
        public static new decimal PrecioPasajeBase => 3000m;
        public override decimal PrecioPasaje => PrecioPasajeBase;

        public Interurbano(string linea) : base(linea) { }
        public Interurbano(string linea, Tiempo tiempo) : base(linea, tiempo) { }
    }
}