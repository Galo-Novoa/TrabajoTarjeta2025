using System;

namespace TarjetaApp
{
    internal class Colectivo
    {
        public static decimal PrecioPasajeBase => 1580m;
        public string linea;

        public Colectivo(string linea) { this.linea = linea; }

        public bool PagarCon(Tarjeta tarjeta)
        {
            if (tarjeta.CobrarPasaje())
            {
                var boleto = new Boleto(this.linea, tarjeta);
                tarjeta.AgregarBoleto(boleto);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}