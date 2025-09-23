using NUnit.Framework;
using System;
using TarjetaApp;

namespace TarjetaTest
{
    public class Tests
    {

        [TestCase(2000)]
        [TestCase(3000)]
        [TestCase(4000)]
        [TestCase(5000)]
        [TestCase(8000)]
        [TestCase(10000)]
        [TestCase(15000)]
        [TestCase(20000)]
        [TestCase(25000)]
        [TestCase(30000)]
        public void CargarSaldo_Aceptado_AumentaSaldo(decimal monto)
        {
            var tarjeta = new Tarjeta(0m);
            tarjeta.cargarSaldo(monto);
            Assert.That(tarjeta.getSaldo(), Is.EqualTo(monto));
            var colectivo = new Colectivo("122R");
            Assert.That(colectivo.pagarCon(tarjeta), !(Is.EqualTo(null)));
            Console.WriteLine($"Saldo luego de pagar el pasaje: ${tarjeta.getSaldo()}.");
        }
    }
}