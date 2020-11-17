namespace WEBAPI_VOPAK
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Cors;
    using System.Web.Http.Cors;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class CORSEnabledAttribute : Attribute, ICorsPolicyProvider
    {
        private CorsPolicy cp;
        public CORSEnabledAttribute()
        {
            cp = new CorsPolicy
            {
                AllowAnyMethod = true,
                AllowAnyHeader = true,
                AllowAnyOrigin = true
            };
        }
        Task<CorsPolicy> ICorsPolicyProvider.GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        { return Task.FromResult(cp); }
    }
}