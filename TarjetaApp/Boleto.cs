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

        // Getters
        public string GetLinea() => linea;
        public string GetFranquicia() => franquicia;
        public DateTime GetFecha() => fecha;
        public decimal GetMonto() => monto;
        public int GetId() => id;
        public decimal GetSaldo() => saldo;
        public decimal GetRestante() => restante;

        // Constructor PRINCIPAL que acepta precio personalizado
        public Boleto(string linea, Tarjeta tarjeta, Tiempo tiempo, decimal precioPasaje)
        {
            this.linea = linea;
            this.franquicia = tarjeta.GetFranquicia();
            this.monto = precioPasaje; // ← Usar el precio específico
            this.id = tarjeta.GetId();
            this.restante = tarjeta.GetSaldo();
            this.saldo = restante + monto;
            this.fecha = tiempo.Now();
        }

        // Constructor de compatibilidad (para código existente)
        public Boleto(string linea, Tarjeta tarjeta, Tiempo tiempo)
            : this(linea, tarjeta, tiempo, Colectivo.PrecioPasajeBase) { }
    }
}