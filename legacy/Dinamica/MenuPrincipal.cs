using System;
using MensajesPorPantalla;
using EspacioJson;
using EspacioPersonajes;
using EspacioHistorial;
using EspacioSeleccionDePersonaje;

namespace EspacioMenuPrincipal
{
    public class MenuPrincipal
    {
        public static async Task MostrarMenu()
        {
            await MostrarMenuPrincipal();
        }
        private static string archivoPersonajes = "Json/Personajes.json";
        private static async Task MostrarMenuPrincipal()
        {
            while (true)
            {
                Mensajes.MostrarMenuPrincipal();
                string opcion = Console.ReadLine();
                switch (opcion)
                {
                    case "1":
                        await ComenzarJuego();
                        break;
                    case "2":
                        VerHistorial();
                        break;
                    case "3":
                        Console.WriteLine("Saliendo del programa...");
                        return;
                    default:
                        Console.WriteLine("Opción no válida. Inténtelo de nuevo.");
                        await Task.Delay(2000);
                        break;
                    
                }
            }
        }

        private static async Task ComenzarJuego()
        {
            List<Personaje> listaPersonajes = new List<Personaje>();
            if (PersonajesJson.Existe(archivoPersonajes))
            {
               listaPersonajes = PersonajesJson.LeerPersonajes(archivoPersonajes);
                if (listaPersonajes.Count < 0)
                {
                    Console.WriteLine("El archivo de personajes está vacío. Creando nuevos personajes...");
                    await CargarDatos.CargandoPersonajes();
                }else
                {
                    listaPersonajes = await PersonajesJson.RecargaDePersonajes(archivoPersonajes);
                }
               
            }else
            {
                Console.WriteLine("No se encontró el archivo de personajes. Creando nuevo archivo...");
                await CargarDatos.CargandoPersonajes();
               listaPersonajes = PersonajesJson.LeerPersonajes(archivoPersonajes);
            }


            await SeleccionDePersonaje.SelectorDePersonajes(listaPersonajes);
            await Task.Delay(1000);
        }

        private static void VerHistorial()
        {
            Console.Clear();
            List<HistorialJson> historial = HistorialJson.LeerGanadores();
            if (historial.Count > 0)
            {
                Mensajes.MostrarHistorial(historial);
            }
            else
            {
                Mensajes.MostrarMensaje("No hay historial de ganadores aun. \nComienza un nuevo juego para empezar a agregar a los ganadores del torneo!");
            }
            Console.WriteLine("Presione una tecla para volver al menú principal...");
            Console.ReadKey(intercept: true);
        }
    }

}