﻿using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
namespace AEAssist.AI.Paladin.Ability
{
    public class _Blank : IAIHandler
    {
        uint spell;
        static public uint GetSpell()
        {
            return 0;
        }
        public int Check(SpellEntity lastSpell)
        {
            spell = GetSpell();

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