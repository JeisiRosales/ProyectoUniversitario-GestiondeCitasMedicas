using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

//Clases
public class Cita
{
    public int Numero;
    public double Costo;
    public DateTime Fecha;
    public int Cedula;
    public string Nombre;
    public string Apellido;
    public string CodigoMedico;
    public string Estado;

    public Cita(int numero, double costo, DateTime fecha, int cedula, string nombre, string apellido, string codigoMedico, string estado)
    {
        Numero = numero;
        Costo = costo;
        Fecha = fecha;
        Cedula = cedula;
        Nombre = nombre;
        Apellido = apellido;
        CodigoMedico = codigoMedico;
        Estado = estado;
    }
}
public class Medico
{
    public string Codigo;
    public string Nombre;
    public string Apellido;
    public string Especialidad;

    public Medico(string codigo, string nombre, string apellido, string especialidad)
    {
        Codigo = codigo;
        Nombre = nombre;
        Apellido = apellido;
        Especialidad = especialidad;
    }
}

//Nodos
public class NodoCita
{
    public Cita Datos;
    public NodoCita Siguiente;

    public NodoCita(Cita datos)
    {
        Datos = datos;
        Siguiente = null!;
    }
}
public class NodoMedico
{
    public Medico Datos;
    public NodoMedico Siguiente;

    public NodoMedico(Medico datos)
    {
        Datos = datos;
        Siguiente = null!;
    }
}

