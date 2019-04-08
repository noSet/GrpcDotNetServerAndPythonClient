using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Quotes;

namespace GrpcServer
{
    public class QuotesService : QuotesServer.QuotesServerBase
    {
        private readonly ILogger<QuotesService> _logger;

        public QuotesService(ILogger<QuotesService> logger)
        {
            _logger = logger;
        }

        public override async Task AccessToTheMarket(Subscription request, IServerStreamWriter<Quote> responseStream, ServerCallContext context)
        {
            _logger.LogInformation($"收到来自客户端的订阅，订阅类型：{request.QuoteType}");

            Random random = new Random();
            try
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(random.Next(1, 500)));
                    string message = $"行情{request.QuoteType}的信息：{DateTimeOffset.Now.Ticks}";
                    _logger.LogInformation($"发送行情：{message}");
                    await responseStream.WriteAsync(new Quote() { QuoteType = request.QuoteType, Message = message });
                }
            }

            catch (InvalidOperationException ex) when (ex.Message == "Writing is not allowed after writer was completed.")
            {
                _logger.LogInformation($"客户端结束行情{request.QuoteType}订阅");
            }
        }
    }
}
