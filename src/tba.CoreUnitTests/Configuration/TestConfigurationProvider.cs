using tba.Core.Configuration;

namespace tba.CoreUnitTests.Configuration
{
    public class TestConfigurationProvider : ConfigurationProvider<TestConfigurationSection>
    {
        public TestConfigurationProvider(string filename)
            : base(filename)
        {
        }
    }
}