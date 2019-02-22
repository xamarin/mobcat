using MobCAT.ClientSecrets;

namespace AssemblyToProcess
{
    public class SampleConfig
    {
        [ClientSecret(SampleConfigTestValues.LOOKUP_KEY_A)]
        private string MyPrivateKeyName = SampleConfigTestValues.LOOKUP_KEY_B;

        [ClientSecret(SampleConfigTestValues.LOOKUP_KEY_A)]
        private static string MyPrivateStaticKeyName = SampleConfigTestValues.LOOKUP_KEY_B;

        [ClientSecret(SampleConfigTestValues.LOOKUP_KEY_A)]
        public const string MyConstantKeyName = SampleConfigTestValues.LOOKUP_KEY_B;

        [ClientSecret(SampleConfigTestValues.LOOKUP_KEY_A)]
        public static string MyStaticKeyName = SampleConfigTestValues.LOOKUP_KEY_B;

        [ClientSecret(SampleConfigTestValues.LOOKUP_KEY_A)]
        public static readonly string MyStaticReadonlyKeyName = SampleConfigTestValues.LOOKUP_KEY_B;

        [ClientSecret(SampleConfigTestValues.LOOKUP_KEY_A)]
        public readonly string MyReadonlyKeyName = SampleConfigTestValues.LOOKUP_KEY_B;

        [ClientSecret(SampleConfigTestValues.LOOKUP_KEY_A)]
        public string MyKeyName = SampleConfigTestValues.LOOKUP_KEY_B;

        public static string TestPrivateStaticWasSet() => MyPrivateStaticKeyName;

        public string TestPrivateWasSet() => MyPrivateKeyName;

        public string TestConstantWasUpdated() => MyConstantKeyName;
    }
}