//Listas y funciones
public class ListaCitas
{
    public NodoCita Cabeza;
    public int Contador;
    public ListaCitas ()
    {
        Cabeza = null!;
        Contador = 0;
    }
    public void AgregarCita(Cita nuevaCita, ListaMedicos medicos)
    {
        NodoMedico nuevoMedico = medicos.Cabeza;
        bool encontrado = false;
        while(nuevoMedico != null)
        {
            if(nuevoMedico.Datos.Codigo == nuevaCita.CodigoMedico)
            {
                encontrado = true;
                nuevaCita.Numero = Contador;
                NodoCita nuevoNodo = new NodoCita(nuevaCita);
                if (Cabeza == null)
                {
                    Cabeza = nuevoNodo;
                }
                else
                {
                    NodoCita actual = Cabeza;
                    while (actual.Siguiente != null)
                    {
                        actual = actual.Siguiente;
                    }
                    actual.Siguiente = nuevoNodo;
                }
                Contador++;
                break;
            }
            nuevoMedico = nuevoMedico.Siguiente;
        }
        if(!encontrado)
        {
            Console.WriteLine("El codigo del medico no ha sido encontrado.");
        }
    }
    public (bool, string) PuedeAgendarCita(string codigoMedico, DateTime fecha, int cedulaPaciente)
    {
        int citasCount = 0;
        NodoCita actual = Cabeza;

        while (actual != null)
        {
            if (actual.Datos.Fecha.Date == fecha.Date)
            {
                citasCount++;
            }
            if (actual.Datos.Cedula == cedulaPaciente && actual.Datos.CodigoMedico == codigoMedico && actual.Datos.Fecha.Date == fecha.Date)
            {
                return (false, "El paciente ya tiene una cita con este médico en esta fecha");
            }
            actual = actual.Siguiente;
        }

        return (citasCount < 10, "El limite de citas dirarias ha sido alcanzado."); // Permitir un máximo de 10 citas por fecha y médico
    }
    public DateTime ObtenerProximaHora(string codigoMedico, DateTime fecha)
    {
        DateTime inicioCitas = fecha.Date.AddHours(10); // Inicia a las 10:00 AM
        DateTime finCitas = inicioCitas.AddHours(7.5); // Termina a las 5:30 PM (7.5 horas después de las 10:00 AM)
        int citasCount = 0;

        NodoCita actual = Cabeza;
        while (actual != null)
        {
            if (actual.Datos.CodigoMedico == codigoMedico && actual.Datos.Fecha.Date == fecha.Date)
            {
                citasCount++;
            }
            actual = actual.Siguiente;
        }

        DateTime proximaHora = inicioCitas.AddMinutes(45 * citasCount);
        if (proximaHora < finCitas)
        {
            return proximaHora;
        }

        throw new Exception("No se encontró una hora disponible para esa fecha");
    }
    public void MostrarCita(ListaMedicos medicos, ListaCitas citas, int input)
    {
        NodoCita numCita = citas.Cabeza;
        bool citaEncontrada = false;

        while (numCita != null)
        {
            if (numCita.Datos.Numero == input)
            {
                citaEncontrada = true;

                // Buscar el médico asignado a esta cita
                NodoMedico medicoActual = medicos.Cabeza;
                while (medicoActual != null)
                {
                    if (medicoActual.Datos.Codigo == numCita.Datos.CodigoMedico)
                    {
                        if(numCita.Datos.Numero < 10)
                        {
                            Console.WriteLine($"\nCita #:  000{numCita.Datos.Numero}");
                        }
                        else if(numCita.Datos.Numero < 100)
                        {
                            Console.WriteLine($"\nCita #:  00{numCita.Datos.Numero}");
                        }
                        else
                        {
                            Console.WriteLine($"\nCita #:  0{numCita.Datos.Numero}");
                        }

                        Console.WriteLine($"------------------------------------------");
                        Console.WriteLine($"Paciente");
                        Console.WriteLine($"C.I: {numCita.Datos.Cedula}");
                        Console.WriteLine($"Nombre: {numCita.Datos.Nombre}");
                        Console.WriteLine($"Apellido: {numCita.Datos.Apellido}");
                        Console.WriteLine($"\nMédico");
                        Console.WriteLine($"Nombre: {medicoActual.Datos.Nombre}");
                        Console.WriteLine($"Apellido: {medicoActual.Datos.Apellido}");
                        Console.WriteLine($"Especialidad: {medicoActual.Datos.Especialidad}");
                        Console.WriteLine($"Costo: ${numCita.Datos.Costo}");
                        Console.WriteLine($"Fecha: {numCita.Datos.Fecha.ToShortDateString()}");
                        Console.WriteLine($"Hora: {numCita.Datos.Fecha.ToShortTimeString()}");
                        Console.WriteLine($"Estado: {numCita.Datos.Estado}");
                    }
                    medicoActual = medicoActual.Siguiente;
                }
                break;
            }
            numCita = numCita.Siguiente;
        }
        if (!citaEncontrada)
        {
            Console.WriteLine($"Cita no encontrada para el número: {input}");
        }
    }
    public void CancelarCita(int numero)
    {
        NodoCita actual = Cabeza;
        while (actual != null && actual.Datos.Numero != numero)
        {
            actual = actual.Siguiente;
        }
        if (actual != null && actual.Datos.Numero == numero)
        {
            actual.Datos.Estado = "Cancelada";
            Console.WriteLine($"La cita número {numero} ha sido cancelada.");
        }
        else
        {
            Console.WriteLine($"No se encontró la cita con el número {numero}.");
        }
    }
    public void ReprogramarCita(int numero)
    {
        NodoCita actual = Cabeza;
        while (actual != null && actual.Datos.Numero != numero)
        {
            actual = actual.Siguiente;
        }
        if (actual == null)
        {
            Console.WriteLine($"No se encontró la cita con el número {numero}");
            return;
        }

        Console.Write("Ingrese la nueva fecha para la cita (yyyy-mm-dd): ");
        DateTime nuevaFecha;
        bool fechaValida = false;

        while (!fechaValida)
        {
            string inputFecha = Console.ReadLine() ?? "";
            fechaValida = DateTime.TryParseExact(inputFecha, "yyyy-MM-dd", null, DateTimeStyles.None, out nuevaFecha);

            if (!fechaValida)
            {
                Console.WriteLine("Formato de fecha incorrecto. Ingrese la fecha en el formato yyyy-MM-dd");
            }
            else
            {
                try
                {
                    DateTime nuevaHoraDisponible = ObtenerProximaHora(actual.Datos.CodigoMedico, nuevaFecha);
                    actual.Datos.Fecha = nuevaFecha.Date.AddHours(nuevaHoraDisponible.Hour).AddMinutes(nuevaHoraDisponible.Minute);
                    Console.WriteLine($"Cita reprogramada exitosamente para {actual.Datos.Fecha:yyyy-MM-dd HH:mm}.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"No se puede reprogramar la cita. Motivo: {ex.Message}");
                    fechaValida = false;
                }
            }
        }
    }
    public void EstadoCitas(ListaMedicos medicos, ListaCitas citas, DateTime fecha, string dato)
    {
        NodoCita actualCita = citas.Cabeza;
        bool citasEncontradas = false;

        while(actualCita != null)
        {
            if((actualCita.Datos.Estado == dato && actualCita.Datos.Fecha.Date == fecha.Date) ||
               (actualCita.Datos.Estado == dato && actualCita.Datos.Fecha.Date == fecha.Date) ||
               (actualCita.Datos.Estado == dato && actualCita.Datos.Fecha.Date == fecha.Date))
            {
                NodoMedico actualMedico = medicos.Cabeza;
                while (actualMedico != null)
                {
                    if (actualMedico.Datos.Codigo == actualCita.Datos.CodigoMedico)
                    {  
                        if(actualCita.Datos.Numero < 10)
                        {
                            Console.WriteLine($"\nCita #:  000{actualCita.Datos.Numero}");
                        }
                        else if(actualCita.Datos.Numero < 100)
                        {
                            Console.WriteLine($"\nCita #:  00{actualCita.Datos.Numero}");
                        }
                        else
                        {
                            Console.WriteLine($"\nCita #:  0{actualCita.Datos.Numero}");
                        }
                        Console.WriteLine($"------------------------------------------");
                        Console.WriteLine($"Paciente");
                        Console.WriteLine($"C.I: {actualCita.Datos.Cedula}");
                        Console.WriteLine($"Nombre: {actualCita.Datos.Nombre}");
                        Console.WriteLine($"Apellido: {actualCita.Datos.Apellido}");
                        Console.WriteLine($"\nMédico");
                        Console.WriteLine($"Nombre: {actualMedico.Datos.Nombre}");
                        Console.WriteLine($"Apellido: {actualMedico.Datos.Apellido}");
                        Console.WriteLine($"Especialidad: {actualMedico.Datos.Especialidad}");
                        Console.WriteLine($"Costo: ${actualCita.Datos.Costo}");
                        Console.WriteLine($"Fecha: {actualCita.Datos.Fecha.ToShortDateString()}");
                        Console.WriteLine($"Hora: {actualCita.Datos.Fecha.ToShortTimeString()}");
                        Console.WriteLine($"Estado: {actualCita.Datos.Estado}");
                        citasEncontradas = true;
                    }
                    actualMedico = actualMedico.Siguiente;
                }
            }
            actualCita = actualCita.Siguiente;
        }

        if (!citasEncontradas)
        {
            Console.WriteLine($"No se encontraron citas en espera para la fecha {fecha.ToShortDateString()}.");
        }
    }
    public void AtencionCitas(ListaMedicos medicos, ListaCitas citas, int numero)
    {
        NodoCita actualCita = citas.Cabeza;
        bool encontrado = false;
        while(actualCita != null)
        {
            if(actualCita.Datos.Numero == numero)
            {
                actualCita.Datos.Estado = "Atendida";
                Console.WriteLine("La cita fue atendida con exito");
                encontrado = true;
                break;
            }
            actualCita = actualCita.Siguiente;
        }
        if(!encontrado)
        {
            Console.WriteLine("El numero de cita no fue encontrado");
        }
    }
    public static bool MedicoExiste(ListaMedicos listaMedicos, string codigo)
    {
        NodoMedico actual = listaMedicos.Cabeza;
        while (actual != null)
        {
            if (actual.Datos.Codigo == codigo)
            {
                return true;
            }
            actual = actual.Siguiente;
        }
        return false;
    }
}
public class ListaMedicos
{
    public NodoMedico Cabeza;
    public ListaMedicos()
    {
        Cabeza = null!;
    }
    public void AgregarMedico(Medico nuevoMedico)
    {
        NodoMedico nuevoNodo = new NodoMedico(nuevoMedico);
        NodoMedico actualMedico = Cabeza;
        while (actualMedico != null)
        {
            if (actualMedico.Datos.Codigo == nuevoMedico.Codigo)
            {
                Console.WriteLine("No se puede agregar al médico, código existente.");
                return; 
            }
            actualMedico = actualMedico.Siguiente;
        }

        if (Cabeza == null)
        {
            Cabeza = nuevoNodo;
        }
        else
        {
            NodoMedico actual = Cabeza;
            while (actual.Siguiente != null)
            {
                actual = actual.Siguiente;
            }
            actual.Siguiente = nuevoNodo;
            Console.WriteLine("Medico agregado exitosamente.");
        }
    }
    public void ModificarMedico(string codigo)
    {
        NodoMedico nuevoMedico = Cabeza;
        bool encontrado = false;
        while(nuevoMedico != null)
        {
            if(nuevoMedico.Datos.Codigo == codigo)
            {
                Console.Write("Ingrese el nombre modificado del medico: ");
                nuevoMedico.Datos.Nombre = Console.ReadLine() ?? "";
                Console.Write("Ingrese el apellido modificado del medico: ");
                nuevoMedico.Datos.Apellido = Console.ReadLine() ?? "";
                Console.Write("Ingrese la especialidad modificada del médico: ");
                nuevoMedico.Datos.Especialidad = Console.ReadLine() ?? "";
                Console.Write("Medico modificado con exito.\n");
                encontrado = true;
                break;
            }
            nuevoMedico = nuevoMedico.Siguiente;
        }
        if(!encontrado)
        {
            Console.WriteLine("El codigo del medico no ha sido encontrado.");
        }
    }
    public void EliminarMedico(string codigo)
    {
        if (Cabeza == null)
        {
            Console.WriteLine("La lista está vacía.");
            return;
        }

        if (Cabeza.Datos.Codigo == codigo)
        {
            Cabeza = Cabeza.Siguiente;
            Console.WriteLine("Médico eliminado exitosamente.");
            return;
        }

        NodoMedico actual = Cabeza;
        while (actual.Siguiente != null && actual.Siguiente.Datos.Codigo != codigo)
        {
            actual = actual.Siguiente;
        }

        if (actual.Siguiente == null)
        {
            Console.WriteLine("Médico no encontrado.");
        }
        else
        {
            actual.Siguiente = actual.Siguiente.Siguiente;
            Console.WriteLine("Médico eliminado exitosamente.");
        }
    }
    public void MostrarMedicos()
    {
        NodoMedico actual = Cabeza;
        int i = 1;
        while (actual != null)
        {
            Console.WriteLine($"{i}) {actual.Datos.Nombre} {actual.Datos.Apellido}, Especialidad: {actual.Datos.Especialidad}, Codigo: {actual.Datos.Codigo}");
            i++;
            actual = actual.Siguiente;
        }
    }
    public void MostrarEspecialidad()
    {
        NodoMedico actual = Cabeza;
        HashSet<string> especialidadesSet = new HashSet<string>();

        while (actual != null)
        {
            especialidadesSet.Add(actual.Datos.Especialidad);
            actual = actual.Siguiente;
        }

        List<string> especialidades = especialidadesSet.ToList();
        for (int i = 0; i < especialidades.Count; i++)
        {
            Console.WriteLine($"\nEspecialidad {i + 1}: {especialidades[i]}");
            NodoMedico actual2 = Cabeza;
            while(actual2 != null)
            {
                if(actual2.Datos.Especialidad == especialidades[i])
                {
                    Console.WriteLine($"-{actual2.Datos.Nombre} {actual2.Datos.Apellido}, Codigo: {actual2.Datos.Codigo}");
                }
                actual2 = actual2.Siguiente;
            }
        }
    }
    public void CalcularGanacias(string codigo, ListaCitas citas, ListaMedicos medicos, DateTime ? fecha = null)
    {
        NodoCita actual = citas.Cabeza;
        double ganacias = 0;
        bool encontrada = false;

        while(actual != null)
        {
            if(actual.Datos.CodigoMedico == codigo && (fecha == null || actual.Datos.Fecha.Date == fecha.Value.Date))
            {
                encontrada = true;
                ganacias += actual.Datos.Costo;
            }
            actual = actual.Siguiente;
        }
        NodoMedico actualMedico = medicos.Cabeza;
        while (actualMedico != null)
        {
            if (actualMedico.Datos.Codigo == codigo)
            {
                if (encontrada)
                {
                    Console.WriteLine($"El salario de {actualMedico.Datos.Nombre} {actualMedico.Datos.Apellido} es: ${ganacias}");
                }
                else if(!encontrada && actualMedico.Datos.Codigo == codigo)
                {
                    Console.WriteLine($"El salario de {actualMedico.Datos.Nombre} {actualMedico.Datos.Apellido} es: $0");
                }
                break;
            }
            actualMedico = actualMedico.Siguiente;
        }
        if(!encontrada && actualMedico == null)
        {
            Console.WriteLine("Médico no encontrado.");
        }
    }
    public string ValidarEntrada(string mensaje)
    {
        string entrada = "";
        while (string.IsNullOrWhiteSpace(entrada))
        {
            Console.Write(mensaje);
            entrada = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(entrada))
            {
                Console.WriteLine("Este campo no puede estar vacío. Por favor, ingrese un valor.");
            }
        }
        return entrada;
    }
}

