using System;

namespace JuegoListaEnlazada
{
    // Clase Nodo que representa un elemento individual en la lista enlazada
    public class NodoEfecto
    {
        public string NombreEfecto;
        public int DuracionSegundos;
        public NodoEfecto Siguiente;

        public NodoEfecto(string nombre, int duracion)
        {
            NombreEfecto = nombre;
            DuracionSegundos = duracion;
            Siguiente = null;
        }
    }

    // Clase que implementa la estructura de lista enlazada para los power-ups del videojuego
    public class ListaEfectosJuego
    {
        private NodoEfecto cabeza;

        public ListaEfectosJuego()
        {
            cabeza = null;
        }

        // --- OPERACIÓN PRINCIPAL: Inserción al inicio ---
        public void AgregarEfecto(string nombre, int duracion)
        {
            /* 
             * DECISIÓN DE RENDIMIENTO Y MEMORIA:
             * Insertar un nuevo nodo al inicio de la lista enlazada toma un tiempo 
             * constante O(1). A diferencia de los arreglos estáticos o listas dinámicas 
             * basadas en arrays (como List<T>), la lista enlazada no requiere redimensionar 
             * bloques de memoria ni copiar elementos anteriores en RAM, lo cual evita 
             * micro-pausas (stutters) en el ciclo de juego (game loop).
             */
            NodoEfecto nuevoNodo = new NodoEfecto(nombre, duracion);
            nuevoNodo.Siguiente = cabeza;
            cabeza = nuevoNodo;
            Console.WriteLine($"[+] Efecto aplicado al jugador: {nombre} ({duracion}s)");
        }

        // --- OPERACIÓN ADICIONAL 1: Búsqueda de un efecto ---
        public bool BuscarEfecto(string nombre)
        {
            /* 
             * DECISIÓN DE RENDIMIENTO:
             * La búsqueda en una lista enlazada es secuencial con una complejidad O(n).
             * Aunque no es tan rápida como buscar por índice en un arreglo, la estructura 
             * distribuye los nodos de manera dispersa en la memoria RAM según se van creando, 
             * lo que optimiza la asignación dinámica si la cantidad de efectos activos cambia 
             * constantemente durante la partida.
             */
            NodoEfecto actual = cabeza;
            while (actual != null)
            {
                if (actual.NombreEfecto.Equals(nombre, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                actual = actual.Siguiente;
            }
            return false;
        }

        // --- OPERACIÓN ADICIONAL 2: Eliminación de un efecto (cuando expira) ---
        public void EliminarEfecto(string nombre)
        {
            /* 
             * DECISIÓN DE RENDIMIENTO Y MEMORIA:
             * La eliminación de un nodo intermedio o inicial en una lista enlazada 
             * se realiza simplemente reasignando apuntadores en O(n) (debido a la búsqueda). 
             * No se requiere desplazar los elementos restantes en la memoria RAM, a diferencia 
             * de los arreglos tradicionales, haciendo la liberación y actualización muy eficiente 
             * cuando un power-up expira o es disipado.
             */
            if (cabeza == null) return;

            // Si el nodo a eliminar es el primero
            if (cabeza.NombreEfecto.Equals(nombre, StringComparison.OrdinalIgnoreCase))
            {
                cabeza = cabeza.Siguiente;
                Console.WriteLine($"[-] Efecto expirado y removido: {nombre}");
                return;
            }

            // Buscar en el resto de la lista
            NodoEfecto actual = cabeza;
            while (actual.Siguiente != null)
            {
                if (actual.Siguiente.NombreEfecto.Equals(nombre, StringComparison.OrdinalIgnoreCase))
                {
                    actual.Siguiente = actual.Siguiente.Siguiente;
                    Console.WriteLine($"[-] Efecto expirado y removido: {nombre}");
                    return;
                }
                actual = actual.Siguiente;
            }
            Console.WriteLine($"[!] El efecto {nombre} no se encontró en el personaje.");
        }

        // Método auxiliar para mostrar todos los efectos activos en pantalla
        public void MostrarEfectosActivos()
        {
            Console.WriteLine("\n--- ESTADO DE EFECTOS ACTIVOS EN EL JUEGO ---");
            if (cabeza == null)
            {
                Console.WriteLine("El personaje no tiene efectos activos.");
                return;
            }

            NodoEfecto actual = cabeza;
            while (actual != null)
            {
                Console.WriteLine($"- Efecto: {actual.NombreEfecto} | Duración: {actual.DuracionSegundos}s");
                actual = actual.Siguiente;
            }
            Console.WriteLine("---------------------------------------------\n");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== SIMULADOR DE VIDEOJUEGO (SISTEMA DE POWER-UPS) ===\n");
            ListaEfectosJuego inventarioEfectos = new ListaEfectosJuego();

            // 1. Aplicando efectos al jugador (Inserción)
            inventarioEfectos.AgregarEfecto("Super Velocidad", 15);
            inventarioEfectos.AgregarEfecto("Escudo Protector", 10);
            inventarioEfectos.AgregarEfecto("Doble Puntuación", 30);

            // Mostrar estado actual
            inventarioEfectos.MostrarEfectosActivos();

            // 2. Buscando si un efecto específico está activo
            string efectoBuscado = "Escudo Protector";
            Console.WriteLine($"Verificando si el jugador posee '{efectoBuscado}'...");
            if (inventarioEfectos.BuscarEfecto(efectoBuscado))
            {
                Console.WriteLine($"-> ¡Acción permitida! El efecto '{efectoBuscado}' está activo.\n");
            }
            else
            {
                Console.WriteLine($"-> El efecto no está activo.\n");
            }

            // 3. Eliminando un efecto que ha expirado
            inventarioEfectos.EliminarEfecto("Escudo Protector");

            // Mostrar estado final actualizado
            inventarioEfectos.MostrarEfectosActivos();

            Console.WriteLine("Presiona cualquier tecla para finalizar la simulación.");
            Console.ReadKey();
        }
    }
}