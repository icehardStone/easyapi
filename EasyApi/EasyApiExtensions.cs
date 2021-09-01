//
// ====================================================================================================================================
// Copyright (C) 2020-218 一朵小黄花
// 文件名: EasyApiExtensions.cs
// 路  径: EasyApi\EasyApiExtensions.cs
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

using System;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RestEase;

namespace EasyApi.HttpCLientFactory
{
    public static class EasyApiExtensions
    {
        public static IHttpClientBuilder AddEasyApiClient<T>(this IServiceCollection services,
            Action<IServiceProvider, HttpClient> actions,
            TimeSpan handlerLifetime)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            return services.CreateHttpClient<T>(UniqueNameForType(typeof(T)), actions, handlerLifetime);
        }
        public static IHttpClientBuilder AddEasyApiClient<T>(this IServiceCollection services,
            string name,
            Action<IServiceProvider, HttpClient> actions,
            TimeSpan handlerLifetime)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));
            return services.CreateHttpClient<T>(name, actions, handlerLifetime);
        }
        public static IHttpClientBuilder AddEasyApiClient<T>(this IServiceCollection services,
            Action<IServiceProvider, HttpClient> actions)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            return services.CreateHttpClient<T>(UniqueNameForType(typeof(T)), actions, TimeSpan.FromMinutes(2));
        }
        public static IHttpClientBuilder AddEasyApiClient<T>(this IServiceCollection services,
            string name,
            Action<IServiceProvider, HttpClient> actions)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));
            return services.CreateHttpClient<T>(name, actions, TimeSpan.FromMinutes(2));
        }

        private static IHttpClientBuilder CreateHttpClient<T>(this IServiceCollection services,
            string name,
            Action<IServiceProvider, HttpClient> actions,
            TimeSpan handlerLifetime)
        {
            var builder = services.AddHttpClient(name, actions)
                            .AddHttpMessageHandler((serviceProvider) =>
                            {
                                var loggerfactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                                return new HttpMessageLoggerHandler(loggerfactory.CreateLogger<T>());
                            }).SetHandlerLifetime(handlerLifetime);
            builder.Services.AddScoped(typeof(T), (serviceProvider) =>
            {
                var clientfactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var client = clientfactory.CreateClient(name);
                return RestClient.For<T>(client);
            });
            return builder;
        }
        private static string UniqueNameForType(Type type)
        {
            var sb = new StringBuilder();
            Impl(type);
            return sb.ToString();

            void Impl(Type typeInfo)
            {
                if (typeInfo.IsGenericType)
                {
                    string fullName = type.GetGenericTypeDefinition().FullName;
                    sb.Append(fullName.Substring(0, fullName.LastIndexOf('`')));
                    sb.Append("<");
                    int i = 0;
                    foreach (var arg in typeInfo.GetGenericArguments())
                    {
                        if (i > 0)
                        {
                            sb.Append(",");
                        }
                        Impl(arg);
                        i++;
                    }
                    sb.Append(">");
                }
                else
                {
                    sb.Append(typeInfo.FullName);
                }
            }
        }

    }
}
