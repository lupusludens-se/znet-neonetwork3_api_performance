using System.ComponentModel;

namespace SE.Neo.Common.Enums
{
    public enum EnvironmentEnum
    {
        [Description("local")]
        Local = 1,
        [Description("demo")]
        Demo,
        [Description("dev")]
        Dev,
        [Description("tst")]
        Test,
        [Description("preprod")]
        PreProd,
        [Description("prod")]
        Prod

    }
}
