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
            var tiempo = new TiempoFalso(new DateTime(2024, 10, 16, 12, 0, 0)); // Miércoles 12:00
            var tarjeta = new Tarjeta(0m, tiempo);
            var colectivo = new Colectivo("142N", tiempo);
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

        [Test]
        public void Monto_de_Carga_no_aceptado()
        {
            var tiempo = new TiempoFalso(new DateTime(2024, 10, 16, 12, 0, 0));
            var tarjeta = new Tarjeta(0m, tiempo);
            tarjeta.CargarSaldo(5m);
            Assert.That(tarjeta.GetSaldo(), Is.EqualTo(0m));
        }

        [Test]
        public void Saldo_Inicial_Correcto()
        {
            var tiempo = new TiempoFalso(new DateTime(2024, 10, 16, 12, 0, 0));
            var tarjeta = new Tarjeta(5000m, tiempo);
            Assert.That(tarjeta.GetSaldo(), Is.EqualTo(5000m));
        }

        [Test]
        public void LimiteSaldo_No_Supera_SaldoMaximo()
        {
            var tiempo = new TiempoFalso(new DateTime(2024, 10, 16, 12, 0, 0));
            var tarjeta = new Tarjeta(Tarjeta.SaldoMaximo - 1000m, tiempo);
            tarjeta.CargarSaldo(2000m);
            Assert.That(tarjeta.GetSaldo(), Is.EqualTo(Tarjeta.SaldoMaximo));
        }
    }
}