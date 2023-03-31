using LogJson.AutoFarmer.Models;

namespace LogJson.AutoFarmer.Controllers.Dtos
{
    public class ScreenDefinitionRequestDto
    {
        public string ScreenId { get; set; }
        public string AppName { get; set; }
        public string Language { get; set; }
        public List<Definition> Definitons { get; set; }
        public List<KeywordDefinition> Keywords { get; set; }
    }
}
