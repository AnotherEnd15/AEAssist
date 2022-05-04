﻿using System.Threading.Tasks;
using AEAssist.Define;
using ff14bot;

namespace AEAssist.AI.Samurai.GCD
{
    public class SamuraiGCD_BaseGCDCombo : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (Core.Me.HasAura(AurasDefine.Kaiten))
                return -1;
            if (Core.Me.HasAura(AurasDefine.MeikyoShisui) && SamuraiSpellHelper.SenCounts() == 3) return -2;

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            //var spell = SamuraiSpellHelper.GetBaseSpell();
            //if (spell == null) return null;
            //if (await SpellHelper.CastGCD(spell,Core.Me.CurrentTarget))
            //    return spell;
            return await SamuraiSpellHelper.GetBaseSpell();
        }
    }
}