using MobCAT.ClientSecrets;

namespace AssemblyToProcess
{
    public class SampleImplicitConfig
    {
        [ClientSecret]
        public const string MyConstantKeyName = SampleConfigTestValues.LOOKUP_KEY_B;

        [ClientSecret]
        public string MyKeyName = SampleConfigTestValues.LOOKUP_KEY_B;
    }
}