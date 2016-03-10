using System;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;

namespace FiltersSample.Filters
{
    public class NaiveCacheResourceFilterAttribute : Attribute,
        IResourceFilter
    {
        private static readonly Dictionary<string, object> _cache 
                    = new Dictionary<string, object>();
        private string _cacheKey;
          
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            _cacheKey = context.HttpContext.Request.Path.ToString();
            if (_cache.ContainsKey(_cacheKey))
            {
                var result = _cache[_cacheKey] as IActionResult;
                if (result != null)
                {
                    context.Result = result;
                }
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            if (!String.IsNullOrEmpty(_cacheKey) &&
                !_cache.ContainsKey(_cacheKey))
            {
                _cache.Add(_cacheKey, context.Result);
            }
        }
    }
}