using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SAIH_Backend.Servicios.DTO
{
    [Serializable]
    [DataContract]
    public partial class SiteDTO
    {
        [DataMember]
        private long pkIdSite;
        [DataMember]
        private string code;
        [DataMember]
        private string name;
        [DataMember]
        private string description;
        [DataMember]
        private string status;
        [DataMember]
        private long fkIdRegion;


        public SiteDTO()
        {
            this.pkIdSite = 0;
            this.code = "";
            this.description = "";
            this.status = "";
            this.name = "";
            this.fkIdRegion = 0;
        }

        public SiteDTO(long pkIdSite, string code, string name, string description, string status, long fkIdRegion)
        {
            this.pkIdSite = pkIdSite;
            this.code = code;
            this.description = description;
            this.status = status;
            this.name = name;
            this.fkIdRegion = fkIdRegion;
        }
        public long PK_IdSite { get => pkIdSite; set => pkIdSite = value; }
        public string Code { get => code; set => code = value; }
        public string Description { get => description; set => description = value; }
        public string Status { get => status; set => status = value; }
        public string Name { get => name; set => name = value; }

        public long FK_idRegion1
        {
            get { return fkIdRegion; }
            set { fkIdRegion = value; }
        }
    }
}