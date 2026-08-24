using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SAIH_Backend.Servicios.DTO
{
    [Serializable]
    [DataContract]
    public partial class RegionDTO
    {
        [DataMember]
        private long pkIdRegion;
        [DataMember]
        private string code;
        [DataMember]
        private string name;
        [DataMember]
        private string description;
        [DataMember]
        private string status;


        public RegionDTO()
        {
            this.pkIdRegion = 0;
            this.code = "";
            this.description = "";
            this.status = "";
            this.name = "";
        }

        public RegionDTO(long pkIdRegion, string code, string name, string description, string status)
        {
            this.pkIdRegion = pkIdRegion;
            this.code = code;
            this.description = description;
            this.status = status;
            this.name = name;
        }
        public long PK_IdRegion { get => pkIdRegion; set => pkIdRegion = value; }
        public string Code { get => code; set => code = value; }
        public string Description { get => description; set => description = value; }
        public string Status { get => status; set => status = value; }
        public string Name { get => name; set => name = value; }
    }
}
