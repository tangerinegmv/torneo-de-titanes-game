using EspacioPersonajes;

namespace EspacioAyuda
{
    public class Ayuda
    {
        public static async void IntroduccionJuego()
        {
            Console.Clear();
            Console.WriteLine("=================================================================================================================================");
            Console.WriteLine("                                    TORNEO DE TITANES (AOT AU)                                    ");
            Console.WriteLine("=================================================================================================================================");
            Console.WriteLine(" Bienvenido al Torneo de Titanes, donde solo los más poderosos eldianos lucharán por el título de Titán Primordial. ");
            Console.WriteLine(" --------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(" Cada uno de los 10 combatientes que verás a continuación ha superado pruebas inimaginables, demostrando una fuerza, destreza, ");
            Console.WriteLine(" y voluntad sobrehumanas. Ahora, en este torneo definitivo, deberán demostrar sus habilidades una última vez. ");
            Console.WriteLine(" --------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(" Ha llegado el momento de elegir tu campeón. Cada eldiano en la competencia posee una habilidad única, ya sea en velocidad, fuerza, ");
            Console.WriteLine(" armadura o destreza. Escoge sabiamente, y tú podrías ser el próximo Titán Primordial que reinará en la Isla Paradis.");
            Console.WriteLine(" --------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(" Que este día quede grabado en las leyendas, y que las futuras generaciones canten tus hazañas. ¡El Torneo de Titanes comienza ahora!");
            Console.WriteLine("=================================================================================================================================");
            Console.WriteLine();
            await Task.Delay(4000);
        }

        public static void ExplicacionDePelea(Personaje personaje)
        {
            Console.Clear();
            Console.WriteLine("=================================================================================================================================");
            Console.WriteLine($" Haz elegido a {personaje.Nombre}, un guerrero formidable. {personaje.Nombre} se enfrentará a un rival digno, seleccionado ");
            Console.WriteLine($" especialmente para poner a prueba sus habilidades como Titán {personaje.Tipo}.");
            Console.WriteLine(" --------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(" El torneo consta de 9 rondas intensas, donde los Titanes lucharán hasta el límite de sus fuerzas. Solo el último en pie será ");
            Console.WriteLine(" coronado como el Titán Primordial y se ganará el respeto eterno de todos los eldianos de la Isla Paradis.");
            Console.WriteLine(" --------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine($" Ahora, {personaje.Nombre}, es tu momento de brillar. Que el legado de Ymir te fortalezca y que el poder del Titán {personaje.Tipo} ");
            Console.WriteLine(" supere a todos tus adversarios. La historia se escribe en este combate.");
            Console.WriteLine(" --------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(" Que este día quede grabado en las leyendas, y que las futuras generaciones canten tus hazañas. ¡La batalla está por comenzar!");
            Console.WriteLine("=================================================================================================================================");
            Console.WriteLine();
            Console.WriteLine(" Presiona cualquier tecla para comenzar la batalla...");
            Console.ReadKey(intercept: true);
        }
    }
}
