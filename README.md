# PROYECTO UNIVERSITARIO
### GestiondeCitasMedicas - Julio 2024

Este proyecto es una aplicación de consola en C# para la gestión de citas y médicos, utilizando estructuras de datos simples como listas enlazadas para un manejo eficiente de la información. El sistema permite a los usuarios interactuar con los datos a través de un menú de opciones, facilitando la administración de un consultorio médico.

#### Características Principales

**Gestión de Citas:**
* **Agendar Citas:** Permite registrar una nueva cita, asignándola a un médico disponible. El sistema valida si el médico existe y si no ha alcanzado el límite diario de 10 citas. Además, calcula automáticamente la próxima hora disponible, con citas que comienzan a las 10:00 AM y duran 45 minutos.
* **Imprimir Cita:** Muestra los detalles de una cita específica, incluyendo información del paciente, el médico asignado, la especialidad, el costo y el estado de la cita (En espera, Atendida, Cancelada).
* **Cancelar y Reprogramar Citas:** Los usuarios pueden cambiar el estado de una cita a "Cancelada" o reprogramarla para una nueva fecha, donde el sistema buscará la próxima hora disponible para el médico.
* **Listar Citas por Estado:** Permite filtrar y mostrar las citas según su estado: "En espera", "Atendida" o "Cancelada", para una fecha específica.
* **Atención de Citas:** Cambia el estado de una cita a "Atendida" una vez que ha sido completada.

**Gestión de Médicos:**
* **CRUD de Médicos:** La aplicación ofrece funcionalidades completas de gestión (Crear, Leer, Actualizar y Eliminar) para los registros de médicos.
* **Listar Médicos:** Permite visualizar una lista de todos los médicos registrados.
* **Listar por Especialidad:** Organiza y muestra a los médicos agrupados por su especialidad, lo que facilita la búsqueda.
* **Cálculo de Ganancias:** Calcula las ganancias totales de un médico o las ganancias obtenidas en una fecha específica, basándose en el costo de las citas que ha atendido.

#### Estructuras de Datos

El programa implementa listas enlazadas (usando `NodoCita` y `NodoMedico`) para almacenar y gestionar los datos de las citas y los médicos. Esta aproximación es ideal para un sistema donde se realizan inserciones y eliminaciones frecuentes, ya que evita la sobrecarga de reasignar memoria que se produce con arreglos estáticos.

#### Clases Principales

* `Cita`: Clase que representa una cita médica con propiedades como número, costo, fecha, datos del paciente y del médico, y estado.
* `Medico`: Clase que define los datos de un médico, incluyendo código, nombre, apellido y especialidad.
* `NodoCita` y `NodoMedico`: Clases auxiliares que forman los nodos de las listas enlazadas para citas y médicos, respectivamente.
* `ListaCitas`: Clase que gestiona la lista enlazada de citas, conteniendo métodos para agregar, buscar, cancelar, reprogramar y mostrar citas.
* `ListaMedicos`: Clase que administra la lista enlazada de médicos, con métodos para agregar, modificar, eliminar, mostrar y calcular ganancias.
* `Program`: La clase principal que contiene el bucle del menú y la lógica de la aplicación.


