using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAIH_Backend.Servicios.Errores
{
    public class Error
    {
        public static string EMAIL_ALREADY_REGISTERED = "This email is already in use";
        public static string REGION_ALREADY_REGISTERED = "This Region code is already in use";
        public static string DEPARTMENT_ALREADY_REGISTERED = "This Department code is already in use";
        public static string PROJECT_ALREADY_REGISTERED = "This Project code is already in use";
        public static string DELIVERABLE_ALREADY_REGISTERED = "This Deliverable code is already in use";
        public static string TASK_ALREADY_REGISTERED = "This Task code is already in use";
        public static string GOAL_ALREADY_REGISTERED = "This Goal code is already in use";
        public static string RESOURCE_ALREADY_REGISTERED = "This Resource code is already in use";
        public static string RESOURCE_GOAL_ALREADY_REGISTERED = "This Resource/Goal link is already registered";
        public static string ROLE_ALREADY_REGISTERED = "This Employee role is already in use";
        public static string SITE_ALREADY_REGISTERED = "This Site code is already in use";
    }
}
