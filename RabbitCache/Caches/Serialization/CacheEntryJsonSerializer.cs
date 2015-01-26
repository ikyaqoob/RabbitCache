using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Text;
using EasyNetQ;
using log4net;
using Newtonsoft.Json;

namespace RabbitCache.Caches.Serialization
{
    /// <summary>
    /// Json Serializer which includes non-public properties and fields when serializing.
    /// </summary>
    public class CacheEntryJsonSerializer : ISerializer
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(CacheEntryJsonSerializer));
        
        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                ContractResolver = new CacheEntryContractResolver
                    {
                        #pragma warning disable 612,618
                        DefaultMembersSearchFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic // No other way works to get JsonSerializer to include non-public setters.
                        #pragma warning restore 612,618
                    },
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore, 
                DefaultValueHandling = DefaultValueHandling.Include,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                Error = (_serializer, _error) =>
                {
                    CacheEntryJsonSerializer._logger.Debug(_error.ErrorContext.Error);
                    _error.ErrorContext.Handled = true;
                },
            };

        /// <summary>
        /// Convert object of Type T to array of bytes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_bytes"></param>
        /// <returns></returns>
        public T BytesToMessage<T>(byte[] _bytes)
        {
            var _value = Encoding.UTF8.GetString(_bytes);
            return JsonConvert.DeserializeObject<T>(_value, this._serializerSettings);
        }
        /// <summary>
        /// Converts an array of bytes back into object of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_message"></param>
        /// <returns></returns>
        public byte[] MessageToBytes<T>(T _message)
        {
            var _serializeObject = JsonConvert.SerializeObject(_message, this._serializerSettings);
            return Encoding.UTF8.GetBytes(_serializeObject);
        }
    }
}