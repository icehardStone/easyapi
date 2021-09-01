//
// ====================================================================================================================================
// Copyright (C) 2020-218 一朵小黄花
// 文件名: LoggingExtensions.cs
// 路  径: EasyApi\LoggingExtensions.cs
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
using System;
using System.Linq;
using System.Net.Http;

namespace EasyApi
{
    internal static class LoggingExtensions
    {
        private static Action<ILogger, HttpResponseMessage, Exception> _loggerRequestMessage;
        static LoggingExtensions()
        {
            _loggerRequestMessage = (logger, response, error) =>
            {
                string info = $@"
[-- easy api ] ============================================= request start =============================================
[-- easy api ] {response.RequestMessage.Method} {response.RequestMessage.RequestUri.PathAndQuery} {response.RequestMessage.RequestUri.Scheme}/{response.RequestMessage.Version}
[-- easy api ] Host: {response.RequestMessage.RequestUri.Scheme}://{response.RequestMessage.RequestUri.Host}
{string.Join("\r\n", response.RequestMessage.Headers.Select(o => $"[-- easy api ] {o.Key} {string.Join(",",o.Value)}"))}
[-- easy api ] Body: {(response.RequestMessage.Content?.GetType().Name == "StringContent" ? response.RequestMessage.Content?.ReadAsStringAsync().GetAwaiter().GetResult().Take(256):string.Empty)}...
[-- easy api ] ============================================= request end  ===============================================
[-- easy api ] ============================================= response start =============================================
[-- easy api ] {response.RequestMessage.RequestUri.Scheme.ToUpper()}/{response.Version} {(int)response.StatusCode} {response.ReasonPhrase}
{string.Join("\r\n", response.Headers.Select(o => $"[-- easy api ] {o.Key} {string.Join(",", o.Value)}"))}
[-- easy api ] ============================================= response end   =============================================
";
                logger.LogInformation(info);
            };
        }
        public static void LoggerRequestMessage(this ILogger logger, HttpResponseMessage requestMessage)
        {
            _loggerRequestMessage(logger, requestMessage, null);
        }
    }
}
