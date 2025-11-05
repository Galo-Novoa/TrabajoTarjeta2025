using System;

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
        private readonly bool esTrasbordo; // Nuevo campo para identificar trasbordos

        // Getters
        public string GetLinea() => linea;
        public string GetFranquicia() => franquicia;
        public DateTime GetFecha() => fecha;
        public decimal GetMonto() => monto;
        public int GetId() => id;
        public decimal GetSaldo() => saldo;
        public decimal GetRestante() => restante;
        public bool EsTrasbordo() => esTrasbordo; // Nuevo getter

        // Constructor PRINCIPAL que acepta precio personalizado y flag de trasbordo
        public Boleto(string linea, Tarjeta tarjeta, Tiempo tiempo, decimal precioPasaje, bool esTrasbordo = false)
        {
            this.linea = linea;
            this.franquicia = tarjeta.GetFranquicia();
            this.monto = precioPasaje;
            this.id = tarjeta.GetId();
            this.restante = tarjeta.GetSaldo();
            this.saldo = restante + monto;
            this.fecha = tiempo.Now();
            this.esTrasbordo = esTrasbordo;
        }

        // Constructor de compatibilidad (para código existente)
        public Boleto(string linea, Tarjeta tarjeta, Tiempo tiempo)
            : this(linea, tarjeta, tiempo, Colectivo.PrecioPasajeBase) { }
    }
}