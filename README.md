# CodeQuest - Juego de Programación en C#

## Descripción
CodeQuest es un juego educativo desarrollado en Windows Forms que permite a los jugadores responder preguntas de programación en C# y ganar XP por respuestas correctas. El juego incluye un sistema de base de datos para almacenar usuarios, preguntas, respuestas y puntuaciones.

## Características Principales

### Sistema de Juego
- **3 rondas por partida** con dificultad progresiva (1, 2, 3)
- **3 preguntas por ronda** seleccionadas aleatoriamente según la dificultad
- **Sistema de XP**: 10 XP por respuesta correcta + bonificación por tiempo
- **Bonificación por velocidad**:
  - ≤30 segundos: +20 XP
  - ≤60 segundos: +10 XP
  - >60 segundos: +0 XP

### Validaciones Implementadas
- **Nombre de usuario**: Requerido, longitud mínima 3 caracteres, solo letras, números y guión bajo
- **Respuestas**: Validación de selección obligatoria antes de continuar
- **Conexión a base de datos**: Manejo de errores y validaciones

### Formularios del Juego
1. **FormInicio**: Solicita nombre de usuario
2. **FormRonda**: Muestra información de la ronda antes de comenzar
3. **FormPreguntas**: Presenta las preguntas con opciones múltiples
4. **FormResultadoRonda**: Muestra resultados de cada ronda
5. **FormResultadosFinales**: Estadísticas finales del juego
6. **FormRanking**: Top 10 mejores jugadores

## Configuración de Base de Datos

### Requisitos
- SQL Server Express (DESKTOP-FN66L1D\SQLEXPRESS)
- Base de datos: CodeQuest

### Instalación
1. Ejecutar el script `Database/querysinlosadd.sql` para crear la estructura de la base de datos
2. Ejecutar el script `Database/AddedQuestions.sql` para insertar las preguntas de ejemplo

### Estructura de la Base de Datos
- **Users**: Almacena información de usuarios (ID, nombre, XP, nivel)
- **Questions**: Preguntas con diferentes niveles de dificultad
- **Choices**: Opciones de respuesta para cada pregunta
- **Rounds**: Rondas de juego por usuario
- **RoundAnswers**: Respuestas específicas de cada ronda

## Cómo Ejecutar

### Prerrequisitos
- .NET 6.0 o superior
- Visual Studio 2022 o Visual Studio Code
- SQL Server Express

### Pasos
1. Clonar o descargar el proyecto
2. Configurar la base de datos ejecutando los scripts SQL
3. Verificar la cadena de conexión en `DatabaseHelper.cs`
4. Compilar y ejecutar el proyecto

```bash
dotnet build
dotnet run
```

## Flujo del Juego

1. **Inicio**: El jugador ingresa su nombre de usuario
2. **Preparación**: Se muestra información sobre la ronda actual
3. **Preguntas**: El jugador responde 3 preguntas con tiempo cronometrado
4. **Resultados de Ronda**: Se muestran los resultados y XP ganado
5. **Siguiente Ronda**: Se repite para las 3 rondas (dificultad 1, 2, 3)
6. **Resultados Finales**: Estadísticas completas de la sesión
7. **Ranking**: Opción de ver el top 10 de jugadores

## Fórmula de XP

```
XP Total = (Respuestas Correctas × 10) + Bonificación por Tiempo

Bonificación por Tiempo:
- Tiempo ≤ 30 segundos: +20 XP
- Tiempo ≤ 60 segundos: +10 XP  
- Tiempo > 60 segundos: +0 XP
```

## Tecnologías Utilizadas
- **Frontend**: Windows Forms (.NET 6.0)
- **Backend**: C#
- **Base de Datos**: SQL Server Express
- **ORM**: ADO.NET con SqlClient

## Estructura del Proyecto
```
CodeQuest/
├── Database/
│   ├── querysinlosadd.sql      # Estructura de BD
│   └── AddedQuestions.sql      # Preguntas de ejemplo
├── Program.cs                  # Punto de entrada
├── DatabaseHelper.cs           # Conexión y operaciones básicas de BD
├── GameHelper.cs              # Lógica del juego y operaciones complejas
├── FormInicio.cs              # Formulario de inicio
├── FormRonda.cs               # Formulario de preparación de ronda
├── FormPreguntas.cs           # Formulario de preguntas
├── FormResultadoRonda.cs      # Formulario de resultados por ronda
├── FormResultadosFinales.cs   # Formulario de resultados finales
├── FormRanking.cs             # Formulario de ranking
└── CodeQuest.csproj           # Archivo de proyecto
```

## Características Técnicas
- **Validación de entrada**: Previene caracteres inválidos y entradas vacías
- **Manejo de errores**: Try-catch en operaciones de base de datos
- **Interfaz intuitiva**: Diseño limpio con colores y fuentes apropiadas
- **Navegación fluida**: Transiciones suaves entre formularios
- **Persistencia de datos**: Almacenamiento completo en base de datos

¡Disfruta jugando CodeQuest y mejora tus conocimientos de programación en C#!