using AutoMapper;
using EyeCareHub.API.Dtos.Doctors;
using EyeCareHub.DAL.Entities.Identity;
using System.Collections.Generic;
using System;
using System.Linq;

namespace EyeCareHub.API.Helper
{
    public class WorkDaysToDtoResolver : IValueResolver<DoctorWorkSchedule, DoctorWorkScheduleDto, List<string>>
    {
        public List<string> Resolve(DoctorWorkSchedule source, DoctorWorkScheduleDto destination, List<string> destMember, ResolutionContext context)
        {
            if (source == null || source.WorkDays == WorkDays.None)
                return new List<string>();

            return Enum.GetValues(typeof(WorkDays))
                .Cast<WorkDays>()
                .Where(day => day != WorkDays.None && source.WorkDays.HasFlag(day))
                .Select(day => day.ToString())
                .ToList();
        }
    }
}
