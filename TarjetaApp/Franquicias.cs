using System;
using TarjetaApp;

namespace TarjetaApp
{
    internal class BoletoEducativo : Tarjeta
    {
        protected override string Franquicia => "Completa";
        protected override decimal Descuento => 1m;
        protected override int ViajesGratisPorDia => 2;

        public BoletoEducativo(decimal saldoInicial) : base(saldoInicial) { }
    }

    internal class Jubilados : Tarjeta
    {
        protected override string Franquicia => "Completa";
        protected override decimal Descuento => 1m;
        protected override int ViajesGratisPorDia => 2;

        public Jubilados(decimal saldoInicial) : base(saldoInicial) { }
    }

    internal class MedioEstudiantil : Tarjeta
    {
        protected override string Franquicia => "Parcial";
        protected override decimal Descuento => 0.5m;
        protected override int MinutosEntreViajes => 5;
        public MedioEstudiantil(decimal saldoInicial) : base(saldoInicial) { }
        public MedioEstudiantil(decimal saldoInicial, Tiempo tiempo) : base(saldoInicial, tiempo) { }
    }


    internal class MedioUniversitario : Tarjeta
    {
        protected override string Franquicia => "Parcial";
        protected override decimal Descuento => 0.5m;
        protected override int MinutosEntreViajes => 5;

        public MedioUniversitario(decimal saldoInicial) : base(saldoInicial) { }
        public MedioUniversitario(decimal saldoInicial, Tiempo tiempo) : base(saldoInicial, tiempo) { }
    }

}