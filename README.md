# TSETest
 Proyecto para la realización de la prueba de Concepto y Habilidades en Unity

## Objetivo
Evaluar las habilidades del candidato en el desarrollo XR con Unity mediante la creación de un proyecto con requisitos específicos.

## Requisitos del Entorno
- **Unity**: 2019.4.5f1
- **Plataforma de Construcción**: Android
- **Lenguaje de Programación**: C#
- **Modo de Compilación**: IL2CPP

## Tareas Realizadas

### 1. Configuración del Proyecto
- Creación de un nuevo proyecto en Unity para Android.
- Configuración de la API mínima y recomendada para compatibilidad con dispositivos Android modernos.

### 2. Solicitar Permisos de Ubicación
- Implementación de un script en C# que solicita permisos de ubicación al usuario al iniciar la aplicación.
- Verificación del estado de los permisos y re-solicitud si no han sido otorgados.
- Mensaje en la UI que indica el estado de los permisos (Otorgado/Rechazado).

### 3. Crear una Ventana Personalizada en el Editor
- Desarrollo de una ventana personalizada en el Editor.
- Listado de todas las escenas incluidas en Build Settings.
- Botones para:
  - Cargar una escena directamente desde la ventana.
  - Recargar la escena actual.
  - Agregar automáticamente una escena a Build Settings al arrastrar el asset de escena a un campo de Drop de la ventana.

### 4. Generar un Cubo en la Escena mediante un Botón en el Editor
- Creación de un script `CubeSpawner` con un método `SpawnCube()` para instanciar un prefab de un cubo en la escena en una posición aleatoria dentro de un rango limitado.
- Implementación de un Editor Script que añade un botón en la Inspector Window para ejecutar `SpawnCube()` directamente desde el editor.
- Configuración de la cantidad de cubos a generar en cada pulsación del botón usando un slider.

### 5. Validaciones y Optimización
- Implementación de un sistema de logs en consola para depurar errores y mensajes de éxito.

### 6. Tareas Adicionales (Opcionales, pero recomendadas)
- Contador en la UI que muestra la cantidad de cubos generados.
- Opción en el script del Editor para eliminar todos los cubos generados en la escena.
- Creación de un prefab del cubo con materiales aleatorios en cada instancia.
- Optimización de la generación de cubos para evitar acumulaciones excesivas de objetos en la jerarquía.

## Criterios de Evaluación

### Técnicos
- Correcta implementación de permisos de ubicación.
- Ventana personalizada en el Editor con todas las funcionalidades solicitadas.
- Uso correcto de ventanas de editor y customización de características.
- Código limpio, modular y bien comentado.

### Creatividad y Proactividad
- Implementación de funciones adicionales no requeridas.
- Gestión de errores y excepciones en los permisos y generación de objetos.
- Comentarios y limpieza en el código.

## Entrega
El proyecto ha sido entregado en un repositorio de Git con acceso al siguiente enlace: