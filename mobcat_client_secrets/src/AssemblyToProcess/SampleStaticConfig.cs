using MobCAT.ClientSecrets;

namespace AssemblyToProcess
{
    public static class SampleStaticConfig
    {
        [ClientSecret(SampleConfigTestValues.LOOKUP_KEY_A)]
        public const string MyConstantKeyName = SampleConfigTestValues.LOOKUP_KEY_B;

        [ClientSecret(SampleConfigTestValues.LOOKUP_KEY_A)]
        public static string MyStaticKeyName = SampleConfigTestValues.LOOKUP_KEY_B;

        [ClientSecret(SampleConfigTestValues.LOOKUP_KEY_A)]
        public static readonly string MyStaticReadonlyKeyName = SampleConfigTestValues.LOOKUP_KEY_B;
    }
}