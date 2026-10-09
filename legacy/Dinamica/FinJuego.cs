using EspacioPelea;
using EspacioPersonajes;
using EspacioHistorial;
using EspacioMenuPrincipal;
using MensajesPorPantalla;

namespace FinJuego
{
    public class FinalDeJuego
    {
        public static async Task Final(Personaje Ganador)
        {
            await Task.Delay(500);

            Mensajes.MostrarMensajeGanador(Ganador);
            await Task.Delay(500);
            Console.WriteLine("Sus estadísticas finales son:");
            Mensajes.MostrarPersonaje(Ganador);
            // Añadir al historial de ganadores
            var Historial = HistorialJson.LeerGanadores();
            HistorialJson.CargarDatos(Ganador, Historial);
            Historial = HistorialJson.LeerGanadores();
            
            Console.WriteLine("Presione una tecla para ver el Historial de Ganadores...");
            Console.ReadKey(intercept: true);
            Mensajes.MostrarHistorial(Historial);
            Console.WriteLine("Presione una tecla para volver al Menu Principal...");
            Console.ReadKey(intercept: true);
           

        }
        
    }
}