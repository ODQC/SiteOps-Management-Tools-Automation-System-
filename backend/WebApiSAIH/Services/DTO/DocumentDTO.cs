using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebApiSAIH.Services.DTO
{
    [Serializable]
    [DataContract]
    public class DocumentDTO
    {
        [DataMember]
        public long DocumentId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string FileType { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember]
        public byte[] DataFiles { get; set; }

        [DataMember]
        public DateTime? CreatedOn { get; set; }
    }
}
