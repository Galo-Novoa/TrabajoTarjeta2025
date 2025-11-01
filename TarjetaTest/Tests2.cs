using NUnit.Framework;
using System;
using TarjetaApp;
using TarjetaApp.Franquicias;

namespace TarjetaTest
{
    internal class Tests2
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
                Assert.That(tarjeta.Saldo, Is.EqualTo(monto));
                Assert.That(colectivo.PagarCon(tarjeta), Is.True);
            });

            Console.WriteLine($"Saldo luego de pagar el pasaje: ${tarjeta.Saldo}.");
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

            Assert.That(tarjeta.Saldo, Is.EqualTo(40000m));
        }

        [Test]
        public void Monto_de_Carga_no_aceptado()
        {
            var tarjeta = new Tarjeta(0m);
            tarjeta.CargarSaldo(5m);

            Assert.That(tarjeta.Saldo, Is.EqualTo(0m));
        }

        [Test]
        public void ViajePlus_Funciona_Hasta_Limite()
        {
            var tarjeta = new Tarjeta(500m);
            var colectivo = new Colectivo("142N");

            Assert.Multiple(() =>
            {
                // Primer viaje plus
                Assert.That(colectivo.PagarCon(tarjeta), Is.True);
                Assert.That(tarjeta.Saldo, Is.EqualTo(-1080m));

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
                Assert.That(tarjeta.Saldo, Is.LessThan(0));
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
                Assert.That(tarjeta.HistorialViajes, Has.Count.EqualTo(1));
                Assert.That(tarjeta.HistorialViajes[0].Linea, Is.EqualTo("142N"));
            });
        }

        [Test]
        public void HistorialViajes_No_Se_Agrega_Si_Falla_Pago()
        {
            var tarjeta = new Tarjeta(0m);
            var colectivo = new Colectivo("142N");

            // Intentar pagar sin saldo y sin viaje plus disponible
            tarjeta.CobrarPasaje(1580m);
            tarjeta.CobrarPasaje(1580m);

            var viajeFallido = colectivo.PagarCon(tarjeta);

            Assert.Multiple(() =>
            {
                Assert.That(viajeFallido, Is.False);
                Assert.That(tarjeta.HistorialViajes, Has.Count.EqualTo(0));
            });
        }

        [Test]
        public void LimiteSaldo_No_Supera_40000()
        {
            var tarjeta = new Tarjeta(39000m);
            tarjeta.CargarSaldo(2000m);

            Assert.That(tarjeta.Saldo, Is.EqualTo(40000m));
        }

        [Test]
        public void Saldo_Inicial_Correcto()
        {
            var tarjeta = new Tarjeta(5000m);
            Assert.That(tarjeta.Saldo, Is.EqualTo(5000m));
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

                Assert.That(completa.Saldo, Is.EqualTo(1000m));
                Assert.That(educativo.Saldo, Is.EqualTo(1000m));
                Assert.That(medio.Saldo, Is.EqualTo(1000m));
            });
        }

        [Test]
        public void Boleto_Creacion_Correcta()
        {
            var tarjeta = new Tarjeta(0m);
            var boleto = new Boleto("142N", tarjeta, Colectivo.PrecioPasajeBase);

            Assert.Multiple(() =>
            {
                Assert.That(boleto.Linea, Is.EqualTo("142N"));
                Assert.That(boleto.Franquicia, Is.EqualTo("Ninguna"));
            });
        }

        [Test]
        public void FranquiciaCompleta_Pasaje_Gratuito()
        {
            var tarjeta = new FranquiciaCompleta(0m);
            var colectivo = new Colectivo("142N");

            Assert.Multiple(() =>
            {
                Assert.That(tarjeta.Franquicia, Is.EqualTo("Franquicia Completa"));
                Assert.That(colectivo.PagarCon(tarjeta), Is.True);
                Assert.That(tarjeta.Saldo, Is.EqualTo(0m));
            });
        }

        [Test]
        public void BoletoEducativo_Pasaje_Gratuito()
        {
            var tarjeta = new BoletoEducativo(0m);
            var colectivo = new Colectivo("142N");

            Assert.Multiple(() =>
            {
                Assert.That(tarjeta.Franquicia, Is.EqualTo("Boleto Educativo Gratuito"));
                Assert.That(colectivo.PagarCon(tarjeta), Is.True);
                Assert.That(tarjeta.Saldo, Is.EqualTo(0m));
            });
        }

        [Test]
        public void MedioBoleto_Medio_Pasaje()
        {
            var tarjeta = new MedioBoleto(1580m);
            var colectivo = new Colectivo("142N");

            Assert.Multiple(() =>
            {
                Assert.That(tarjeta.Franquicia, Is.EqualTo("Medio Boleto Estudiantil"));
                Assert.That(colectivo.PagarCon(tarjeta), Is.True);
                Assert.That(tarjeta.Saldo, Is.EqualTo(790m));
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
                Assert.That(tarjeta.Saldo, Is.EqualTo(420m));
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
                Assert.That(tarjeta.Saldo, Is.EqualTo(0m));
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
                Assert.That(tarjeta.Saldo, Is.EqualTo(790m));
            });
        }
    }
}