using NUnit.Framework;
using System;
using TarjetaApp;

namespace TarjetaTest
{
    internal class Tests2
    {
        [Test]
        public void ViajePlus_Funciona_Hasta_Limite()
        {
            var tarjeta = new Tarjeta(500m);
            var colectivo = new Colectivo("142N");

            Assert.Multiple(() =>
            {
                // Primer viaje plus
                Assert.That(colectivo.PagarCon(tarjeta), Is.True);
                Assert.That(tarjeta.GetSaldo(), Is.EqualTo(-1080m));

                // Segundo viaje plus (debería fallar)
                Assert.That(colectivo.PagarCon(tarjeta), Is.False);
            });
        }

        [Test]
        public void ViajePlus_Con_Saldo_Positivo_Previo()
        {
            var tarjeta = new Tarjeta(1000m);
            var colectivo = new Colectivo("142N");

            Assert.Multiple(() =>
            {
                Assert.That(colectivo.PagarCon(tarjeta), Is.True);
                Assert.That(tarjeta.GetSaldo(), Is.LessThan(0));
            });
        }

        [Test]
        public void HistorialViajes_Se_Agrega_Correctamente()
        {
            var tarjeta = new Tarjeta(5000m);
            var colectivo = new Colectivo("142N");

            var viajeExitoso = colectivo.PagarCon(tarjeta);

            Assert.Multiple(() =>
            {
                Assert.That(viajeExitoso, Is.True);
                Assert.That(tarjeta.GetHistorialViajes(), Has.Count.EqualTo(1));
                Assert.That(tarjeta.GetHistorialViajes()[0].GetLinea(), Is.EqualTo("142N"));
            });
        }

        [Test]
        public void HistorialViajes_No_Se_Agrega_Si_Falla_Pago()
        {
            var tarjeta = new Tarjeta(0m);
            var colectivo = new Colectivo("142N");

            // Intentar pagar sin saldo y sin viaje plus disponible
            tarjeta.CobrarPasaje(Colectivo.PrecioPasajeBase);
            tarjeta.CobrarPasaje(Colectivo.PrecioPasajeBase);

            var viajeFallido = colectivo.PagarCon(tarjeta);

            Assert.Multiple(() =>
            {
                Assert.That(viajeFallido, Is.False);
                Assert.That(tarjeta.GetHistorialViajes(), Has.Count.EqualTo(0));
            });
        }

        [Test]
        public void Franquicias_Herencia_Correcta()
        {
            var completa = new Jubilados(1000m);
            var educativo = new BoletoEducativo(1000m);
            var medio = new MedioEstudiantil(1000m);

            Assert.Multiple(() =>
            {
                Assert.That(completa, Is.InstanceOf<Tarjeta>());
                Assert.That(educativo, Is.InstanceOf<Tarjeta>());
                Assert.That(medio, Is.InstanceOf<Tarjeta>());

                Assert.That(completa.GetSaldo(), Is.EqualTo(1000m));
                Assert.That(educativo.GetSaldo(), Is.EqualTo(1000m));
                Assert.That(medio.GetSaldo(), Is.EqualTo(1000m));
            });
        }

        [Test]
        public void Boleto_Creacion_Correcta()
        {
            var tiempo = new TiempoFalso();
            var tarjeta = new Tarjeta(0m);
            var boleto = new Boleto("142N", tarjeta, tiempo);

            Assert.Multiple(() =>
            {
                Assert.That(boleto.GetLinea(), Is.EqualTo("142N"));
                Assert.That(boleto.GetFranquicia(), Is.EqualTo("Ninguna"));
            });
        }

        [Test]
        public void Jubilados_Pasaje_Gratuito()
        {
            var tarjeta = new Jubilados(0m);
            var colectivo = new Colectivo("142N");

            Assert.Multiple(() =>
            {
                Assert.That(tarjeta.GetFranquicia(), Is.EqualTo("Completa"));
                Assert.That(colectivo.PagarCon(tarjeta), Is.True);
                Assert.That(tarjeta.GetSaldo(), Is.EqualTo(0m));
            });
        }

        [Test]
        public void BoletoEducativo_Pasaje_Gratuito()
        {
            var tarjeta = new BoletoEducativo(0m);
            var colectivo = new Colectivo("142N");

            Assert.Multiple(() =>
            {
                Assert.That(tarjeta.GetFranquicia(), Is.EqualTo("Completa"));
                Assert.That(colectivo.PagarCon(tarjeta), Is.True);
                Assert.That(tarjeta.GetSaldo(), Is.EqualTo(0m));
            });
        }

