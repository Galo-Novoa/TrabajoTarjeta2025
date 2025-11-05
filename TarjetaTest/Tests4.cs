using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TarjetaApp;

namespace TarjetaTest
{
    internal class Tests4
    {
        [Test]
        public void Interurbano_Cobra_3000_Pesos()
        {
            var tarjeta = new Tarjeta(5000m);
            var interurbano = new Interurbano("I1");

            Assert.That(interurbano.PagarCon(tarjeta), Is.True);
            Assert.That(tarjeta.GetSaldo(), Is.EqualTo(2000m)); // 5000 - 3000 = 2000
        }

        [Test]
        public void Interurbano_Con_MedioEstudiantil_Cobra_Mitad()
        {
            var tarjeta = new MedioEstudiantil(5000m);
            var interurbano = new Interurbano("I1");

            Assert.That(interurbano.PagarCon(tarjeta), Is.True);
            Assert.That(tarjeta.GetSaldo(), Is.EqualTo(3500m)); // 5000 - (3000 * 0.5) = 3500
        }

        [Test]
        public void Interurbano_Con_Jubilados_Primeros_Viajes_Gratis()
        {
            var tarjeta = new Jubilados(5000m);
            var interurbano = new Interurbano("I1");

            // Primer viaje gratis
            Assert.That(interurbano.PagarCon(tarjeta), Is.True);
            Assert.That(tarjeta.GetSaldo(), Is.EqualTo(5000m));

            // Segundo viaje gratis  
            Assert.That(interurbano.PagarCon(tarjeta), Is.True);
            Assert.That(tarjeta.GetSaldo(), Is.EqualTo(5000m));

            // Tercer viaje paga completo
            Assert.That(interurbano.PagarCon(tarjeta), Is.True);
            Assert.That(tarjeta.GetSaldo(), Is.EqualTo(2000m)); // 5000 - 3000 = 2000
        }

        [Test]
        public void Boleto_Interurbano_Muestra_Precio_Correcto()
        {
            var tiempo = new TiempoFalso();
            var tarjeta = new Tarjeta(5000m, tiempo);
            var interurbano = new Interurbano("I1", tiempo);

            interurbano.PagarCon(tarjeta);
            var boleto = tarjeta.GetHistorialViajes()[0];

            Assert.That(boleto.GetMonto(), Is.EqualTo(3000m)); // Debe mostrar 3000, no 1580
        }
        [Test]
        public void Boleto_Uso_Frecuente_Solo_Aplica_Tarjetas_Normales()
        {
            var tarjetaNormal = new Tarjeta(10000m);
            var tarjetaJubilado = new Jubilados(10000m);
            var tarjetaEstudiantil = new MedioEstudiantil(10000m);

            // Para tarjeta normal en viaje 35 debería aplicar descuento
            decimal descuentoNormal = tarjetaNormal.CalcularDescuentoUsoFrecuente(35);
            Assert.That(descuentoNormal, Is.EqualTo(0.8m));

            // Para franquicias no debería aplicar
            decimal descuentoJubilado = tarjetaJubilado.CalcularDescuentoUsoFrecuente(35);
            decimal descuentoEstudiantil = tarjetaEstudiantil.CalcularDescuentoUsoFrecuente(35);
            Assert.That(descuentoJubilado, Is.EqualTo(1m));
            Assert.That(descuentoEstudiantil, Is.EqualTo(1m));
        }

        [Test]
        public void Descuento_20_Por_Ciento_Viajes_30_a_59()
        {
            var tarjeta = new Tarjeta(10000m);

            // Viaje 30-59: 20% descuento
            Assert.That(tarjeta.CalcularDescuentoUsoFrecuente(30), Is.EqualTo(0.8m));
            Assert.That(tarjeta.CalcularDescuentoUsoFrecuente(45), Is.EqualTo(0.8m));
            Assert.That(tarjeta.CalcularDescuentoUsoFrecuente(59), Is.EqualTo(0.8m));
        }

        [Test]
        public void Descuento_25_Por_Ciento_Viajes_60_a_80()
        {
            var tarjeta = new Tarjeta(10000m);

            // Viaje 60-80: 25% descuento
            Assert.That(tarjeta.CalcularDescuentoUsoFrecuente(60), Is.EqualTo(0.75m));
            Assert.That(tarjeta.CalcularDescuentoUsoFrecuente(70), Is.EqualTo(0.75m));
            Assert.That(tarjeta.CalcularDescuentoUsoFrecuente(80), Is.EqualTo(0.75m));
        }

        [Test]
        public void Tarifa_Normal_Fuera_Rangos_Descuento()
        {
            var tarjeta = new Tarjeta(10000m);

            // Viajes 1-29: tarifa normal
            Assert.That(tarjeta.CalcularDescuentoUsoFrecuente(1), Is.EqualTo(1m));
            Assert.That(tarjeta.CalcularDescuentoUsoFrecuente(15), Is.EqualTo(1m));
            Assert.That(tarjeta.CalcularDescuentoUsoFrecuente(29), Is.EqualTo(1m));

            // Viajes 81+: tarifa normal
            Assert.That(tarjeta.CalcularDescuentoUsoFrecuente(81), Is.EqualTo(1m));
            Assert.That(tarjeta.CalcularDescuentoUsoFrecuente(100), Is.EqualTo(1m));
        }

