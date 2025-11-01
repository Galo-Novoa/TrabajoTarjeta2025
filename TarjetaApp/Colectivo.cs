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
            if (tarjeta.CobrarPasaje(CalcularPrecio(tarjeta)))
            {
                var boleto = new Boleto(this.linea, tarjeta, PrecioPasajeBase);
                tarjeta.AgregarBoleto(boleto);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static decimal CalcularPrecio(Tarjeta tarjeta)
        {
            decimal descuento = tarjeta.Franquicia switch
            {
                "Franquicia Completa" => 0m,
                "Boleto Educativo Gratuito" => 0m,
                "Medio Boleto Estudiantil" => 0.5m,
                _ => 1m
            };
            return PrecioPasajeBase * descuento;
        }
    }
}