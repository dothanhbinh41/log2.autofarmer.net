namespace LogJson.AutoFarmer.Controllers.Dtos
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int Count { get; set; }
    }
}
