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
            tarjeta.CobrarPasaje();
            tarjeta.CobrarPasaje();

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
            var completa = new FranquiciaCompleta(1000m);
            var educativo = new BoletoEducativo(1000m);
            var medio = new MedioBoleto(1000m);

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
        public void FranquiciaCompleta_Pasaje_Gratuito()
        {
            var tarjeta = new FranquiciaCompleta(0m);
            var colectivo = new Colectivo("142N");

            Assert.Multiple(() =>
            {
                Assert.That(tarjeta.GetFranquicia(), Is.EqualTo("Franquicia Completa"));
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
                Assert.That(tarjeta.GetFranquicia(), Is.EqualTo("Boleto Educativo Gratuito"));
                Assert.That(colectivo.PagarCon(tarjeta), Is.True);
                Assert.That(tarjeta.GetSaldo(), Is.EqualTo(0m));
            });
        }

        [Test]
        public void MedioBoleto_Medio_Pasaje()
        {
            var tarjeta = new MedioBoleto(1580m);
            var colectivo = new Colectivo("142N");

            Assert.Multiple(() =>
            {
                Assert.That(tarjeta.GetFranquicia(), Is.EqualTo("Medio Boleto Estudiantil"));
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
        public void CalcularPrecio_Con_FranquiciaCompleta_Es_Cero()
        {
            var tarjeta = new FranquiciaCompleta(0m);
            var colectivo = new Colectivo("142N");

            var viajeExitoso = colectivo.PagarCon(tarjeta);

            Assert.Multiple(() =>
            {
                Assert.That(viajeExitoso, Is.True);
                Assert.That(tarjeta.GetSaldo(), Is.EqualTo(0m));
            });
        }

        [Test]
        public void CalcularPrecio_Con_MedioBoleto_Es_Mitad()
        {
            var tarjeta = new MedioBoleto(1580m);
            var colectivo = new Colectivo("142N");

            var viajeExitoso = colectivo.PagarCon(tarjeta);

            Assert.Multiple(() =>
            {
                Assert.That(viajeExitoso, Is.True);
                Assert.That(tarjeta.GetSaldo(), Is.EqualTo(790m));
            });
        }
    }
}