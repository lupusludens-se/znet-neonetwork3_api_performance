using System.ComponentModel;

namespace SE.Neo.Common.Enums
{
    public enum EventType
    {
        [Description("Invitations only")]
        Private = 1,

        [Description("Open to all members")]
        Public
    }
}
