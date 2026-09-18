using Microsoft.EntityFrameworkCore;
using Rewear.Data;
using Rewear.Models;

namespace Rewear.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _context;

        private readonly string[] extensionesPermitidas =
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };

        private const long tamanoMaximo = 5 * 1024 * 1024;

        public UsuarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Usuario>> ObtenerTodosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario?> ObtenerParaEditarAsync(int id)
        {
            return await _context.Usuarios
                .FindAsync(id);
        }

        public async Task<Usuario?> CrearAsync(
            Usuario usuario,
            IFormFile? fotoPerfil)
        {
            usuario.FechaRegistro = DateTime.Now;
            usuario.Estado = true;

            if (fotoPerfil != null && fotoPerfil.Length > 0)
            {
                string? rutaFoto = await GuardarFotoAsync(fotoPerfil);

                if (rutaFoto == null)
                    return null;

                usuario.FotoPerfil = rutaFoto;
            }

            _context.Usuarios.Add(usuario);

            await _context.SaveChangesAsync();

            return usuario;
        }

        public async Task<bool> ActualizarAsync(
            int id,
            Usuario usuario,
            IFormFile? fotoPerfil)
        {
            var usuarioExistente = await _context.Usuarios
                .FindAsync(id);

            if (usuarioExistente == null)
                return false;

            usuarioExistente.Nombres = usuario.Nombres;
            usuarioExistente.Apellidos = usuario.Apellidos;
            usuarioExistente.Correo = usuario.Correo;
            usuarioExistente.Telefono = usuario.Telefono;

            if (fotoPerfil != null && fotoPerfil.Length > 0)
            {
                string? nuevaRuta = await GuardarFotoAsync(fotoPerfil);

                if (nuevaRuta == null)
                    return false;

                EliminarFoto(usuarioExistente.FotoPerfil);

                usuarioExistente.FotoPerfil = nuevaRuta;
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var usuario = await _context.Usuarios
                .FindAsync(id);

            if (usuario == null)
                return false;

            EliminarFoto(usuario.FotoPerfil);

            _context.Usuarios.Remove(usuario);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.Id == id);
        }

        public async Task<bool> CambiarContrasenaAsync(
            int id,
            string nuevaContrasena)
        {
            var usuario = await _context.Usuarios
                .FindAsync(id);

            if (usuario == null)
                return false;

            usuario.Contrasena = nuevaContrasena;

            await _context.SaveChangesAsync();

            return true;
        }

        private async Task<string?> GuardarFotoAsync(
            IFormFile fotoPerfil)
        {
            string extension = Path
                .GetExtension(fotoPerfil.FileName)
                .ToLowerInvariant();

            if (!extensionesPermitidas.Contains(extension))
                return null;

            if (fotoPerfil.Length > tamanoMaximo)
                return null;

            string carpeta = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                "perfiles");

            if (!Directory.Exists(carpeta))
            {
                Directory.CreateDirectory(carpeta);
            }

            string nombreArchivo =
                Guid.NewGuid().ToString() + extension;

            string rutaArchivo =
                Path.Combine(carpeta, nombreArchivo);

            using (var stream = new FileStream(
                rutaArchivo,
                FileMode.Create))
            {
                await fotoPerfil.CopyToAsync(stream);
            }

            return "/uploads/perfiles/" + nombreArchivo;
        }

        private void EliminarFoto(string? rutaFoto)
        {
            if (string.IsNullOrEmpty(rutaFoto))
                return;

            string rutaCompleta = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                rutaFoto.TrimStart('/'));

            if (System.IO.File.Exists(rutaCompleta))
            {
                System.IO.File.Delete(rutaCompleta);
            }
        }
    }
}