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
    internal class AstAbilityDivination:IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.Divination.IsReady()) return -1;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Divination.GetSpellEntity();
            if (spell == null) return null;
            AIRoot.GetBattleData<AstBattleData>().half = true;
            SettingMgr.GetSetting<AstSettings>().AstHalfCard = true;
            var ret = await spell.DoAbility();
            return ret ? spell : null;
        }
    }
}
