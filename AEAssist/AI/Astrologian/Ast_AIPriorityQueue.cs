using AEAssist.AI.Astrologian.Ability;
using AEAssist.AI.Astrologian.GCD;
using AEAssist.Helper;
using ff14bot.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AEAssist.AI.Astrologian
{
    [Job(ClassJobType.Astrologian)]
    internal class Ast_AIPriorityQueue : IAIPriorityQueue
    {
        public List<IAIHandler> GCDQueue { get; } = new List<IAIHandler>()
        {
            //new SageGCDEgeiro(),
            new AstGCDAscend(),
            new AstGCDDot(),
            //new SageGcdToxikon(),
            //new SageGcdPhlegma(),
            new AstAspectedAstHelios(),
            new AstHelios(),
            new AstAOE(),
            new AstBaseGCD(),
            //new SageGCDDyskrasia(),
            new AstGCDAspectedBenefic(),
        };

        public List<IAIHandler> AbilityQueue { get; } = new List<IAIHandler>()
        {
            
            new AstAbilityLightspeed(),
            new AstAbilityAstrodyne(),
            new AstAbilityDraw(),
            new AstAbilityRedraw(),
            new AstAbilityPlay(),
            new AstAbilityDivination(),
            new AstAbilityHalfPlay(),
            new AstAbilityCelestialOpposition(),
            new AstAbilityCelestialIntersection(),
            new AstAbilityEssentialDignity(),
            new AstAbilitySynastry(),
            new AstAbilityExaltation(),
            new AstAbilityMinorArcana(),
            new AstAbilityCrownPlay(),
            new AstAbilityLucidDreaming(),
            new AstAbilityUsePotion(),
        };
        public async Task<bool> UsePotion()
        {
            return await PotionHelper.ForceUsePotion(SettingMgr.GetSetting<GeneralSettings>().MindPotionId);
        }
    }
}
