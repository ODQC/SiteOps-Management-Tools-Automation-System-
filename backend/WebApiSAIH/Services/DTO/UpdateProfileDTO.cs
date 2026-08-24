using System.ComponentModel.DataAnnotations;

namespace SAIH_Backend.Servicios.DTO
{
    public class ActualizarPerfilDTO
    {
        [Required]
        public string Phone { get; set; }

        public long? FK_idDocument1 { get; set; }
    }
}
