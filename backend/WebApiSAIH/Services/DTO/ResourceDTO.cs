using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using WebApiSAIH.Models.Entidades;

namespace WebApiSAIH.Services.DTO
{
    [Serializable]
    [DataContract]
    public class ResourceDTO
    {
        [DataMember]
        private long pK_idResource;

        [DataMember]
        private String code;

        [DataMember]
        private String type;

        [DataMember]
        private String description;

        [DataMember]
        private String status;

        public ResourceDTO()
        {
            this.pK_idResource = 0;
            this.code = "";
            this.type = "";
            this.description = "";
            this.status = "";

        }

        public ResourceDTO(long pK_idResource, string code, string type, string description, string status)
        {
            this.pK_idResource = pK_idResource;
            this.code = code;
            this.type = type;
            this.description = description;
            this.status = status;
        }

        public long PK_idResource { get => pK_idResource; set => pK_idResource = value; }
        public string Code { get => code; set => code = value; }
        public string Type { get => type; set => type = value; }
        public string Description { get => description; set => description = value; }
        public string Status { get => status; set => status = value; }
    }
}
