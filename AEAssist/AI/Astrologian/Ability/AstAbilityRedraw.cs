﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Objects;


namespace AEAssist.AI.Astrologian.Ability
{
    internal class AstAbilityRedraw:IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!Core.Me.HasAura(AurasDefine.ClarifyingDraw))
            {
                return -1;
            }
            if (ActionResourceManager.Astrologian.Arcana == ActionResourceManager.Astrologian.AstrologianCard.Balance || ActionResourceManager.Astrologian.Arcana == ActionResourceManager.Astrologian.AstrologianCard.Arrow || ActionResourceManager.Astrologian.Arcana == ActionResourceManager.Astrologian.AstrologianCard.Spear)
            {
                LogHelper.Debug("近战卡不抽");
                return -2;
            }
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Redraw.GetSpellEntity();
            if (spell == null) return null;
            var ret = await spell.DoAbility();
            return ret ? spell : null;
        }
    }
}