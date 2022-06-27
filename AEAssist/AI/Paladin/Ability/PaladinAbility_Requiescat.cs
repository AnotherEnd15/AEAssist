﻿using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;

namespace AEAssist.AI.Paladin.Ability
{
    public class PaladinAbility_Requiescat : IAIHandler
    {
        uint spell = SpellsDefine.Requiescat;


        public int Check(SpellEntity lastSpell)
        {
            if (!spell.IsReady())
                return -1;

            if (AIRoot.Instance.CloseBurst)
                return -2;

            if (Core.Me.HasMyAuraWithTimeleft(AurasDefine.Requiescat, 30-5*(int)AIRoot.Instance.GetGCDDuration()))
                return -3;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoAbility()) return spell.GetSpellEntity();

            return null;
        }
    }
}