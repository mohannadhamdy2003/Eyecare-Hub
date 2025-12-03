using AutoMapper;
using EyeCareHub.API.Dtos.Doctors;
using EyeCareHub.API.Dtos.Product;
using EyeCareHub.DAL.Entities.Identity;
using EyeCareHub.DAL.Entities.ProductInfo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EyeCareHub.API.Helper
{
    public class WorkDaysFromDtoResolver : IValueResolver<DoctorWorkScheduleDto, DoctorWorkSchedule, WorkDays>
    {
        public WorkDays Resolve(DoctorWorkScheduleDto source, DoctorWorkSchedule destination, WorkDays destMember, ResolutionContext context)
        {
            if (source.WorkDays == null || !source.WorkDays.Any())
                return WorkDays.None;

            WorkDays result = WorkDays.None;
            foreach (var day in source.WorkDays)
            {
                if (Enum.TryParse<WorkDays>(day, true, out var workDay))
                {
                    result |= workDay;
                }
            }

            return result;
        }
    }
}
