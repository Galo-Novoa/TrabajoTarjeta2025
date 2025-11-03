using NUnit.Framework;
using TarjetaApp;

namespace TarjetaTest
{
    internal class TestsLimitacionFranquicia
    {
        [Test]
        public void Franquicia_Tiene_Solo_Dos_Viajes_Gratis()
        {
            var tarjeta = new FranquiciaCompleta(5000m);
            var colectivo = new Colectivo("142N");

            Assert.That(colectivo.PagarCon(tarjeta), Is.True);
            Assert.That(tarjeta.GetSaldo(), Is.EqualTo(5000m));

            Assert.That(colectivo.PagarCon(tarjeta), Is.True);
            Assert.That(tarjeta.GetSaldo(), Is.EqualTo(5000m));

            Assert.That(colectivo.PagarCon(tarjeta), Is.True);
            Assert.That(tarjeta.GetSaldo(), Is.EqualTo(3420m));
        }

        [Test]
        public void BoletoEducativo_Tiene_Solo_Dos_Viajes_Gratis()
        {
            var tarjeta = new BoletoEducativo(5000m);
            var colectivo = new Colectivo("142N");

            Assert.That(colectivo.PagarCon(tarjeta), Is.True);
            Assert.That(tarjeta.GetSaldo(), Is.EqualTo(5000m));

            Assert.That(colectivo.PagarCon(tarjeta), Is.True);
            Assert.That(tarjeta.GetSaldo(), Is.EqualTo(5000m));

            Assert.That(colectivo.PagarCon(tarjeta), Is.True);
            Assert.That(tarjeta.GetSaldo(), Is.EqualTo(3420m));
        }
    }
}