namespace Rewear.Services
{
    public class ResultadoOperacion
    {
        public bool Exito { get; set; }

        public string? Campo { get; set; }

        public string? Mensaje { get; set; }

        public static ResultadoOperacion Correcto()
        {
            return new ResultadoOperacion
            {
                Exito = true
            };
        }

        public static ResultadoOperacion Error(
            string mensaje,
            string? campo = null)
        {
            return new ResultadoOperacion
            {
                Exito = false,
                Campo = campo,
                Mensaje = mensaje
            };
        }
    }
}