//Main
public class Program
{
    public static void Main()
    {
        ListaCitas listaCitas = new ListaCitas();
        ListaMedicos listaMedicos = new ListaMedicos();

        // Agregar médicos iniciales
        listaMedicos.AgregarMedico(new Medico("abc123", "Jose", "Gomez", "Cardiología"));
        listaMedicos.AgregarMedico(new Medico("bca231", "Maria", "Marcano", "Nefrologia"));
        listaMedicos.AgregarMedico(new Medico("cab321", "Luisa", "Marin", "Internista"));
        listaMedicos.AgregarMedico(new Medico("abc321", "Celena", "Sanchez", "Internista"));

        // Agregar citas iniciales
        listaCitas.AgregarCita(new Cita(0, 50, new DateTime(2024, 07, 12, 10, 00, 0), 10555555, "Jose", "Gomez", "abc123", "En espera"), listaMedicos);
        listaCitas.AgregarCita(new Cita(0, 50, new DateTime(2024, 07, 12, 10, 45, 0), 8555555, "Maria", "Lopez", "abc123", "En espera"), listaMedicos);
        listaCitas.AgregarCita(new Cita(0, 30, new DateTime(2024, 07, 14, 10, 00, 0), 11111111, "Luis", "Gonzalez", "bca231", "En espera"), listaMedicos);
        listaCitas.AgregarCita(new Cita(0, 30, new DateTime(2024, 07, 14, 10, 45, 0), 9999999, "Luis", "Hernandez", "bca231", "En espera"), listaMedicos);
        listaCitas.AgregarCita(new Cita(0, 60, new DateTime(2024, 07, 12, 10, 00, 0), 8888888, "Juan", "Gonzalez", "cab321", "En espera"), listaMedicos);
        listaCitas.AgregarCita(new Cita(0, 30, new DateTime(2024, 07, 14, 11, 30, 0), 7777777, "Angela", "Fernandez", "bca231", "En espera"), listaMedicos);
        listaCitas.AgregarCita(new Cita(0, 50, new DateTime(2024, 07, 14, 10, 00, 0), 6666666, "Elena", "Martinez", "abc321", "En espera"), listaMedicos);
        listaCitas.AgregarCita(new Cita(0, 60, new DateTime(2024, 07, 16, 10, 00, 0), 5555555, "Irene", "Marcano", "cab321", "En espera"), listaMedicos);
        listaCitas.AgregarCita(new Cita(0, 30, new DateTime(2024, 07, 14, 11, 30, 0), 4444444, "Juan", "Zapata", "bca231", "En espera"), listaMedicos);
        
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Sistema de gestion de citas medicas:");
            Console.WriteLine("1. Gestionar citas");
            Console.WriteLine("2. Gestionar medicos");
            Console.WriteLine("3. Salir");
            Console.Write("Escoga una opcion: ");
            ConsoleKeyInfo opcion = Console.ReadKey(intercept: true);

            if (opcion.KeyChar == '1')
            {
                while(true)
                {
                    Console.Clear();
                    Console.WriteLine("Gestion de citas");
                    Console.WriteLine("1. Agendar cita");
                    Console.WriteLine("2. Imprimir cita");
                    Console.WriteLine("3. Cancelar cita");
                    Console.WriteLine("4. Reprogramar cita");
                    Console.WriteLine("5. Listar las citas en espera");
                    Console.WriteLine("6. Listar citas atendidas");
                    Console.WriteLine("7. Listar citas canceladas");
                    Console.WriteLine("8. Atención las citas");
                    Console.WriteLine("9. salir");
                    Console.Write("Escoga una opcion: ");
                    ConsoleKeyInfo tecla = Console.ReadKey(intercept: true);

                    if(tecla.KeyChar == '1')
                    {
                        Console.Clear();
                        double costo = 0;
                        while (true)
                        {
                            Console.Write("Ingrese el costo de la cita: ");
                            string costoCita = Console.ReadLine() ?? "";
                            if (string.IsNullOrEmpty(costoCita))
                            {
                                Console.WriteLine("La entrada no puede estar vacía. Por favor, ingrese un valor.");
                                continue;
                            }

                            if (double.TryParse(costoCita, out costo))
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Entrada no válida. Por favor, ingrese un número.");
                            }
                        }
                        Console.Write("Ingrese la fecha de la cita (yyyy-mm-dd): ");
                        DateTime fecha;
                        while (!DateTime.TryParse(Console.ReadLine(), out fecha))
                        {
                            Console.Write("Fecha inválida. Ingrese la fecha de la cita (yyyy-mm-dd): ");
                        }
                        int cedula = 0;
                        while (true)
                        {
                            Console.Write("Ingrese la cédula del paciente: ");
                            string inputCedula = Console.ReadLine() ?? "";

                            if (string.IsNullOrEmpty(inputCedula))
                            {
                                Console.WriteLine("La entrada no puede estar vacía. Por favor, ingrese un valor.");
                                continue;
                            }
                            else if(inputCedula.Length < 6)
                            {
                                Console.WriteLine("La cedula no puede tener menos de 6 digitos");
                            }
                            else if (int.TryParse(inputCedula, out cedula))
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Entrada no válida. Por favor, ingrese un número.");
                            }
                        }
                        
                        string nombre = listaMedicos.ValidarEntrada("Ingrese el nombre del paciente: ");
                        string apellido = listaMedicos.ValidarEntrada("Ingrese el apellido del paciente: ");
                        string codigoMedico = "";
                        while (true)
                        {
                            Console.Write("Ingrese el código del médico: ");
                            codigoMedico = Console.ReadLine() ?? "";
                            if (!Regex.IsMatch(codigoMedico, @"^[A-Za-z]{3}\d{3}$"))
                            {
                                Console.WriteLine($"El código del médico '{codigoMedico}' no es válido. Debe tener 3 letras seguidas de 3 números.");
                            }
                            else if (!ListaCitas.MedicoExiste(listaMedicos, codigoMedico))
                            {
                                Console.WriteLine($"El código del médico '{codigoMedico}' no existe en la lista de médicos.");
                            }
                            else
                            {
                                break;
                            }
                        }           
                        string estado = "En espera";

                        var (puedeAgendar, mensajeError) = listaCitas.PuedeAgendarCita(codigoMedico, fecha, cedula);
                        if (puedeAgendar)
                        {
                            try
                            {
                                DateTime horaCita = listaCitas.ObtenerProximaHora(codigoMedico, fecha);
                                Cita nuevaCita = new Cita(0, costo, horaCita, cedula, nombre, apellido, codigoMedico, estado);
                                listaCitas.AgregarCita(nuevaCita, listaMedicos);
                                Console.WriteLine("Cita agregada exitosamente para el " + horaCita);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        else
                        {
                            Console.WriteLine(mensajeError);
                        }

                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if(tecla.KeyChar == '2')
                    {
                        Console.Clear();
                        int num = 0;
                        while (true)
                        {
                            Console.Write("Ingrese el número de la cita: ");
                            string input = Console.ReadLine() ?? "";
                            if (string.IsNullOrEmpty(input))
                            {
                                Console.WriteLine("La entrada no puede estar vacía. Por favor, ingrese un valor.");
                                continue;
                            }

                            if (int.TryParse(input, out num))
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Entrada no válida. Por favor, ingrese un número.");
                            }
                        }
                        listaCitas.MostrarCita(listaMedicos, listaCitas, num);
                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if(tecla.KeyChar == '3')
                    {
                        Console.Clear();
                        int num = 0;
                        while (true)
                        {
                            Console.Write("Ingrese el número de la cita a cancelar: ");
                            string input = Console.ReadLine() ?? "";
                            if (string.IsNullOrEmpty(input))
                            {
                                Console.WriteLine("La entrada no puede estar vacía. Por favor, ingrese un valor.");
                                continue;
                            }

                            if (int.TryParse(input, out num))
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Entrada no válida. Por favor, ingrese un número.");
                            }
                        }
                        listaCitas.CancelarCita(num);
                        
                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if(tecla.KeyChar == '4')
                    {
                        Console.Clear();
                        int num = 0;
                        while (true)
                        {
                            Console.Write("Ingrese el numero de la cita que desea reprogramar: ");
                            string input = Console.ReadLine() ?? "";
                            if (string.IsNullOrEmpty(input))
                            {
                                Console.WriteLine("La entrada no puede estar vacía. Por favor, ingrese un valor.");
                                continue;
                            }

                            if (int.TryParse(input, out num))
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Entrada no válida. Por favor, ingrese un número.");
                            }
                        }
                        listaCitas.ReprogramarCita(num);

                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if(tecla.KeyChar == '5')
                    {
                        Console.Clear();
                        Console.Write("Ingrese la fecha para buscar las citas 'En espera' (yyyy-MM-dd): ");
                        DateTime fecha;
                        bool fechaValida = DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out fecha);

                        if (fechaValida)
                        {
                            listaCitas.EstadoCitas(listaMedicos, listaCitas, fecha, "En espera");
                        }
                        else
                        {
                            Console.WriteLine("Formato de fecha incorrecto. Intente de nuevo.");
                        }

                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if(tecla.KeyChar == '6')
                    {
                        Console.Clear();
                        Console.Write("Ingrese la fecha para buscar las citas 'Atendidas' (yyyy-MM-dd): ");
                        DateTime fecha;
                        bool fechaValida = DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out fecha);

                        if (fechaValida)
                        {
                            listaCitas.EstadoCitas(listaMedicos, listaCitas, fecha, "Atendida");
                        }
                        else
                        {
                            Console.WriteLine("Formato de fecha incorrecto. Intente de nuevo.");
                        }

                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if(tecla.KeyChar == '7')
                    {
                        Console.Clear();
                        Console.Write("Ingrese la fecha para buscar las citas 'Canceladas' (yyyy-MM-dd): ");
                        DateTime fecha;
                        bool fechaValida = DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out fecha);

                        if (fechaValida)
                        {
                            listaCitas.EstadoCitas(listaMedicos, listaCitas, fecha, "Cancelada");
                        }
                        else
                        {
                            Console.WriteLine("Formato de fecha incorrecto. Intente de nuevo.");
                        }

                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if(tecla.KeyChar == '8')
                    {
                        Console.Clear();
                        int num = 0;
                        while (true)
                        {
                            Console.Write("Ingrese el numero de la cita: ");
                            string input = Console.ReadLine() ?? "";
                            if (string.IsNullOrEmpty(input))
                            {
                                Console.WriteLine("La entrada no puede estar vacía. Por favor, ingrese un valor.");
                                continue;
                            }

                            if (int.TryParse(input, out num))
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Entrada no válida. Por favor, ingrese un número.");
                            }
                        }
                        listaCitas.AtencionCitas(listaMedicos, listaCitas, num);
                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if(tecla.KeyChar == '9')
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Opción no válida, por favor ingrese '1' a '9'");
                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                }
            }
            else if (opcion.KeyChar == '2')
            {
                while(true)
                {
                    Console.Clear();
                    Console.WriteLine("Gestion de medicos");
                    Console.WriteLine("1. Agregar médico");
                    Console.WriteLine("2. Modificar médico");
                    Console.WriteLine("3. Eliminar médico");
                    Console.WriteLine("4. Listar todos los médicos");
                    Console.WriteLine("5. Listar médicos por especialidad");
                    Console.WriteLine("6. Calcular ganancias de un médico");
                    Console.WriteLine("7. salir");
                    Console.Write("Escoga una opcion: ");
                    ConsoleKeyInfo tecla = Console.ReadKey(intercept: true);
    

                    if(tecla.KeyChar == '1')
                    {
                        Console.Clear();
                        string input = "";
                        string nombre = listaMedicos.ValidarEntrada("Ingrese el nombre del medico: ");
                        string apellido = listaMedicos.ValidarEntrada("Ingrese el apellido del medico: ");
                        string especialidad = listaMedicos.ValidarEntrada("Ingrese la especialidad del médico: ");;
                        while(true)
                        {
                            Console.Write("Ingrese el codigo del medico: ");
                            input = Console.ReadLine() ?? "";
                            if (!Regex.IsMatch(input, @"^[A-Za-z]{3}\d{3}$"))
                            {
                                Console.WriteLine($"El código del médico {input} no es válido. Debe tener 3 letras seguidas de 3 números.");
                            }
                            else
                            {
                                break;
                            }
                        }
                        Medico nuevoMedico = new Medico(input, nombre, apellido, especialidad);
                        listaMedicos.AgregarMedico(nuevoMedico);

                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if(tecla.KeyChar == '2')
                    {
                        Console.Clear();
                        Console.Write("Indique el codigo del medico que desea modificar: ");
                        string codigo = Console.ReadLine() ?? "";                      
                        listaMedicos.ModificarMedico(codigo);

                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if(tecla.KeyChar == '3')
                    {
                        Console.Clear();
                        Console.Write("Indique el codigo del medico que desea eliminar: ");
                        string codigo = Console.ReadLine() ?? "";
                        listaMedicos.EliminarMedico(codigo);

                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if(tecla.KeyChar == '4')
                    {
                        Console.Clear();
                        Console.WriteLine("Estos son los medicos registrados:\n");
                        listaMedicos.MostrarMedicos();

                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if(tecla.KeyChar == '5')
                    {
                        Console.Clear();
                        Console.WriteLine("Estos son los mediocs filtrados por especialidad: ");
                        listaMedicos.MostrarEspecialidad();

                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if(tecla.KeyChar == '6')
                    {
                        Console.Clear();
                        while(true)
                        {
                            Console.WriteLine("Calcular la ganancia de un medico: ");
                            Console.WriteLine("1. Calcular la gancia total de un médico");
                            Console.WriteLine("2. Calcular la gancia en un tiempo especifico de un médico");
                            Console.WriteLine("3. salir");
                            Console.Write("Escoga una opcion: ");
                            ConsoleKeyInfo op = Console.ReadKey(intercept: true);
                            if(op.KeyChar == '1')
                            {
                                Console.Clear();
                                Console.Write("Indique el codigo del medico para calcular sus ganancias: ");
                                string codigo = Console.ReadLine() ?? "";
                                listaMedicos.CalcularGanacias(codigo, listaCitas, listaMedicos);
                                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                                Console.ReadKey();
                                break;
                            }
                            else if(op.KeyChar == '2')
                            {
                                Console.Clear();
                                Console.Write("Indique el codigo del medico para calcular sus ganancias: ");
                                string codigo = Console.ReadLine() ?? "";
                                Console.Write("Ingrese la fecha para calcular la ganancia (yyyy-mm-dd): ");
                                DateTime fecha;
                                while (!DateTime.TryParse(Console.ReadLine(), out fecha))
                                {
                                    Console.Write("Fecha inválida. Ingrese la fecha de la cita (yyyy-mm-dd): ");
                                }
                                listaMedicos.CalcularGanacias(codigo, listaCitas, listaMedicos, fecha);
                                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                                Console.ReadKey();
                                break;
                            }
                            else if( op.KeyChar == '3')
                            {
                                break;
                            }
                        }
                    }
                    else if(tecla.KeyChar == '7')
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Opción no válida, por favor ingrese '1' a '7'");
                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                }
            }
            else if (opcion.KeyChar == '3')
            {
                break;
            }
            else
            {
                Console.WriteLine("Opción no válida, por favor ingrese '1', '2' o '3'");
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }
    }
}