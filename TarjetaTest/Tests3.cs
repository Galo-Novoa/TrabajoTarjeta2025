using NUnit.Framework;
using TarjetaApp;

namespace TarjetaTest
{
    internal class Tests3
    {
        [Test]
        public void Carga_Que_Supera_Limite_Guarda_Excedente_Pendiente()
        {
            var tarjeta = new Tarjeta(Tarjeta.SaldoMaximo - 1000m);
            tarjeta.CargarSaldo(3000m);

            Assert.Multiple(() =>
            {
                Assert.That(tarjeta.GetSaldo(), Is.EqualTo(Tarjeta.SaldoMaximo));
                Assert.That(tarjeta.GetSaldoPendiente(), Is.EqualTo(2000m));
            });
        }

        [Test]
        public void Viaje_Acredita_Saldo_Pendiente_Automaticamente()
        {
            var tarjeta = new Tarjeta(Tarjeta.SaldoMaximo);
            tarjeta.CargarSaldo(2000m);

            Assert.Multiple(() =>
            {
                Assert.That(tarjeta.GetSaldoPendiente(), Is.EqualTo(2000m));

                tarjeta.CobrarPasaje();

                Assert.That(tarjeta.GetSaldo(), Is.EqualTo(Tarjeta.SaldoMaximo));
                Assert.That(tarjeta.GetSaldoPendiente(), Is.EqualTo(420m));
            });
        }

        [Test]
        public void Multiples_Cargas_Acumulan_Saldo_Pendiente()
        {
            var tarjeta = new Tarjeta(Tarjeta.SaldoMaximo - 2000m);

            tarjeta.CargarSaldo(3000m);
            tarjeta.CargarSaldo(2000m);

            Assert.Multiple(() =>
            {
                Assert.That(tarjeta.GetSaldo(), Is.EqualTo(Tarjeta.SaldoMaximo));
                Assert.That(tarjeta.GetSaldoPendiente(), Is.EqualTo(3000m));
            });
        }

        [Test]
        public void Carga_Exacta_Al_Limite_No_Genera_Pendiente()
        {
            var tarjeta = new Tarjeta(Tarjeta.SaldoMaximo - 2000m);
            tarjeta.CargarSaldo(2000m);

            Assert.Multiple(() =>
            {
                Assert.That(tarjeta.GetSaldo(), Is.EqualTo(Tarjeta.SaldoMaximo));
                Assert.That(tarjeta.GetSaldoPendiente(), Is.EqualTo(0m));
            });
        }

        [Test]
        public void Viaje_Sin_Saldo_Pendiente_No_Afecta_Acreditacion()
        {
            var tarjeta = new Tarjeta(5000m);
            var viajeExitoso = tarjeta.CobrarPasaje();

            Assert.Multiple(() =>
            {
                Assert.That(viajeExitoso, Is.True);
                Assert.That(tarjeta.GetSaldo(), Is.EqualTo(3420m));
                Assert.That(tarjeta.GetSaldoPendiente(), Is.EqualTo(0m));
            });
        }

        [Test]
        public void Franquicia_Tiene_Solo_Dos_Viajes_Gratis()
        {
            var tarjeta = new FranquiciaCompleta(5000m);
            var colectivo = new Colectivo("142N");

            Assert.Multiple(() =>
            {
                Assert.That(colectivo.PagarCon(tarjeta), Is.True);
                Assert.That(tarjeta.GetSaldo(), Is.EqualTo(5000m));

                Assert.That(colectivo.PagarCon(tarjeta), Is.True);
                Assert.That(tarjeta.GetSaldo(), Is.EqualTo(5000m));

                Assert.That(colectivo.PagarCon(tarjeta), Is.True);
                Assert.That(tarjeta.GetSaldo(), Is.EqualTo(3420m));
            });
        }

        [Test]
        public void BoletoEducativo_Tiene_Solo_Dos_Viajes_Gratis()
        {
            var tarjeta = new BoletoEducativo(5000m);
            var colectivo = new Colectivo("142N");

            Assert.Multiple(() =>
            {
                Assert.That(colectivo.PagarCon(tarjeta), Is.True);
                Assert.That(tarjeta.GetSaldo(), Is.EqualTo(5000m));

                Assert.That(colectivo.PagarCon(tarjeta), Is.True);
                Assert.That(tarjeta.GetSaldo(), Is.EqualTo(5000m));

                Assert.That(colectivo.PagarCon(tarjeta), Is.True);
                Assert.That(tarjeta.GetSaldo(), Is.EqualTo(3420m));
            });
        }
    }
}