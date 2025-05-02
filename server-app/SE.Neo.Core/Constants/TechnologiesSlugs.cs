using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SE.Neo.Core.Constants
{
    public static class TechnologiesSlugs
    {
        public const string CarportSolar = "carport-solar";
        public const string BuildingControls = "building-controls";
        public const string BatteryStorage = "battery-storage";
        public const string BuildingEnvelope = "building-envelope";
        public const string EVCharging = "ev-charging";
        public const string FuelCells = "fuel-cell";
        public const string GroundmountSolar = "groundmount-solar";
        public const string HVAC = "hvac";
        public const string GreenHydrogen = "green_hydrogen";
        public const string Lighting = "lighting";
        public const string OffshoreWind = "offshore-wind";
        public const string OnshoreWind = "onshore-wind";
        public const string RooftopSolar = "rooftop-solar";
        public const string RenewableThermal = "renewable_thermal";
        public const string EmergingTechnology = "emerging_technology";

        public static string GetSlugValue(string slugName)
        {
            FieldInfo field = typeof(TechnologiesSlugs).GetField(slugName, BindingFlags.Public | BindingFlags.Static);
            return field != null ? (string)field.GetValue(null) : null;
        }
    }
}
