﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AEAssist.AI.Astrologian.Ability;
using AEAssist.AI.Astrologian.GCD;
using AEAssist.Helper;
using ff14bot.Enums;

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
            new AstBaseGCD(),
            //new SageGCDDyskrasia(),
        };

        public List<IAIHandler> AbilityQueue { get; } = new List<IAIHandler>()
        {
            new AstAbilityLightspeed(),
            new AstAbilityCelestialIntersection(),
            new AstAbilityEssentialDignity(),
            new AstAbilityExaltation(),
            new AstAbilityAstrodyne(),
            new AstAbilityDraw(),
            new AstAbilityRedraw(),
            new AstAbilityPlay(),
            new AstAbilityHalfPlay(),
            new AstAbilityLucidDreaming(),
            new AstAbilityDivination(),
            new AstAbilityMinorArcana(),
            new AstAbilityCrownPlay(),
            //new SageAbilityUsePotion(),
        };
        public async Task<bool> UsePotion()
        {
            return await PotionHelper.ForceUsePotion(SettingMgr.GetSetting<GeneralSettings>().MindPotionId);
        }
    }
}
