using System;
using System.Collections.Generic;
using CodeQuest.Models;
using CodeQuest.Repositories;

namespace CodeQuest.Services
{
    /// <summary>
    /// Servicio de administración que gestiona las operaciones con administradores
    /// Implementa el pilar de Abstracción y usa el patrón Singleton para el repositorio
    /// </summary>
    public class AdministratorService : IAdministratorService
    {
        private readonly IAdministratorRepository administratorRepository;

        /// <summary>
        /// Constructor que inyecta el repositorio de administradores
        /// </summary>
        /// <param name="administratorRepository">Repositorio de administradores</param>
        public AdministratorService(IAdministratorRepository administratorRepository)
        {
            this.administratorRepository = administratorRepository ?? 
                throw new ArgumentNullException(nameof(administratorRepository));
        }

        /// <summary>
        /// Autentica un administrador en el sistema con validaciones
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <param name="password">Contraseña</param>
        /// <returns>Administrador autenticado o null si falla</returns>
        /// <exception cref="ArgumentException">Se lanza cuando los parámetros son inválidos</exception>
        public Administrator Authenticate(string username, string password)
        {
            try
            {
                
                if (string.IsNullOrWhiteSpace(username))
                    throw new ArgumentException("El nombre de usuario no puede estar vacío", nameof(username));
                
                if (string.IsNullOrWhiteSpace(password))
                    throw new ArgumentException("La contraseña no puede estar vacía", nameof(password));

                return administratorRepository.Authenticate(username, password);
            }
            catch (ArgumentException)
            {
                
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al autenticar administrador: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Crea un nuevo administrador con validaciones completas
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <param name="password">Contraseña</param>
        /// <returns>ID del administrador creado</returns>
        /// <exception cref="ArgumentException">Se lanza cuando los parámetros son inválidos</exception>
        /// <exception cref="InvalidOperationException">Se lanza cuando el usuario ya existe</exception>
        public int CreateAdministrator(string username, string password)
        {
            try
            {
                
                if (string.IsNullOrWhiteSpace(username))
                    throw new ArgumentException("El nombre de usuario no puede estar vacío", nameof(username));
                
                if (username.Length < 3)
                    throw new ArgumentException("El nombre de usuario debe tener al menos 3 caracteres", nameof(username));
                
                if (username.Length > 50)
                    throw new ArgumentException("El nombre de usuario no puede exceder 50 caracteres", nameof(username));

                if (string.IsNullOrWhiteSpace(password))
                    throw new ArgumentException("La contraseña no puede estar vacía", nameof(password));
                
                if (password.Length < 4)
                    throw new ArgumentException("La contraseña debe tener al menos 4 caracteres", nameof(password));

              
                if (administratorRepository.AdministratorExists(username))
                    throw new InvalidOperationException("El nombre de usuario ya está en uso");

                return administratorRepository.CreateAdministrator(username, password);
            }
            catch (ArgumentException)
            {
                
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al crear administrador: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene todos los administradores
        /// </summary>
        /// <returns>Lista de administradores</returns>
        /// <exception cref="InvalidOperationException">Se lanza cuando hay problemas de base de datos</exception>
        public List<Administrator> GetAllAdministrators()
        {
            try
            {
                return administratorRepository.GetAllAdministrators();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al obtener administradores: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene un administrador por su ID con validaciones
        /// </summary>
        /// <param name="adminId">ID del administrador</param>
        /// <returns>Administrador encontrado o null</returns>
        /// <exception cref="ArgumentException">Se lanza cuando el ID es inválido</exception>
        /// <exception cref="InvalidOperationException">Se lanza cuando hay problemas de base de datos</exception>
        public Administrator GetAdministratorById(int adminId)
        {
            try
            {
                if (adminId <= 0)
                    throw new ArgumentException("El ID del administrador debe ser mayor a 0", nameof(adminId));

                return administratorRepository.GetAdministratorById(adminId);
            }
            catch (ArgumentException)
            {
        
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al obtener administrador por ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene un administrador por su nombre de usuario con validaciones
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <returns>Administrador encontrado o null</returns>
        /// <exception cref="ArgumentException">Se lanza cuando el username es inválido</exception>
        /// <exception cref="InvalidOperationException">Se lanza cuando hay problemas de base de datos</exception>
        public Administrator GetAdministratorByUsername(string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                    throw new ArgumentException("El nombre de usuario no puede estar vacío", nameof(username));

                return administratorRepository.GetAdministratorByUsername(username);
            }
            catch (ArgumentException)
            {
                
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al obtener administrador por username: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Actualiza la información de un administrador con validaciones
        /// </summary>
        /// <param name="administrator">Administrador con datos actualizados</param>
        /// <returns>True si se actualizó correctamente</returns>
        /// <exception cref="ArgumentNullException">Se lanza cuando el administrador es null</exception>
        /// <exception cref="ArgumentException">Se lanza cuando el administrador es inválido</exception>
        /// <exception cref="InvalidOperationException">Se lanza cuando hay problemas de base de datos</exception>
        public bool UpdateAdministrator(Administrator administrator)
        {
            try
            {
                
                if (administrator == null)
                    throw new ArgumentNullException(nameof(administrator));
                
                if (administrator.AdminID <= 0)
                    throw new ArgumentException("El ID del administrador debe ser mayor a 0", nameof(administrator));

                return administratorRepository.UpdateAdministrator(administrator);
            }
            catch (ArgumentException)
            {
                
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al actualizar administrador: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Actualiza la contraseña de un administrador con validaciones
        /// </summary>
        /// <param name="adminId">ID del administrador</param>
        /// <param name="newPassword">Nueva contraseña</param>
        /// <returns>True si se actualizó correctamente</returns>
        /// <exception cref="ArgumentException">Se lanza cuando los parámetros son inválidos</exception>
        /// <exception cref="InvalidOperationException">Se lanza cuando hay problemas de base de datos</exception>
        public bool UpdatePassword(int adminId, string newPassword)
        {
            try
            {
                
                if (adminId <= 0)
                    throw new ArgumentException("El ID del administrador debe ser mayor a 0", nameof(adminId));
                
                if (string.IsNullOrWhiteSpace(newPassword))
                    throw new ArgumentException("La nueva contraseña no puede estar vacía", nameof(newPassword));
                
                if (newPassword.Length < 4)
                    throw new ArgumentException("La nueva contraseña debe tener al menos 4 caracteres", nameof(newPassword));

                return administratorRepository.UpdatePassword(adminId, newPassword);
            }
            catch (ArgumentException)
            {
               
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al actualizar contraseña: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Elimina un administrador con validaciones
        /// </summary>
        /// <param name="adminId">ID del administrador a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        /// <exception cref="ArgumentException">Se lanza cuando el ID es inválido</exception>
        /// <exception cref="InvalidOperationException">Se lanza cuando hay problemas de base de datos</exception>
        public bool DeleteAdministrator(int adminId)
        {
            try
            {
               
                if (adminId <= 0)
                    throw new ArgumentException("El ID del administrador debe ser mayor a 0", nameof(adminId));

                return administratorRepository.DeleteAdministrator(adminId);
            }
            catch (ArgumentException)
            {
                
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al eliminar administrador: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Verifica si un nombre de usuario ya existe con validaciones
        /// </summary>
        /// <param name="username">Nombre de usuario a verificar</param>
        /// <returns>True si existe, False en caso contrario</returns>
        /// <exception cref="ArgumentException">Se lanza cuando el username es inválido</exception>
        /// <exception cref="InvalidOperationException">Se lanza cuando hay problemas de base de datos</exception>
        public bool AdministratorExists(string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                    throw new ArgumentException("El nombre de usuario no puede estar vacío", nameof(username));

                return administratorRepository.AdministratorExists(username);
            }
            catch (ArgumentException)
            {
              
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al verificar existencia de administrador: {ex.Message}", ex);
            }
        }
    }
}