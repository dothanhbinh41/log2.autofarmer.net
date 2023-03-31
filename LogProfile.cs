using AutoMapper;
using LogJson.AutoFarmer.Controllers.Dtos;
using LogJson.AutoFarmer.Models;

namespace LogJson.AutoFarmer
{
    public class LogProfile : Profile
    {
        public LogProfile()
        {
            CreateMap<SendXmlLogRequestDto, XmlLogDto>();
            CreateMap<ScreenDefinitionRequestDto, ScreenDefinition>();
        }
    }
}
