using AutoMapper;
using LogJson.AutoFarmer.Controllers.Dtos;
using LogJson.AutoFarmer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LogJson.AutoFarmer.Controllers
{
    [Route("v1")]
    public class ScreenLogController : ControllerBase
    {
        private readonly IAutoFarmerDistributeCache autoFarmerDistributeCache;
        private readonly IMapper mapper;
        const string KeyLogs = "logs";
        public ScreenLogController(IAutoFarmerDistributeCache autoFarmerDistributeCache, IMapper mapper)
        {
            this.autoFarmerDistributeCache = autoFarmerDistributeCache;
            this.mapper = mapper;
        }

        [HttpPost("send-xml-log")]
        public async Task<bool> SendXmlLogAsync([FromBody] SendXmlLogRequestDto request)
        {
            var data = await autoFarmerDistributeCache.GetAsync<List<XmlLogDto>>(KeyLogs);
            data ??= new List<XmlLogDto>();
            var log = mapper.Map<XmlLogDto>(request);
            log.CreationDate = DateTime.UtcNow;
            data.Add(log);
            await autoFarmerDistributeCache.SetAsync(KeyLogs, data, new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1) });
            return true;
        }


        [HttpPost("get-xml-log")]
        public async Task<PagedResult<XmlLogDto>> GetXmlLogAsync([FromBody] GetXmlLogRequestDto request)
        {
            var data = await autoFarmerDistributeCache.GetAsync<List<XmlLogDto>>(KeyLogs);
            data ??= new List<XmlLogDto>();
            return new PagedResult<XmlLogDto>
            {
                Count = data.Count,
                Items = data
                .Where(d => string.IsNullOrEmpty(request.AndroidId) || d.AndroidId == request.AndroidId)
                .Where(d => string.IsNullOrEmpty(request.StartDate) || d.AndroidId == request.AndroidId)
                .Where(d => string.IsNullOrEmpty(request.EndDate) || d.AndroidId == request.AndroidId)
                .Skip((request.Page - 1) * request.Limit)
                .Take(request.Limit)
                .ToList()
            };
        }
    }
}
