using System;
using System.Linq;

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
            // Pasar la línea del colectivo para verificar trasbordos
            if (tarjeta.CobrarPasaje(this.PrecioPasaje, this.linea))
            {
                // Solo crear boleto si no fue un trasbordo (los trasbordos ya crean su boleto)
                if (!tarjeta.GetHistorialViajes().Any(b => b.EsTrasbordo() && b.GetLinea() == this.linea &&
                    (tiempo.Now() - b.GetFecha()).TotalMinutes < 1))
                {
                    var boleto = new Boleto(this.linea, tarjeta, tiempo, this.PrecioPasaje);
                    tarjeta.AgregarBoleto(boleto);
                }
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