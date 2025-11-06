# CodeQuest

CodeQuest es una aplicacion de escritorio para Windows, desarrollada en C# y .NET, que permite administrar y jugar un cuestionario enfocado en conocimiento de desarrollo.
La solucion utiliza patrones de diseno modernos (repositorio, servicios y factory) y una base de datos SQL local.

Tabla de contenido

- Descripcion general
- Tecnologias
- Requisitos
- Instalacion y ejecucion
- Base de datos
- Arquitectura y componentes principales
- Estructura del proyecto
- Capturas de pantalla

Descripcion general

CodeQuest permite gestionar jugadores, administradores, preguntas y rondas, y tocar la logica.

A continuacion se describen los componentes principales y la estructura del proyecto.

Tecnologias

- Lenguaje: C#
- Plataforma: .NET 8.0 para Windows
- Interfaz: Windows Forms
- Almacenamiento: SQL Server LocalDB (archivo CodeQuest.sql)
- Patrones: Repositorio, Servicios y Factory (DI)

Requisitos

- .NET SDK 8.0 o superior (Windows)
- Windows 10/11
- SQL Server LocalDB o SQL Server Express

Instalacion y ejecucion

- Opcion 1: Abrir la solucion en Visual Studio y sincronizar paquetes, luego ejecutar.
- Opcion 2: Siguientes comandos:
  - dotnet restore
  - dotnet build CodeQuest.sln
  - dotnet run --project CodeQuest.csproj

Base de datos

- El esquema se encuentra en [`Database/CodeQuest.sql`](Database/CodeQuest.sql)
- Inicializar la base de datos ejecutando ese archivo en su servidor
- La cadena de conexion se define en [`Database/DbConnection.cs`](Database/DbConnection.cs)

Arquitectura y componentes principales

CodeQuest implementa:

- Repositorios para el acceso a datos: [`Repositories/QuestionRepository.cs`](Repositories/QuestionRepository.cs) y [`Repositories/AdministratorRepository.cs`](Repositories/AdministratorRepository.cs)
- Servicios de negocio: [`Services/GameService.cs`](Services/GameService.cs), [`Services/AdministratorService.cs`](Services/AdministratorService.cs) y [`Services/IGameService.cs`](Services/IGameService.cs)
- Interfaces de repositorio y servicio: [`Repositories/IQuestionRepository.cs`](Repositories/IQuestionRepository.cs), [`Services/IGameService.cs`](Services/IGameService.cs)
- Fábrica de servicios (inyeccion de dependencias): [`Factories/ServiceFactory.cs`](Factories/ServiceFactory.cs)
- Interfaz y logicas de la UI: FormLogin.cs, FormStart.cs, FormQuestionManagement.cs, FormQuestions.cs, FormFinalResults.cs, FormRanking.cs, FormInformation.cs, FormAdminManagement.cs, FormAdminLocker.cs, FormManageOptions.cs
- Punto de entrada y proyecto: [`Program.cs`](Program.cs) y [`CodeQuest.csproj`](CodeQuest.csproj)

Estructura del proyecto

- Carpeta Database: [`Database/CodeQuest.sql`](Database/CodeQuest.sql)
- Carpeta Models: [`Models/Administrator.cs`](Models/Administrator.cs), [`Models/Question.cs`](Models/Question.cs), [`Models/User.cs`](Models/User.cs), [`Models/Round.cs`](Models/Round.cs)
- Carpeta Repositories: [`Repositories/IQuestionRepository.cs`](Repositories/IQuestionRepository.cs), [`Repositories/QuestionRepository.cs`](Repositories/QuestionRepository.cs), [`Repositories/IAdministratorRepository.cs`](Repositories/IAdministratorRepository.cs), [`Repositories/AdministratorRepository.cs`](Repositories/AdministratorRepository.cs)
- Carpeta Services: [`Services/IGameService.cs`](Services/IGameService.cs), [`Services/GameService.cs`](Services/GameService.cs), [`Services/IAdministratorService.cs`](Services/IAdministratorService.cs), [`Services/AdministratorService.cs`](Services/AdministratorService.cs)
- Carpeta Factories: [`Factories/ServiceFactory.cs`](Factories/ServiceFactory.cs)
- Formas de UI: FormLogin.cs, FormStart.cs, FormQuestionManagement.cs, FormQuestions.cs, FormFinalResults.cs, FormRanking.cs, FormInformation.cs, FormAdminManagement.cs, FormAdminLocker.cs, FormManageOptions.cs, FormQuestionManagement.cs
- Archivo de entrada: [`Program.cs`](Program.cs)
- Solucion: [`ProyectoFinalHerramientasII.sln`](ProyectoFinalHerramientasII.sln)


Imágenes inline
Pantalla de inicio de sesión: ![Pantalla de inicio de sesión](Screenshots/login.png) [`Screenshots/login.png`](Screenshots/login.png)
Gestor de administradores: ![Gestor de administradores](Screenshots/gestoradmin.png) [`Screenshots/gestoradmin.png`](Screenshots/gestoradmin.png)
Gestión de preguntas: ![Gestión de preguntas](Screenshots/gestorpreguntas.png) [`Screenshots/gestorpreguntas.png`](Screenshots/gestorpreguntas.png)
Juego en curso: ![Juego en curso](Screenshots/iniciaropcion1.png) [`Screenshots/iniciaropcion1.png`](Screenshots/iniciaropcion1.png)
Resultados finales: ![Resultados finales](Screenshots/resultados.png) [`Screenshots/resultados.png`](Screenshots/resultados.png)
Ronda: ![Ronda](Screenshots/ronda.png) [`Screenshots/ronda.png`](Screenshots/ronda.png)
Esquema de base de datos: ![Esquema DB](Screenshots/scriptDB.png) [`Screenshots/scriptDB.png`](Screenshots/scriptDB.png)
Carga de script: ![Carga de script](Screenshots/cargascript.png) [`Screenshots/cargascript.png`](Screenshots/cargascript.png)
Información: ![Información](Screenshots/informacion.png) [`Screenshots/informacion.png`](Screenshots/informacion.png)
Ranking de usuarios: ![Ranking de usuarios](Screenshots/rankingusuario.png) [`Screenshots/rankingusuario.png`](Screenshots/rankingusuario.png)
Inicio: ![Inicio](Screenshots/inicio.png) [`Screenshots/inicio.png`](Screenshots/inicio.png)
Panel Admin: ![Panel Admin](Screenshots/paneladmin.png) [`Screenshots/paneladmin.png`](Screenshots/paneladmin.png)
Contribucion

- Contribuciones son bienvenidas. Por favor abra un issue y/o haga un pull request.

Licencia

- MIT

Agradecimientos

- Al equipo de desarrollo.