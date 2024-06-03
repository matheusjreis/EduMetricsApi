using AutoMapper;
using EduMetricsApi.Application.Interfaces;
using EduMetricsApi.Domain.Core.Services.Base;
using EduMetricsApi.Domain.Core.Services;
using EduMetricsApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EduMetricsApi.Application;

public class ApplicationServiceSession : IApplicationServiceSession
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public IServiceBaseGeneric<UserRegister> _serviceUserRegister;
    public IServiceBaseGeneric<UserSession> _serviceUserSession;
    public IServiceAuth _serviceAuth;
    public readonly IMapper _mapper;

    public ApplicationServiceSession(IServiceBaseGeneric<UserRegister> serviceUserRegister
                                , IServiceBaseGeneric<UserSession> serviceUserSession
                                , IServiceAuth serviceAuth
                                , IMapper mapper
                                , IHttpContextAccessor httpContextAccessor)
    {
        _serviceUserRegister = serviceUserRegister;
        _serviceUserSession = serviceUserSession;
        _serviceAuth = serviceAuth;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task<bool> IsSessionActivated()
    {
        _httpContextAccessor.HttpContext.Items.TryGetValue("SessionId", out var sessionId);
        _httpContextAccessor.HttpContext.Items.TryGetValue("UserId", out var userId);

        var activatedSession = _serviceUserSession.Get(x => x.UserId == Convert.ToInt32(userId));

        return await Task.FromResult(activatedSession.Where(x => x.ExpirationDate >= DateTime.Now).Any());
    }
}