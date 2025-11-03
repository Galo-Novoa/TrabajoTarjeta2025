using NUnit.Framework;
using TarjetaApp;

namespace TarjetaTest
{
    internal class TestsLimitacionFranquicia
    {
        [Test]
        public void Franquicia_Tiene_Solo_Dos_Viajes_Gratis()
        {
            var tarjeta = new FranquiciaCompleta(0m);
            var colectivo = new Colectivo("142N");

            Assert.That(colectivo.PagarCon(tarjeta), Is.True);
            Assert.That(colectivo.PagarCon(tarjeta), Is.True);
            Assert.That(colectivo.PagarCon(tarjeta), Is.True);
        }

        [Test]
        public void BoletoEducativo_Tiene_Solo_Dos_Viajes_Gratis()
        {
            var tarjeta = new BoletoEducativo(0m);
            var colectivo = new Colectivo("142N");

            Assert.That(colectivo.PagarCon(tarjeta), Is.True);
            Assert.That(colectivo.PagarCon(tarjeta), Is.True);
            Assert.That(colectivo.PagarCon(tarjeta), Is.True);
        }
    }
}
