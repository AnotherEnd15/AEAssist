﻿using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;

namespace AEAssist.AI.Samurai.Ability
{
    public class SamuraiAbility_MeikyoShisui : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!AIRoot.GetBattleData<SamuraiBattleData>().Bursting || !AIRoot.GetBattleData<SamuraiBattleData>().EvenBursting)
            {
                return -4;
            }
            if (!SpellsDefine.MeikyoShisui.IsReady())
                return -14;
            if (Core.Me.HasAura(AurasDefine.MeikyoShisui) || SpellsDefine.MeikyoShisui.RecentlyUsed())
            {
                return -13;
            }
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.MeikyoShisui;
            if (await spell.DoAbility())
                return spell.GetSpellEntity();
            return null;
        }
    }
}