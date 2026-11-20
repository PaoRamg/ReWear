using Microsoft.EntityFrameworkCore;
using Rewear.Data;
using Rewear.Models;

namespace Rewear.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _context;

        // Extensiones permitidas para las fotos
        private readonly string[] extensionesPermitidas =
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };

        // Tamaño máximo: 5 MB
        private const long tamanoMaximo = 5 * 1024 * 1024;

        public UsuarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================================================
        // OBTENER TODOS LOS USUARIOS
        // =========================================================
        public async Task<List<Usuario>> ObtenerTodosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // =========================================================
        // OBTENER USUARIO POR ID
        // =========================================================
        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        // =========================================================
        // OBTENER USUARIO PARA EDITAR
        // =========================================================
        public async Task<Usuario?> ObtenerParaEditarAsync(int id)
        {
            return await _context.Usuarios
                .FindAsync(id);
        }

        // =========================================================
        // VALIDAR SI EL CORREO YA EXISTE
        // =========================================================
        private async Task<bool> CorreoExisteAsync(
            string correo,
            int? idExcluir = null)
        {
            return await _context.Usuarios
                .AnyAsync(u =>
                    u.Correo == correo &&
                    (!idExcluir.HasValue ||
                     u.Id != idExcluir.Value));
        }

        // =========================================================
        // CREAR USUARIO
        // =========================================================
        public async Task<ResultadoOperacion> CrearAsync(
            Usuario usuario,
            IFormFile? fotoPerfil)
        {
            // Regla de negocio:
            // no se permite registrar un correo que ya existe.
            if (await CorreoExisteAsync(usuario.Correo))
            {
                return ResultadoOperacion.Error(
                    "El correo electrónico ya está registrado.",
                    nameof(Usuario.Correo));
            }

            // Datos asignados automáticamente por el sistema
            usuario.FechaRegistro = DateTime.Now;
            usuario.Estado = true;

            // Procesar foto de perfil
            if (fotoPerfil != null && fotoPerfil.Length > 0)
            {
                var resultadoFoto =
                    await GuardarFotoAsync(fotoPerfil);

                if (!resultadoFoto.Exito)
                {
                    return ResultadoOperacion.Error(
                        resultadoFoto.Mensaje!,
                        "fotoPerfil");
                }

                usuario.FotoPerfil = resultadoFoto.Ruta;
            }

            _context.Usuarios.Add(usuario);

            await _context.SaveChangesAsync();

            return ResultadoOperacion.Correcto();
        }

        // =========================================================
        // ACTUALIZAR USUARIO
        // =========================================================
        public async Task<ResultadoOperacion> ActualizarAsync(
            int id,
            Usuario usuario,
            IFormFile? fotoPerfil)
        {
            // Buscar usuario original
            var usuarioExistente =
                await _context.Usuarios.FindAsync(id);

            if (usuarioExistente == null)
            {
                return ResultadoOperacion.Error(
                    "El usuario no existe.");
            }

            // Regla de negocio:
            // el correo no puede pertenecer a otro usuario.
            if (await CorreoExisteAsync(usuario.Correo, id))
            {
                return ResultadoOperacion.Error(
                    "El correo electrónico ya está registrado.",
                    nameof(Usuario.Correo));
            }

            // Actualizar datos permitidos
            usuarioExistente.Nombres = usuario.Nombres;
            usuarioExistente.Apellidos = usuario.Apellidos;
            usuarioExistente.Correo = usuario.Correo;
            usuarioExistente.Telefono = usuario.Telefono;

            // La contraseña NO se modifica aquí.
            // Existe una pantalla independiente para cambiarla.

            // =====================================================
            // ACTUALIZAR FOTO
            // =====================================================

            if (fotoPerfil != null && fotoPerfil.Length > 0)
            {
                var resultadoFoto =
                    await GuardarFotoAsync(fotoPerfil);

                if (!resultadoFoto.Exito)
                {
                    return ResultadoOperacion.Error(
                        resultadoFoto.Mensaje!,
                        "fotoPerfil");
                }

                // Eliminar la foto anterior
                EliminarFoto(usuarioExistente.FotoPerfil);

                // Guardar la ruta de la nueva foto
                usuarioExistente.FotoPerfil =
                    resultadoFoto.Ruta;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExisteAsync(id))
                {
                    return ResultadoOperacion.Error(
                        "El usuario ya no existe.");
                }

                throw;
            }

            return ResultadoOperacion.Correcto();
        }

        // =========================================================
        // ELIMINAR USUARIO
        // =========================================================
        public async Task<bool> EliminarAsync(int id)
        {
            var usuario =
                await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return false;
            }

            // Eliminar foto asociada al usuario
            EliminarFoto(usuario.FotoPerfil);

            _context.Usuarios.Remove(usuario);

            await _context.SaveChangesAsync();

            return true;
        }

        // =========================================================
        // COMPROBAR SI EXISTE UN USUARIO
        // =========================================================
        public async Task<bool> ExisteAsync(int id)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.Id == id);
        }

        // =========================================================
        // CAMBIAR CONTRASEÑA
        // =========================================================
        public async Task<bool> CambiarContrasenaAsync(
            int id,
            string nuevaContrasena)
        {
            var usuario =
                await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return false;
            }

            usuario.Contrasena = nuevaContrasena;

            await _context.SaveChangesAsync();

            return true;
        }

        // =========================================================
        // GUARDAR FOTO
        // =========================================================
        private async Task<(bool Exito, string? Ruta, string? Mensaje)>
            GuardarFotoAsync(IFormFile fotoPerfil)
        {
            // Obtener extensión
            string extension = Path
                .GetExtension(fotoPerfil.FileName)
                .ToLowerInvariant();

            // Validar extensión
            if (!extensionesPermitidas.Contains(extension))
            {
                return (
                    false,
                    null,
                    "Solo se permiten imágenes JPG, JPEG, PNG o WEBP."
                );
            }

            // Validar tamaño
            if (fotoPerfil.Length > tamanoMaximo)
            {
                return (
                    false,
                    null,
                    "La imagen no puede superar los 5 MB."
                );
            }

            // Carpeta de almacenamiento
            string carpeta = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                "perfiles");

            // Crear carpeta si no existe
            if (!Directory.Exists(carpeta))
            {
                Directory.CreateDirectory(carpeta);
            }

            // Generar nombre único
            string nombreArchivo =
                Guid.NewGuid().ToString() + extension;

            string rutaArchivo =
                Path.Combine(carpeta, nombreArchivo);

            // Guardar archivo
            using (var stream = new FileStream(
                rutaArchivo,
                FileMode.Create))
            {
                await fotoPerfil.CopyToAsync(stream);
            }

            // Ruta que se almacenará en la BD
            string rutaWeb =
                "/uploads/perfiles/" + nombreArchivo;

            return (
                true,
                rutaWeb,
                null
            );
        }

        // =========================================================
        // ELIMINAR FOTO
        // =========================================================
        private void EliminarFoto(string? rutaFoto)
        {
            if (string.IsNullOrEmpty(rutaFoto))
            {
                return;
            }

            string rutaCompleta = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                rutaFoto.TrimStart('/')
            );

            if (System.IO.File.Exists(rutaCompleta))
            {
                System.IO.File.Delete(rutaCompleta);
            }
        }
    }
}