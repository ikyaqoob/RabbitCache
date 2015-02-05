using System.Configuration;

namespace RabbitCache
{
    public class SectionElement : ConfigurationElement
    {
        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get
            {
                return this["value"] as string;
            }
        }

        [ConfigurationProperty("type", IsRequired = false)]
        public string DataType
        {
            get
            {
                return this["type"] as string;
            }
        }
    }
}