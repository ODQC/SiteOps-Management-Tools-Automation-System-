using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SAIH_Backend.Servicios.DTO
{
    [Serializable]
    [DataContract]
    public class EmployeeDTO
    {
        [DataMember]
        private long id;
        [DataMember]
        private string nationalId;
        [DataMember]
        private string firstName;
        [DataMember]
        private string lastName;
        [DataMember]
        private string secondLastName;
        [DataMember]
        private string phone;
        [DataMember]
        private string email;
        [DataMember]
        private string status;
        [DataMember]
        private long fK_idDepartment1;
        [DataMember]
        private long fK_idSite1;
        [DataMember]
        private long fK_idRole1;
        [DataMember]
        private long fK_idDocument1;


        public EmployeeDTO()
        {
            this.nationalId = "";
            this.firstName = "";
            this.lastName = "";
            this.secondLastName = "";
            this.phone = "";
            this.email = "";
            this.status = "";
            this.fK_idDepartment1 = 0;
            this.fK_idSite1 = 0;
            this.fK_idRole1 = 0;
            this.fK_idDocument1 = 0;
        }

        public EmployeeDTO(
             string nationalId,
             string firstName,
             string lastName,
             string secondLastName,
             string phone,
             string email,
             string status,
             int codigoDepartment,
             int codigParque,
             int code,
             long fK_idDocument1
            )
        {

            this.nationalId = nationalId;
            this.firstName = firstName;
            this.lastName = lastName;
            this.secondLastName = secondLastName;
            this.phone = phone;
            this.email = email;
            this.status = status;
            this.fK_idDepartment1 = codigoDepartment;
            this.fK_idSite1 = codigParque;
            this.fK_idRole1 = code;
            this.fK_idDocument1 = fK_idDocument1;
        }

        public long PK_idEmployee
        {
            get { return id; }
            set { id = value; }
        }

        public string NationalId
        {
            get { return nationalId; }
            set { nationalId = value; }
        }
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        public string SecondLastName
        {
            get { return secondLastName; }
            set { secondLastName = value; }
        }
        public string PhoneEmployee
        {
            get { return phone; }
            set { phone = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        public long FK_idDepartment1
        {
            get { return fK_idDepartment1; }
            set { fK_idDepartment1 = value; }
        }
        public long FK_idSite1
        {
            get { return fK_idSite1; }
            set { fK_idSite1 = value; }
        }
        public long FK_idRole1
        {
            get { return fK_idRole1; }
            set { fK_idRole1 = value; }
        }
        public long FK_idDocument1
        {
            get { return fK_idDocument1; }
            set { fK_idDocument1 = value; }
        }
    }
}
