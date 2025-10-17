using System;

namespace TarjetaApp
{
    internal class Colectivo
    {
        private const readonly decimal PRECIO_PASAJE_BASE = 1580m;
        public string linea;

        public Colectivo(string linea) { this.linea = linea; }

        public bool pagarCon(Tarjeta tarjeta)
        {
            if (tarjeta.cobrarPasaje(calcularPrecio(tarjeta))
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

        private decimal calcularPrecio(Tarjeta tarjeta)
        {
            decimal descuento = tarjeta.getFranquicia() switch
            {
                "Franquicia Completa" => 0m,
                "Boleto Educativo Gratuito" => 0m,
                "Medio Boleto Estudiantil" => 0.5m,
                _ => 1m
            };
            return PRECIO_PASAJE_BASE * descuento;
        }

        private decimal calcularPrecio
    }
}