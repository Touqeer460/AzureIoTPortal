using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureIOT.Models
{
    public class Rules
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DeviceGroup { get; set; }
        public RuleSeverity Severity { get; set; }
        public RuleSet[] RuleSets { get; set; }
        public Status Status { get; set; }

        public static string GetSeverityString(RuleSeverity severity)
        {
            if (severity == RuleSeverity.High)
                return "High";
            else if (severity == RuleSeverity.Low)
                return "Low";
            return "N/A";
        }

        public static string[] AllowedOperators()
        {
            return new string[] { "<", ">", "<=", ">=", "==" };
        }
    }

    public class RuleSet
    {
        public string Telemetry { get; set; }
        public string Operator { get; set; }
        public double Value { get; set; }
    }

    public enum RuleSeverity
    {
        High = 1,
        Low = 0,
        NA = -1
    }
}
