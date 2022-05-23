﻿using System.CodeDom;
using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;

namespace AEAssist.AI.Ninja.Ability
{
    public class NinjaAbility_TenChiJin : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.TenChiJin.IsUnlock())
            {
                return -10;
            }
            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.TenChiJin.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoAbility();
            if (ret)
                return spell;
            return null;
        }
    }
}