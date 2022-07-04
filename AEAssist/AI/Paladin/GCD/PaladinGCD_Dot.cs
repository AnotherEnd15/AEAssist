﻿using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist.AI.Paladin.GCD
{
    public class PaladinGCD_Dot : IAIHandler
    {
        uint spell;
        static public uint GetSpell()
        {
            switch (ActionManager.LastSpellId)
            {
                case SpellsDefine.FastBlade:
                    return SpellsDefine.RiotBlade;
                case SpellsDefine.RiotBlade:
                    return SpellsDefine.GoringBlade;
                default:
                    return SpellsDefine.FastBlade;
            }


        }
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.GoringBlade.IsUnlock())
                return -2;

            if (!DataBinding.Instance.UseDot)
                return -3;

            if (!Paladin_SpellHelper.NeedRenewDot())
                return -4;
            spell = GetSpell();

            if (!spell.IsReady())
                return -1;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoGCD()) return spell.GetSpellEntity();

            return null;
        }

        
    }
}