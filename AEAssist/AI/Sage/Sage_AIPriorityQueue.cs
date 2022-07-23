using AEAssist.AI.Sage.Ability;
using AEAssist.AI.Sage.GCD;
using AEAssist.Helper;
using ff14bot.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AEAssist.AI.Sage
{
    [Job(ClassJobType.Sage)]
    public class Sage_AIPriorityQueue : IAIPriorityQueue
    {
        public List<IAIHandler> GCDQueue { get; } = new List<IAIHandler>()
        {
            new SageGCDEgeiro(),
            new SageGCDDyskrasia(),
            new SageGcdDot(),
            new SageGcdToxikon(),
            new SageGcdPhlegma(),
            new SageGCDEukrasianDiagnosis(),
            new SageBaseGCD(),
        };

        public List<IAIHandler> AbilityQueue { get; } = new List<IAIHandler>()
        {
         new SageAbilityLucidDreaming(),
         new SageAbilityUsePotion(),
         //new SageAbilityKardia()
        };
        public async Task<bool> UsePotion()
        {
            return await PotionHelper.ForceUsePotion(SettingMgr.GetSetting<GeneralSettings>().MindPotionId);
        }
    }
}