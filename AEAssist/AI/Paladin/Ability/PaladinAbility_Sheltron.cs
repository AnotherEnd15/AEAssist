﻿using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;

namespace AEAssist.AI.Paladin.Ability
{
    public class PaladinAbility_Sheltron : IAIHandler
    {
        uint spell;
        static public uint GetSpell()
        {
            if (SpellsDefine.HolySheltron.IsUnlock())
                return SpellsDefine.HolySheltron;
            return SpellsDefine.Sheltron;
        }
        public int Check(SpellEntity lastSpell)
        {
            if (ActionResourceManager.Paladin.Oath < SettingMgr.GetSetting<PaladinSettings>().SheltronThreshold)
                return -3;
            if (Core.Me.HasAura(AurasDefine.KnightsResolve) || Core.Me.HasAura(AurasDefine.Sheltron) || Core.Me.HasAura(AurasDefine.Sheltron))
                return -4;
            spell = GetSpell();
            if (!SettingMgr.GetSetting<PaladinSettings>().Sheltron)
                return -5;
            if (!spell.IsReady())
                return -1;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoAbility()) return spell.GetSpellEntity();

            return null;
        }
    }

}