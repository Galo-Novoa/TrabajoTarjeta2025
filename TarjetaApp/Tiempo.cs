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

    public class TiempoFalso : Tiempo
    {
        private DateTime _tiempoActual;

        public TiempoFalso()
        {
            _tiempoActual = new DateTime(2024, 10, 14, 10, 0, 0);
        }

        public TiempoFalso(DateTime fechaInicial)
        {
            _tiempoActual = fechaInicial;
        }

        public override DateTime Now()
        {
            return _tiempoActual;
        }

        public override DateTime Today()
        {
            return _tiempoActual.Date;
        }

        public void AgregarDias(int cantidad)
        {
            _tiempoActual = _tiempoActual.AddDays(cantidad);
        }

        public void AgregarMinutos(int cantidad)
        {
            _tiempoActual = _tiempoActual.AddMinutes(cantidad);
        }

        public void AgregarHoras(int cantidad)
        {
            _tiempoActual = _tiempoActual.AddHours(cantidad);
        }

        public void EstablecerTiempo(DateTime nuevoTiempo)
        {
            _tiempoActual = nuevoTiempo;
        }
    }
}