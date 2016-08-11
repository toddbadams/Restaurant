using System.Reflection;

namespace tba.MicroService
{
    public class MicroServiceOptions
    {
        private string _displayName;
        private string _serviceName;
        public int? Port { get; set; }

        public string DisplayName
        {
            get
            {
                return string.IsNullOrEmpty(_displayName)
                    ? Assembly.GetEntryAssembly().GetName().Name
                    : _displayName;
            }
            set { _displayName = value; }
        }

        public string ServiceName
        {
            get
            {
                return string.IsNullOrEmpty(_serviceName)
                    ? Assembly.GetEntryAssembly().GetName().Name
                    : _serviceName;
            }
            set { _serviceName = value; }
        }

        public string Uri { get; set; }
    }
}