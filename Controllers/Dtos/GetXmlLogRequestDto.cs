namespace LogJson.AutoFarmer.Controllers.Dtos
{
    public class GetXmlLogRequestDto
    {
        public string AndroidId { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 100;
    }
}
