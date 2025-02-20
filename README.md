# TSETest
Proyecto para la realización de la prueba de Concepto y Habilidades en Unity

## Funcionalidades

### 1. Solicitar Permisos de Ubicación
- Implementación de un script en C# que solicita permisos de ubicación al usuario al iniciar la aplicación.
- Verificación del estado de los permisos y re-solicitud si no han sido otorgados.
- Mensaje en la UI que indica el estado de los permisos (Otorgado/Rechazado).
- Opción de apertura de configuración de la aplicación en caso de ser rechazados los permisos dos veces.
- Manejo de ventanas en caso de éxito o error
### 3. Ventana Personalizada en el Editor
- Desarrollo de una ventana personalizada en el Editor con un listado de todas las escenas incluidas en Build Settings.
- Funcionalidades:
  - Botón para cargar una escena directamente desde la ventana.
  - Botón para recargar la escena actual.
  - Botón para eliminar una escena de los BuildSettings
  - Agregación automática de escens a Build Settings mediante drag and drop.

### 4. Generación de cubos en la Escena
- Creación de un script `CubeSpawner` con un método `SpawnCube()` para instanciar un prefab de un cubo en la escena en una posición aleatoria dentro de un rango limitado.
- Implementación de Gizmos y Handles para editar el tamaño del area de spawneo.
- Implementación de un Editor Script para CubeSpawner:
  - Botón en la Inspector Window para ejecutar `SpawnCube()` directamente desde el editor.
  - Configuración de propiedades de spawneo (cantidad de cubos a generar mediante un slider, zona de spawneo, color de la zona de spawneo)
- Contador en la UI que muestra la cantidad de cubos generados.
- Creación de un script `CubePool` con el que optimizar la generación de cubos para evitar acumulaciones excesivas. Nota: No se usa Meesh.CombineMeshes porque los materiales son random, pero sería una solución posible.
- Creación de un script `MaterialRandomizer` con un inspector customizado que permite modificar aquellas variables que se quieren randomizar.
- Manejo de problemas relacionados con ejecución en el editor frente a ejecución en runtime
 - Uso de renderer.material o creación de un nuevo material
 - Detección de insconsitencias en la pool de objetos
 - Botón y Slider añadido a la UI del usuario (Canvas)
 - Visualización de la zona de spawn y los correspondientes cubos desde la cámara
