using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.DAL.Entities.Identity
{
    public  enum AppointmentStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,

        [EnumMember(Value = "Confirmed")]
        Confirmed ,

        [EnumMember(Value = "Completed")]
        Completed ,

        [EnumMember(Value = "Cancelled")]
        Cancelled ,
    
    }
}
