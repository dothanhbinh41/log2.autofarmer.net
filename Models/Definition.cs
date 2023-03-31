namespace LogJson.AutoFarmer.Models
{

    public class Definition
    {
        public string ClassName { get; set; }
        public string ContentDescription { get; set; }
        public string Text { get; set; }
        public string HintText { get; set; }
        public bool Checked { get; set; }
        public bool Clickable { get; set; }
        public bool Selected { get; set; }

    }

    public class KeywordDefinition : Definition
    {
        public string Keyword { get; set; }
    }

    public class ScreenDefinition : Entity
    {
        public string ScreenId { get; set; }
        public string AppName { get; set; }
        public string Language { get; set; }
        public List<Definition> Definitons { get; set; }
        public List<KeywordDefinition> Keywords { get; set; }
    }
}
