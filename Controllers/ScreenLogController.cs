using AutoMapper;
using LogJson.AutoFarmer.Controllers.Dtos;
using LogJson.AutoFarmer.Models;
using LogJson.AutoFarmer.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LogJson.AutoFarmer.Controllers
{
    [Route("v1")]
    public class ScreenLogController : ControllerBase
    {
        private readonly IMongoRepository<ScreenDefinition> repository;
        private readonly IAutoFarmerDistributeCache autoFarmerDistributeCache;
        private readonly IMapper mapper;
        const string KeyLogs = "logs";
        public ScreenLogController(
            IMongoRepository<ScreenDefinition> repository,
            IAutoFarmerDistributeCache autoFarmerDistributeCache,
            IMapper mapper)
        {
            this.repository = repository;
            this.autoFarmerDistributeCache = autoFarmerDistributeCache;
            this.mapper = mapper;
        }

        [HttpPost("send-xml-log")]
        public async Task<bool> SendXmlLogAsync([FromBody] SendXmlLogRequestDto request)
        {
            var data = await autoFarmerDistributeCache.GetAsync<List<XmlLogDto>>(KeyLogs);
            data ??= new List<XmlLogDto>();
            var log = mapper.Map<XmlLogDto>(request);
            log.TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
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
                .Where(d => request.StartDate.HasValue == false || d.TimeStamp >= request.StartDate)
                .Where(d => request.EndDate.HasValue == false || d.TimeStamp <= request.EndDate)
                .Skip((request.Page - 1) * request.Limit)
                .Take(request.Limit)
                .ToList()
            };
        }

        [HttpPost("update-definition")]
        public async Task<ScreenDefinition> UpdateDefinition([FromBody] ScreenDefinitionRequestDto request)
        {
            var entity = mapper.Map<ScreenDefinition>(request);
            entity.Id = ObjectId.GenerateNewId().ToString();
            var filter = Builders<ScreenDefinition>.Filter.Eq(d => d.ScreenId, request.ScreenId);
            var document = await repository.Collection.FindOneAndReplaceAsync(filter, entity, new FindOneAndReplaceOptions<ScreenDefinition> { IsUpsert = true, ReturnDocument = ReturnDocument.After });
            return document;
        }

        [HttpPost("get-definition")]
        public async Task<ScreenDefinition> GetDefinition([FromBody] GetScreenDefinitionRequestDto request)
        {
            var filter = Builders<ScreenDefinition>.Filter.Eq(d => d.ScreenId, request.ScreenId)
               & Builders<ScreenDefinition>.Filter.Eq(d => d.AppName, request.AppName)
               & Builders<ScreenDefinition>.Filter.Eq(d => d.Language, request.Language);
            var document = await repository.Collection.Find(filter).FirstOrDefaultAsync();
            return document;
        }

        [HttpGet("definitions")]
        public async Task<List<ScreenDefinition>> GetAllDefinition()
        {
            var documents = await repository.Collection.Find(Builders<ScreenDefinition>.Filter.Empty).ToListAsync();
            return documents;
        }
    }
}