        [Test]
        public void Contador_Viajes_Se_Reinicia_Cada_Mes()
        {
            var tiempo = new TiempoFalso(new DateTime(2024, 10, 31)); // 31 de octubre
            var tarjeta = new Tarjeta(10000m, tiempo);
            var colectivo = new Colectivo("142N", tiempo);

            // Realizar viajes en octubre
            for (int i = 0; i < 5; i++)
            {
                colectivo.PagarCon(tarjeta);
            }

            int viajesOctubre = tarjeta.GetNumeroViaje();

            // Cambiar a noviembre
            tiempo.AgregarDias(1); // 1 de noviembre

            // El primer viaje de noviembre debería ser #1
            colectivo.PagarCon(tarjeta);
            Assert.That(tarjeta.GetNumeroViaje(), Is.EqualTo(2)); // 1 (reinicio) + 1 = 2
        }

        [Test]
        public void Viajes_Franquicia_No_Aumentan_Contador_Uso_Frecuente()
        {
            var tiempo = new TiempoFalso();
            var tarjetaJubilado = new Jubilados(10000m);
            var colectivo = new Colectivo("142N", tiempo);

            int viajeInicial = tarjetaJubilado.GetNumeroViaje();

            // Viajes de jubilado no deberían incrementar el contador
            colectivo.PagarCon(tarjetaJubilado);
            colectivo.PagarCon(tarjetaJubilado);

            Assert.That(tarjetaJubilado.GetNumeroViaje(), Is.EqualTo(viajeInicial));
        }

        [Test]
        public void Simulacion_Completa_Mes_Con_Descuentos_Progresivos()
        {
            var tiempo = new TiempoFalso(new DateTime(2024, 10, 1));

            // CALCULAR SALDO SUFICIENTE PARA 85 VIAJES
            decimal costoTotalEsperado = CalcularCostoTotal85Viajes();
            var tarjeta = new Tarjeta(costoTotalEsperado + 1000m, tiempo); // Saldo extra para margen

            var colectivo = new Colectivo("142N", tiempo);

            decimal saldoInicial = tarjeta.GetSaldo();
            decimal costoAcumulado = 0m;

            Console.WriteLine($"Saldo inicial: {saldoInicial}");
            Console.WriteLine($"Costo total esperado: {costoTotalEsperado}");

            // Realizar 85 viajes en el mes
            for (int i = 1; i <= 85; i++)
            {
                int numeroViajeAntes = tarjeta.GetNumeroViaje();

                bool viajeExitoso = colectivo.PagarCon(tarjeta);
                Assert.That(viajeExitoso, Is.True, $"Viaje {i} falló - Saldo: {tarjeta.GetSaldo()}");

                // Calcular costo esperado según el número de viaje que se ACABA de realizar
                decimal precioBase = 1580m;
                decimal descuento = numeroViajeAntes switch
                {
                    <= 29 => 1m,      // 0% descuento (viajes 1-29)
                    <= 59 => 0.8m,    // 20% descuento (viajes 30-59)
                    <= 80 => 0.75m,   // 25% descuento (viajes 60-80)
                    _ => 1m           // 0% descuento (viajes 81+)
                };

                decimal costoViaje = precioBase * descuento;
                costoAcumulado += costoViaje;

                Console.WriteLine($"Viaje {i}: #{numeroViajeAntes} - ${costoViaje} - Saldo restante: {tarjeta.GetSaldo()}");
            }

            // Verificar saldo final
            decimal saldoEsperado = saldoInicial - costoAcumulado;
            decimal saldoReal = tarjeta.GetSaldo();

            Console.WriteLine($"RESUMEN:");
            Console.WriteLine($"Saldo inicial: {saldoInicial}");
            Console.WriteLine($"Costo acumulado: {costoAcumulado}");
            Console.WriteLine($"Saldo esperado: {saldoEsperado}");
            Console.WriteLine($"Saldo real: {saldoReal}");

            Assert.That(saldoReal, Is.EqualTo(saldoEsperado).Within(0.01m));
        }

        // Método auxiliar para calcular el costo total de 85 viajes
        private decimal CalcularCostoTotal85Viajes()
        {
            decimal costo = 0m;
            decimal precioBase = 1580m;

            for (int i = 1; i <= 85; i++)
            {
                decimal descuento = i switch
                {
                    <= 29 => 1m,      // 0% descuento
                    <= 59 => 0.8m,    // 20% descuento
                    <= 80 => 0.75m,   // 25% descuento
                    _ => 1m           // 0% descuento
                };
                costo += precioBase * descuento;
            }

            return costo;
        }
    }
}
