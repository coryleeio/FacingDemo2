using System.Collections.Generic;

namespace Gamepackage
{
    public class CampaignTemplate
    {
        public string Identifier;
        public string RulesEngineClassName;
        public Dictionary<string, string> Settings;
        public Dictionary<int, int> XpForLevel;
        public Dictionary<int, int> XpAwardedForKillingEntityOfLevel;
    }
}
