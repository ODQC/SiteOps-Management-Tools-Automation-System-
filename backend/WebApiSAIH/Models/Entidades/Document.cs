using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendAPI3._1.Models
{
    [Table("documents")]
    public class Document
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long DocumentId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(100)]
        public string FileType { get; set; }

        [MaxLength(100)]
        public string FilePath { get; set; }

        [MaxLength]
        public byte[] DataFiles { get; set; }

        public DateTime? CreatedOn { get; set; }
    }
}
