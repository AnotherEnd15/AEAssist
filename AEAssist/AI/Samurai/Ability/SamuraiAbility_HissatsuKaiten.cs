﻿using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;

namespace AEAssist.AI.Samurai.Ability
{
    public class SamuraiAbility_HissatsuKaiten : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            var ta = Core.Me.CurrentTarget as Character;
            if (Core.Me.HasAura(AurasDefine.Kaiten))
                return -1;
            if (Core.Me.HasAura(AurasDefine.OgiReady) &&
                SpellsDefine.KaeshiSetsugekka.GetSpellEntity().Cooldown.TotalMilliseconds != 0 &&
                ta.HasAura(AurasDefine.Higanbana))
                return 1;

            if (SamuraiSpellHelper.SenCounts() == 0) return -1;

            if (SamuraiSpellHelper.SenCounts() == 1)
            {
                if (SpellsDefine.KaeshiSetsugekka.GetSpellEntity().Cooldown.TotalSeconds == 0)
                    return -2;

                if (ta.HasMyAura(AurasDefine.Higanbana) && ta.GetAuraById(AurasDefine.Higanbana)?.TimeLeft > 3)
                    return -1;
            }

            if (SamuraiSpellHelper.SenCounts() == 2)
                return -5;

            return 10;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.HissatsuKaiten;
            if (await spell.DoAbility())
                return spell.GetSpellEntity();
            return null;
        }
    }
}