        [Test]
        public void MedioEstudiantil_Medio_Pasaje()
        {
            var tarjeta = new MedioEstudiantil(1580m);
            var colectivo = new Colectivo("142N");

            Assert.Multiple(() =>
            {
                Assert.That(tarjeta.GetFranquicia(), Is.EqualTo("Parcial"));
                Assert.That(colectivo.PagarCon(tarjeta), Is.True);
                Assert.That(tarjeta.GetSaldo(), Is.EqualTo(790m));
            });
        }

        [Test]
        public void CalcularPrecio_Usa_PrecioPasajeBase_Correctamente()
        {
            var tarjeta = new Tarjeta(0m);
            var colectivo = new Colectivo("142N");

            tarjeta.CargarSaldo(2000m);
            var viajeExitoso = colectivo.PagarCon(tarjeta);

            Assert.Multiple(() =>
            {
                Assert.That(viajeExitoso, Is.True);
                Assert.That(tarjeta.GetSaldo(), Is.EqualTo(420m));
            });
        }

        [Test]
        public void CalcularPrecio_Con_Jubilados_Es_Cero()
        {
            var tarjeta = new Jubilados(0m);
            var colectivo = new Colectivo("142N");

            var viajeExitoso = colectivo.PagarCon(tarjeta);

            Assert.Multiple(() =>
            {
                Assert.That(viajeExitoso, Is.True);
                Assert.That(tarjeta.GetSaldo(), Is.EqualTo(0m));
            });
        }

        [Test]
        public void CalcularPrecio_Con_MedioEstudiantil_Es_Mitad()
        {
            var tarjeta = new MedioEstudiantil(1580m);
            var colectivo = new Colectivo("142N");

            var viajeExitoso = colectivo.PagarCon(tarjeta);

            Assert.Multiple(() =>
            {
                Assert.That(viajeExitoso, Is.True);
                Assert.That(tarjeta.GetSaldo(), Is.EqualTo(790m));
            });
        }

		[Test]
		public void Boleto_Todas_Las_Propiedades_Deben_Retornar_Valores_Correctos()
		{
			var tiempo = new TiempoFalso();
			var tarjeta = new Tarjeta(5000m, tiempo);
			var boleto = new Boleto("142N", tarjeta, tiempo);

			Assert.Multiple(() =>
			{
				Assert.That(boleto.GetLinea(), Is.EqualTo("142N"));
				Assert.That(boleto.GetFranquicia(), Is.EqualTo("Ninguna"));
				Assert.That(boleto.GetFecha(), Is.EqualTo(tiempo.Now()));
				Assert.That(boleto.GetMonto(), Is.EqualTo(1580m));
				Assert.That(boleto.GetId(), Is.EqualTo(tarjeta.GetId()));
				Assert.That(boleto.GetSaldo(), Is.EqualTo(5000m + 1580m));
				Assert.That(boleto.GetRestante(), Is.EqualTo(5000m));
			});
		}

		[Test]
		public void MedioUniversitario_Herencia_Y_Propiedades_Correctas()
		{
			var tarjeta = new MedioUniversitario(3000m);

			Assert.Multiple(() =>
			{
				Assert.That(tarjeta, Is.InstanceOf<Tarjeta>());
				Assert.That(tarjeta.GetFranquicia(), Is.EqualTo("Parcial"));
				Assert.That(tarjeta.GetSaldo(), Is.EqualTo(3000m));
			});
		}

		[Test]
		public void MedioUniversitario_Constructor_Con_Tiempo_Funciona()
		{
			var tiempo = new TiempoFalso();
			var tarjeta = new MedioUniversitario(3000m, tiempo);

			Assert.That(tarjeta.GetSaldo(), Is.EqualTo(3000m));
		}

		[Test]
		public void Tarjeta_Base_ViajesGratisPorDia_Es_Cero()
		{
			var tarjeta = new Tarjeta(1000m);

			// Esto prueba que la propiedad virtual base funciona
			// Aunque no se accede directamente, se prueba a través del comportamiento
			var viajeExitoso = tarjeta.CobrarPasaje(Colectivo.PrecioPasajeBase);

			Assert.That(viajeExitoso, Is.True);
			Assert.That(tarjeta.GetSaldo(), Is.EqualTo(1000m - 1580m));
		}
	}
}