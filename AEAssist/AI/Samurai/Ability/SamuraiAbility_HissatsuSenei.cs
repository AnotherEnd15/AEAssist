﻿using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist.AI
{
    public class SamuraiAbility_HissatsuSenei : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (ActionResourceManager.Samurai.Kenki >=25 && 
                SpellsDefine.HissatsuSenei.Cooldown.TotalSeconds == 0 &&
                Core.Me.HasAura(AurasDefine.Shifu))
                return 1;
            return -1;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.HissatsuSenei;
            if (await spell.DoAbility())
                return spell;
            return null;
        }
    }
}