using System;
using EasyNetQ;
using log4net;
using RabbitMQ.Client.Events;

namespace RabbitCache.Caches.Serialization
{
    public class CacheConsumerErrorStrategy : IConsumerErrorStrategy
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(CacheConsumerErrorStrategy));

        public PostExceptionAckStrategy PostExceptionAckStrategy()
        {
            return EasyNetQ.PostExceptionAckStrategy.ShouldAck;
        }
        public void HandleConsumerError(BasicDeliverEventArgs _devliverArgs, Exception _exception)
        {
            this._logger.Debug(_exception);
        }

        public void Dispose()
        {

        }
    }
}
