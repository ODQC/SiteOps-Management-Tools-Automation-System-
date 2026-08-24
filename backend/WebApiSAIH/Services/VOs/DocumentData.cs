using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiSAIH.Services.VOs
{
    public class DatosDocument
    {
        public MemoryStream Memory { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}
