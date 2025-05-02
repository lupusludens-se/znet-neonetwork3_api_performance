using System.ComponentModel;

namespace SE.Neo.Core.Enums
{
    public enum SettlementHubOrLoadZoneType
    {
        [Description("AZ-PV")]
        AzPv = 1,
        [Description("CA-NP15")]
        CaNp15,
        [Description("CA-SP15")]
        CaSp15,
        [Description("CA-ZP26")]
        CaZp26,
        [Description("ERCOT-H")]
        ErcotH,
        [Description("ERCOT-N")]
        ErcotN,
        [Description("ERCOT-S")]
        ErcotS,
        [Description("ERCOT-W")]
        ErcotW,
        [Description("MISO-IA")]
        MisoIa,
        [Description("MISO-Ark")]
        MisoArk,
        [Description("MISO-Gul")]
        MisoGul,
        [Description("MISO-IL")]
        MisoIl,
        [Description("MISO-IN")]
        MisoIn,
        [Description("MISO-MI")]
        MisoMi,
        [Description("MISO-MN")]
        MisoMn,
        [Description("MISO-MO")]
        MisoMo,
        [Description("MISO-ND")]
        MisoNd,
        [Description("NY-A")]
        NyA,
        [Description("NY-G")]
        NyG,
        [Description("NY-J")]
        NyJ,
        [Description("SPP-S")]
        SppS,
        [Description("SPP-N")]
        SppN,
        [Description("PJM-AEP GEN HUB")]
        PjmAepGenHub,
        [Description("PJM-AEP-DAYTON HUB")]
        PjmAepDaytonHub,
        [Description("PJM-ATSI GEN HUB")]
        PjmAtsiGenHub,
        [Description("PJM-CHICAGO GEN HUB")]
        PjmChicagoGenHub,
        [Description("PJM-CHICAGO HUB")]
        PjmChicagoHub,
        [Description("PJM-DOMINION HUB")]
        PjmDominionHub,
        [Description("PJM-EASTERN HUB")]
        PjmEasternHub,
        [Description("PJM-N ILLINOIS HUB")]
        PjmNIllinoisHub,
        [Description("PJM-N NEW JERSEY HUB")]
        PjmNNewJerseyHub,
        [Description("PJM-N OHIO HUB")]
        PjmNOhioHub,
        [Description("PJM-N WEST INT HUB")]
        PjmNWestIntHub,
        [Description("PJM-N WESTERN HUB")]
        PjmNWesternHub,
        [Description("PSEG Load Zone")]
        PsegLoadZone,
        [Description("ComEd Load Zone")]
        ComEdLoadZone,
        [Description("Dominion Load Zone")]
        DominionLoadZone,
        [Description("NEPOOL")]
        Nepool,
        [Description("NE-CT")]
        NeCt,
        [Description("NE-MA")]
        NeMa,
        [Description("NE-ME")]
        NeMe,
        [Description("NE-NH")]
        NeNh,
        [Description("NE-RI")]
        NeRi,
        [Description("Other")]
        Other,
        [Description("MISO-LA")]
        MisoLa,
    }
}
