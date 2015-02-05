using System.Configuration;

namespace RabbitCache
{
    public class ConfigurationSectionHandler : ConfigurationSection
    {
        protected const string SECTION_PATH = "RabbitCache";

        [ConfigurationProperty("ServiceBusName")]
        public virtual SectionElement ServiceBusNameElement
        {
            get { return (SectionElement)this["ServiceBusName"]; }
            set { this["ServiceBusName"] = value; }
        }
        [ConfigurationProperty("ConnectionString")]
        public virtual SectionElement ConnectionStringElement
        {
            get { return (SectionElement) this["ConnectionString"]; }
            set
            { this["ConnectionString"] = value; }
        }

        public static ConfigurationSectionHandler GetSection()
        {
            return (ConfigurationSectionHandler)ConfigurationManager.GetSection(ConfigurationSectionHandler.SECTION_PATH);
        }
    }
}