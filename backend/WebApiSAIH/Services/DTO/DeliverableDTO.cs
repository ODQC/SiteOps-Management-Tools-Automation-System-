using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebApiSAIH.Services.DTO
{
    [Serializable]
    [DataContract]
    public class DeliverableDTO
    {
        [DataMember]
        private long pK_idDeliverable;

        [DataMember]
        private string code;

        [DataMember]
        private string status;

        [DataMember]
        private long fK_idDocument;

        [DataMember]
        private long fK_idTask1;

        [DataMember]
        private string descripcion;

        [DataMember]
        private string nombre;

        [DataMember]
        private string nombreDocument;

        public DeliverableDTO()
        {
            this.pK_idDeliverable = 0;
            this.code = "";
            this.status = "";
            this.fK_idDocument = 0;
            this.nombreDocument = "";
        }

        public DeliverableDTO(long pK_idDeliverable, string code, string estado, long fK_idDocument1)
        {
            this.pK_idDeliverable = pK_idDeliverable;
            this.code = code;
            this.status = estado;
            this.fK_idDocument = fK_idDocument1;
        }

        public long PK_idDeliverable
        {
            get { return pK_idDeliverable; }
            set { pK_idDeliverable = value; }
        }

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public long FK_idDocument
        {
            get { return fK_idDocument; }
            set { fK_idDocument = value; }
        }

        public long FK_idTask1
        {
            get { return fK_idTask1; }
            set { fK_idTask1 = value; }
        }

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public string DocumentName
        {
            get { return nombreDocument; }
            set { nombreDocument = value; }
        }
    }
}
