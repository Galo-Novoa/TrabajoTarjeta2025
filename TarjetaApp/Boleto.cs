using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarjetaApp
{
    internal class Boleto
    {
        private readonly string linea;
        private readonly string franquicia;
        private readonly DateTime fecha;
        private readonly decimal monto;
        private readonly int id;
        private readonly decimal saldo;
        private readonly decimal restante;

        // Getters
        public string GetLinea() => linea;
        public string GetFranquicia() => franquicia;
        public DateTime GetFecha() => fecha;
        public decimal GetMonto() => monto;
        public int GetId() => id;
        public decimal GetSaldo() => saldo;
        public decimal GetRestante() => restante;

        public Boleto(string linea, Tarjeta tarjeta)
        {
            this.linea = linea;
            this.franquicia = tarjeta.GetFranquicia();
            this.monto = Colectivo.PrecioPasajeBase;
            this.id = tarjeta.GetId();
            this.restante = tarjeta.GetSaldo();
            this.saldo = restante + monto;

            //Falta implementación real de tiempo para Fecha.
            this.fecha = DateTime.Now;
        }
    }
}