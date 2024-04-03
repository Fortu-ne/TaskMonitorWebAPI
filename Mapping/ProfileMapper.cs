
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TaskMonitorWebAPI.Dto;
using TaskMonitorWebAPI.Entities;

namespace TaskMonitorWebAPI.Mapping
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<Tasks, TasksDto>();
            CreateMap<TasksDto, Tasks>();
        }
    }
}
