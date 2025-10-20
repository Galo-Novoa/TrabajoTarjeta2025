using System;

namespace TarjetaApp
{
    internal class Colectivo
    {
        private static readonly decimal precioPasaje = 1580m;
        public string linea;

        public Colectivo(string linea) { this.linea = linea; }

        public bool pagarCon(Tarjeta tarjeta)
        {
            if (tarjeta.cobrarPasaje(precioPasaje))
            {
                Boleto boleto = new Boleto(this.linea);
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