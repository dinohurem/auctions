
using PomoziAuctions.Core.Aggregates.CompanyAggregate.Interfaces;
using PomoziAuctions.Core.Aggregates.CompanyAggregate.Models;
using PomoziAuctions.Core.Auth.Security.Models;

namespace PomoziAuctions.Web.Security;

public class CurrentCompanyAccessor : ICurrentCompanyAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<CurrentCompanyAccessor> _logger;

    public CurrentCompanyAccessor(IHttpContextAccessor httpContextAccessor, ILogger<CurrentCompanyAccessor> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public CompanyInfo GetCurrentCompany()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            _logger.LogError("HttpContext is null.");
            return null;
        }

        var user = httpContext.User;
        if (user == null)
        {
            _logger.LogError("HttpContext.User is null.");
            return null;
        }

        var companyId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.CompanyId);
        var companyName = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.CompanyName);
        var companyType = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.CompanyType);
        return new CompanyInfo
        {
            Id = companyId != null ? companyId.Value != string.Empty ? int.Parse(companyId.Value) : null : null,
            Name = companyName?.Value,
            Type = companyType?.Value
        };
    }
}
