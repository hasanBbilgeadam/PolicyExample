using AutoMapper;
using PolicyExample.Context;
using PolicyExample.Dtos;
using System.Runtime;

namespace PolicyExample.Mapping
{
    public class MyApplicationMappingProfile:Profile
    {
        public MyApplicationMappingProfile()
        {

            CreateMap<DailyCreateDto, Daily>();
            CreateMap<DailyUpdateDto, Daily>();


        }
    }

}
