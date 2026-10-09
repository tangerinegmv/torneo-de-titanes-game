using EspacioPersonajes;
using EspacioApi;
using EspacioFabricaDePersonajes;
using EspacioJson;
using EspacioHistorial;
using EspacioAsciiArt;
namespace MensajesPorPantalla
{
    public static class Mensajes
    {
        public static void MostrarIntroduccion()
        {
            Console.Clear();
            Console.WriteLine("========================================================================================================================");
            AsciiArt.Titulo();
            Console.WriteLine("========================================================================================================================");
            Console.WriteLine();
            Console.WriteLine("En las profundidades de la Isla Paradis, un antiguo secreto se esconde entre las sombras.");
            Console.WriteLine("Cada ser nacido en esta tierra posee un poder aterrador: el poder de un Titán.");
            Console.WriteLine("Con el tiempo, estos seres celestiales se enfrentan en un torneo sagrado para determinar al Titán Primordial.");
            Console.WriteLine("Un torneo donde solo el más fuerte, el más astuto y el más valiente se alzará como el gobernante supremo.");
            Console.WriteLine();
            Console.WriteLine("¡Prepárate para forjar tu destino en esta épica batalla por el poder y la gloria!");
            Console.WriteLine("========================================================================================================================");
            Console.WriteLine();
            Console.WriteLine("Presiona cualquier tecla para continuar...");
            Console.ReadKey(intercept: true);
        }
        public static void mostrarPersonajes(List<Personaje> lista)
        {
            // Imprimir encabezados
            Console.WriteLine("================================================================================================================================");
            Console.WriteLine("ID |      Nombre             | Genero  | Edad | Titán        | Velocidad | Destreza | Fuerza | Nivel | Armadura | Salud ");
            Console.WriteLine("================================================================================================================================");

            int id = 0;
            foreach (var personaje in lista)
            {
                // Imprimir cada personaje con formato, incluyendo el tipo de personaje
                Console.WriteLine($"{id,-2} | {personaje.Nombre,-23} | {personaje.Genero,-7} | {personaje.Edad,-4} | {personaje.Tipo,-12} | {personaje.Velocidad,-9} | {personaje.Destreza,-8} | {personaje.Fuerza,-6} | {personaje.Nivel,-5} | {personaje.Armadura,-8} | {personaje.Salud,-5}");
                id++;
            }

            Console.WriteLine("================================================================================================================================");
        }


  

       public static void MostrarHistorial(List<HistorialJson> listaHistorial)
{
    if (listaHistorial == null || listaHistorial.Count == 0)
    {
        Console.WriteLine("No hay registros en el historial.");
        return;
    }

    Console.WriteLine("================================================================");
    Console.WriteLine("            HISTORIAL DE GANADORES                      ");
    Console.WriteLine("================================================================");
    Console.WriteLine("Nombre del Ganador        | Tipo de Titán | Fecha y Hora");
    Console.WriteLine("----------------------------------------------------------------");

    foreach (var registro in listaHistorial)
    {
        Console.WriteLine($"{registro.Ganador.Nombre,-25} | {registro.Ganador.Tipo,-14} | {registro.Hora}");
    }

    Console.WriteLine("================================================================");
}


        internal static void MostrarPersonaje(Personaje personaje)
        {
           
            Console.WriteLine("===================================================");
            Console.WriteLine($"        {personaje.Nombre.ToUpper()}");
            Console.WriteLine("===================================================");
            Console.WriteLine($"     Género:      {personaje.Genero}");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"     Edad:        {personaje.Edad}");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"    Velocidad:   {personaje.Velocidad}");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"    Destreza:    {personaje.Destreza}");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"     Fuerza:      {personaje.Fuerza}");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"     Nivel:       {personaje.Nivel}");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"    Armadura:    {personaje.Armadura}");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"     Salud:       {personaje.Salud}");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"     Titan:      {personaje.Tipo}");
            Console.WriteLine("===================================================");
        }

        public static void MostrarMensajeGanador(Personaje ganador)
        {
            Console.Clear();
            Console.WriteLine("=================================================================================================================================");
            Console.WriteLine("                      ¡VICTORIA ÉPICA!                      ");
            Console.WriteLine("=================================================================================================================================");
            Console.WriteLine($" ¡{ganador.Nombre.ToUpper()} ha ganado el torneo y se convierte en el TITAN PRIMORDIAL. ¡Felicidades!");
            Console.WriteLine("                     ------------------------------------------------------------");
            Console.WriteLine(" Después de una gran batalla, donde la fuerza, la destreza, y el coraje fueron puestos a prueba, un verdadero campeón");
            Console.WriteLine(" ha emergido de entre los titanes. Con cada golpe, con cada movimiento, ha demostrado que solo los más fuertes prevalecen.");
            Console.WriteLine("                    ------------------------------------------------------------");
            Console.WriteLine(" Las tierras temblaron, los cielos se abrieron, y los titanes han sido testigos del poder inigualable de un verdadero guerrero.");
            Console.WriteLine($" ¡El nombre de {ganador.Nombre} resonará por la eternidad!");
            Console.WriteLine("                    ------------------------------------------------------------");
            Console.WriteLine(" Que la leyenda de este día sea contada en canciones, y que los futuros guerreros se arrodillen ante la grandeza.    ");
            Console.WriteLine("=================================================================================================================================");
            Console.WriteLine();
           
        }

          

       

        public static void MostrarMenuPrincipal()
        {
            Console.Clear();
                Console.WriteLine("=======================================");
                Console.WriteLine("       MENÚ PRINCIPAL");
                Console.WriteLine("=======================================");
                Console.WriteLine("1. Empezar el Juego");
                Console.WriteLine("2. Ver Historial de Ganadores");
                Console.WriteLine("3. Salir");
                Console.WriteLine("=======================================");
                Console.Write("Seleccione una opción (1-3): ");
        }
        public static void MostrarMensajeRonda(int ronda)
        {
            Console.WriteLine("=========================================================================================================");
            Console.WriteLine($"                           RONDA {ronda}");
            Console.WriteLine("=========================================================================================================");
        }

        public static void MostrarMensaje(string mensaje)
        {
            Console.WriteLine("========================================================================================================");
            Console.WriteLine($"                {mensaje}");
            Console.WriteLine("========================================================================================================");
        }
        
        
    }
    

  }