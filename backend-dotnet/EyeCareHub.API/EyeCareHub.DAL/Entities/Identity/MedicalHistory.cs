using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.DAL.Entities.Identity
{
    [Flags]
    public enum MedicalHistory
    {
            None = 0,
            Diabetes = 1,    
            HighBloodPressure = 2,   
            EyeAllergies = 4,        
            DryEyes = 8,             
            WearsContactLenses = 16,   
    }
}
