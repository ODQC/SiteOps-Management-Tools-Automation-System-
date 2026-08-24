using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAIH_Backend.Servicios.VO
{
    public class RespuestaGenerica
    {
        private string codigo;
        private string mensaje;
        private Object objectResponse;

        public RespuestaGenerica()
        {
            codigo = "";
            mensaje = "";
            objectResponse = null;
        }

        public RespuestaGenerica(string codigo, string mensaje, Object objectResponse)
        {
            this.codigo = codigo;
            this.mensaje = mensaje;
            this.objectResponse = objectResponse;
        }

        public string Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        public string Mensaje
        {
            get { return mensaje; }
            set { mensaje = value; }
        }

        public Object Object
        {
            get { return objectResponse; }
            set { objectResponse = value; }
        }
    }
}
