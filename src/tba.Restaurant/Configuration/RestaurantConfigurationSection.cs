using System.Configuration;

namespace tba.Restaurant.Configuration
{
    /// <summary>
    /// Configuration section of the web.config file covering restaurant domain
    /// </summary>
    public class RestaurantConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// URL of the restaurant microservice
        /// </summary>
        [ConfigurationProperty("ServiceUrl", IsRequired = true)]
        public string ServiceUrl
        {
            get { return (string)this["ServiceUrl"]; }
            set { this["ServiceUrl"] = value; }
        }
    }
}
