using System.ComponentModel;

namespace SE.Neo.Common.Enums
{
    public enum InitiativeScale : int
    {
        [Description("State Level (Choose one or more states - US only)")]
        State = 1,


        [Description("National (Choose one or more countries)")]
        National,


        [Description("Regional (Choose one or more continents)")]
        Regional,
    }
}
