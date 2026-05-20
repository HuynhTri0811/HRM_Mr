using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Yarp.ReverseProxy.Forwarder;

namespace ApiGateway.Extensions
{
    public static class YarpExtensions
    {
        public static void ConfigureYarp(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient();

            builder.Services.AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

            var pipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
                .AddRetry(new()
                {
                    ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                        .Handle<HttpRequestException>()
                        .HandleResult(r => r.StatusCode == HttpStatusCode.InternalServerError
                                        || r.StatusCode == HttpStatusCode.BadGateway
                                        || r.StatusCode == HttpStatusCode.ServiceUnavailable),
                    MaxRetryAttempts = 3, 
                    Delay = TimeSpan.FromSeconds(1), 
                    BackoffType = DelayBackoffType.Exponential, 
                    UseJitter = true 
                })
                .AddCircuitBreaker(new()
                {
                    ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                        .Handle<HttpRequestException>()
                        .HandleResult(r => (int)r.StatusCode >= 500),

                    FailureRatio = 0.5,
                    SamplingDuration = TimeSpan.FromSeconds(10),

                    MinimumThroughput = 8,

                    BreakDuration = TimeSpan.FromSeconds(30)
                })
                .AddTimeout(TimeSpan.FromSeconds(5))
                .Build();

            builder.Services.AddSingleton<IForwarderHttpClientFactory>(new ResilientForwarderHttpClientFactory(pipeline));
        }

        public static void UseYarp(this WebApplication app)
        {
            app.MapReverseProxy();
        }
    }

    // ---------------------------------------------------------------------
    // CÁC LỚP HỖ TRỢ ĐĂNG KÝ RESILIENCE CHO YARP FORWARDER HTTP CLIENT
    // ---------------------------------------------------------------------
    public class ResilientForwarderHttpClientFactory : ForwarderHttpClientFactory
    {
        private readonly ResiliencePipeline<HttpResponseMessage> _pipeline;

        public ResilientForwarderHttpClientFactory(ResiliencePipeline<HttpResponseMessage> pipeline)
        {
            _pipeline = pipeline;
        }

        protected override HttpMessageHandler WrapHandler(ForwarderHttpClientContext context, HttpMessageHandler handler)
        {
            var baseHandler = base.WrapHandler(context, handler);
            return new PollyDelegatingHandler(_pipeline, baseHandler);
        }
    }

    public class PollyDelegatingHandler : DelegatingHandler
    {
        private readonly ResiliencePipeline<HttpResponseMessage> _pipeline;

        public PollyDelegatingHandler(ResiliencePipeline<HttpResponseMessage> pipeline, HttpMessageHandler innerHandler)
        {
            _pipeline = pipeline;
            InnerHandler = innerHandler;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await _pipeline.ExecuteAsync(async token =>
                await base.SendAsync(request, token), cancellationToken);
        }
    }
}
