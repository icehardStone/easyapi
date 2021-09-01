//
// ====================================================================================================================================
// Copyright (C) 2020-218 一朵小黄花
// 文件名: HttpMessageLoggerHandler.cs
// 路  径: EasyApi\HttpMessageLoggerHandler.cs
// 时  间: 2021-09-01
// 版  本: 1.0
// 作  者: hu.guanghua@synyi.com
// 邮  箱: hu.guanghua@synyi.com
// 主  题: 
// 
// before i write this only god and i know ,now only god
// 
//  文件修改记录
// =========================================================================================================================================

using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EasyApi
{
    public class HttpMessageLoggerHandler: DelegatingHandler
    {
        private readonly ILogger _logger;
        public HttpMessageLoggerHandler(ILogger logger)
        {
            _logger = logger;
        }
        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = base.Send(request, cancellationToken);
            _logger.LoggerRequestMessage(response);

            return response;
        }
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            _logger.LoggerRequestMessage(response);
            return response;
        }
    }
}
