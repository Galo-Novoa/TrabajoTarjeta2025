using NUnit.Framework;
using System;
using TarjetaApp;

namespace TarjetaTest
{
	internal class TestsLimitacionMedioBoleto
	{
		[Test]
		public void No_Puede_Viajar_Dos_Veces_En_Menos_De_5_Minutos()
		{
			var tarjeta = new MedioBoleto(5000m);
			var colectivo = new Colectivo("142N");

			Assert.That(colectivo.PagarCon(tarjeta), Is.True);
			Assert.That(colectivo.PagarCon(tarjeta), Is.False);
		}

		[Test]
		public void Tercer_Viaje_Se_Cobra_Completo()
		{
			var tarjeta = new MedioBoleto(5000m);
			var colectivo = new Colectivo("142N");

			Assert.That(colectivo.PagarCon(tarjeta), Is.True);
			System.Threading.Thread.Sleep(300000);
			Assert.That(colectivo.PagarCon(tarjeta), Is.True);
			System.Threading.Thread.Sleep(300000);
			Assert.That(colectivo.PagarCon(tarjeta), Is.True);
		}
	}
}
