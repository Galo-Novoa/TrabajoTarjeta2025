using NUnit.Framework;
using System;
using TarjetaApp;

namespace TarjetaTest
{
    public class Tests1
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
        public void Pagar_Colectivo(decimal monto)
        {
            var tarjeta = new Tarjeta(0m);
            var colectivo = new Colectivo("142N");
            var boleto = colectivo.PagarCon(tarjeta);

            Assert.Multiple(() =>
            {
                Assert.That(boleto, Is.False);
                tarjeta.CargarSaldo(monto);
                Assert.That(tarjeta.GetSaldo(), Is.EqualTo(monto));
                Assert.That(colectivo.PagarCon(tarjeta), Is.True);
            });

            Console.WriteLine($"Saldo luego de pagar el pasaje: ${tarjeta.GetSaldo()}.");
        }

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
        public void Pasarse_de_Saldo_al_cargar(decimal monto)
        {
            var tarjeta = new Tarjeta(39000m);
            tarjeta.CargarSaldo(monto);
            Assert.That(tarjeta.GetSaldo(), Is.EqualTo(40000m));
        }

        [Test]
        public void Monto_de_Carga_no_aceptado()
        {
            var tarjeta = new Tarjeta(0m);
            tarjeta.CargarSaldo(5m);
            Assert.That(tarjeta.GetSaldo(), Is.EqualTo(0m));
        }
    }
}