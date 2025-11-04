using System;

namespace TarjetaApp
{
    public class Tiempo
    {
        public Tiempo() { }

        public virtual DateTime Now()
        {
            return DateTime.Now;
        }

        public virtual DateTime Today()
        {
            return DateTime.Today;
        }
    }

    internal class TiempoFalso : Tiempo
    {
        private DateTime tiempoActual;

        public TiempoFalso()
        {
            tiempoActual = new DateTime(2024, 10, 14, 10, 0, 0);
        }

        public TiempoFalso(DateTime fechaInicial)
        {
            tiempoActual = fechaInicial;
        }

        public override DateTime Now()
        {
            return tiempoActual;
        }

        public override DateTime Today()
        {
            return tiempoActual.Date;
        }

        public void AgregarDias(int cantidad)
        {
            tiempoActual = tiempoActual.AddDays(cantidad);
        }

        public void AgregarMinutos(int cantidad)
        {
            tiempoActual = tiempoActual.AddMinutes(cantidad);
        }

        public void AgregarHoras(int cantidad)
        {
            tiempoActual = tiempoActual.AddHours(cantidad);
        }

        public void EstablecerTiempo(DateTime nuevoTiempo)
        {
            tiempoActual = nuevoTiempo;
        }
    }
}