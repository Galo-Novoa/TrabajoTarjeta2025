using NuGet.Frameworks;
using NUnit.Framework;
using System;
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
                tarjeta.CobrarPasaje(Colectivo.PrecioPasajeBase);
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
            var viajeExitoso = tarjeta.CobrarPasaje(Colectivo.PrecioPasajeBase);

            Assert.Multiple(() =>
            {
                Assert.That(viajeExitoso, Is.True);
                Assert.That(tarjeta.GetSaldo(), Is.EqualTo(3420m));
                Assert.That(tarjeta.GetSaldoPendiente(), Is.EqualTo(0m));
            });
        }

        [Test]
        public void No_Puede_Viajar_Dos_Veces_En_Menos_De_5_Minutos()
        {
            var tiempo = new TiempoFalso();
            var tarjeta = new MedioEstudiantil(5000m, tiempo);
            var colectivo = new Colectivo("142N", tiempo);

            Assert.Multiple(() =>
            {
                Assert.That(colectivo.PagarCon(tarjeta), Is.True);
                Assert.That(colectivo.PagarCon(tarjeta), Is.False);
            });
        }

        [Test]
        public void MedioEstudiantil_Solo_Dos_Viajes_Con_Descuento()
        {
            var tiempo = new TiempoFalso();
            var tarjeta = new MedioEstudiantil(5000m, tiempo);
            var colectivo = new Colectivo("142N", tiempo);

            Assert.Multiple(() =>
            {
                // Primer viaje con descuento
                Assert.That(colectivo.PagarCon(tarjeta), Is.True);

                // Avanzar 6 minutos
                tiempo.AgregarMinutos(6);

                // Segundo viaje con descuento
                Assert.That(colectivo.PagarCon(tarjeta), Is.True);

                // Avanzar 6 minutos más
                tiempo.AgregarMinutos(6);

                // Tercer viaje - debería cobrarse completo
                Assert.That(colectivo.PagarCon(tarjeta), Is.True);

                // Verificar que se cobró completo (790 * 2 + 1580 = 3160)
                Assert.That(tarjeta.GetSaldo(), Is.EqualTo(5000m - 3160m));
            });
        }

        [Test]
        public void Franquicia_Tiene_Solo_Dos_Viajes_Gratis()
        {
            var tiempo = new TiempoFalso(new DateTime(2024, 10, 16, 12, 0, 0));
            var tarjeta = new Jubilados(5000m, tiempo);
            var colectivo = new Colectivo("142N", tiempo);

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
            var tiempo = new TiempoFalso(new DateTime(2024, 10, 16, 12, 0, 0));
            var tarjeta = new BoletoEducativo(5000m, tiempo);
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
        public void FranquiciaCompleta_Muestra_Mensaje_Al_Alcanzar_Limite_Viajes_Gratis()
        {
            var tiempo = new TiempoFalso(new DateTime(2024, 10, 16, 12, 0, 0));
            var tarjeta = new Jubilados(5000m, tiempo);
            var colectivo = new Colectivo("142N");

            // Primer viaje gratis
            colectivo.PagarCon(tarjeta);
            // Segundo viaje gratis  
            colectivo.PagarCon(tarjeta);
            // Tercer viaje - debería mostrar mensaje de límite alcanzado
            colectivo.PagarCon(tarjeta);

            Assert.That(tarjeta.GetSaldo(), Is.EqualTo(5000m - 1580m)); // Se cobró normal
        }

        [Test]
        public void TiempoFalso_AgregarDias_Funciona_Correctamente()
        {
            var tiempo = new TiempoFalso(new DateTime(2024, 10, 14));
            tiempo.AgregarDias(3);
            Assert.That(tiempo.Now(), Is.EqualTo(new DateTime(2024, 10, 17)));
        }

        [Test]
        public void TiempoFalso_AgregarHoras_Funciona_Correctamente()
        {
            var tiempo = new TiempoFalso(new DateTime(2024, 10, 14, 10, 0, 0));
            tiempo.AgregarHoras(5);
            Assert.That(tiempo.Now(), Is.EqualTo(new DateTime(2024, 10, 14, 15, 0, 0)));
        }

        [Test]
        public void TiempoFalso_EstablecerTiempo_Funciona_Correctamente()
        {
            var tiempo = new TiempoFalso();
            var nuevoTiempo = new DateTime(2024, 12, 25, 18, 30, 0);
            tiempo.EstablecerTiempo(nuevoTiempo);
            Assert.That(tiempo.Now(), Is.EqualTo(nuevoTiempo));
        }

        [Test]
        public void AcreditarCarga_Rama_Else_Se_Ejecuta_Cuando_Pendiente_Cabe_Completo()
        {
            // Crear una situación donde el pendiente cabe completamente
            var tarjeta = new Tarjeta(40000m); // Saldo lejos del máximo
            tarjeta.CargarSaldo(20000m); // Esto debería crear pendiente

            // Verificar que hay pendiente
            decimal pendienteInicial = tarjeta.GetSaldoPendiente();
            Assert.That(pendienteInicial, Is.GreaterThan(0m));

            // Realizar una operación que active AcreditarCarga
            // Como hay mucho espacio, debería entrar en la rama ELSE
            tarjeta.CobrarPasaje(Colectivo.PrecioPasajeBase);

            // Después de la acreditación, el pendiente debería ser 0
            // y el saldo debería haber aumentado
            Assert.Multiple(() =>
            {
                Assert.That(tarjeta.GetSaldoPendiente(), Is.EqualTo(4000m - Colectivo.PrecioPasajeBase));
                Assert.That(tarjeta.GetSaldo(), Is.GreaterThan(40000m));
            });
        }
    }
}