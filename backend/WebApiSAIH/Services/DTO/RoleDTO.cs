using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SAIH_Backend.Servicios.DTO
{
    [Serializable]
    [DataContract]
    public class RoleDTO
    {
        [DataMember]
        private long idRole;
        [DataMember]
        private string code;
        [DataMember]
        private string name;
        [DataMember]
        private string description;
        [DataMember]
        private string status;

        public RoleDTO()
        {
            this.idRole = 0;
            this.code = "";
            this.name = "";
            this.description = "";
            this.status = "";
        }

        public RoleDTO(long IdRole, string code, string name, string description, string status)
        {
            this.idRole = IdRole;
            this.code = code;
            this.name = name;
            this.description = description;
            this.status = status;
        }

        public long IdRole { get => idRole; set => idRole = value; }
        public string Code { get => code; set => code = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public string Status { get => status; set => status = value; }
    }
}