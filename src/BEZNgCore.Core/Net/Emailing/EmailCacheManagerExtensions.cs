using Abp.Runtime.Caching;

namespace BEZNgCore.Net.Emailing;

public static class EmailCacheManagerExtensions
{
    public static ITypedCache<string, EmailTemplateCacheItem> GetEmailTemplateCache(this ICacheManager cacheManager)
    {
        return cacheManager.GetCache<string, EmailTemplateCacheItem>(EmailTemplateCacheItem.CacheName);
    }
}