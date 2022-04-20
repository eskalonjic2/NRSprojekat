using CinemaluxAPI.DAL.CinemaluxCatalogue.DAL.OrganizationCatalogue.DAL;
using Microsoft.AspNetCore.Http;

public sealed class TenantService : ITenantService
{
    private readonly HttpContext _httpContext;
    private readonly ITenantIdentificationService _service;

    public TenantService(IHttpContextAccessor accessor, ITenantIdentificationService service)
    {
        this._httpContext = accessor.HttpContext;
        this._service = service;
    }

    public string GetCurrentTenant()
    {
        return "default";
    }
}