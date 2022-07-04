﻿using System.Threading.Tasks;
using AEAssist.Define;
using ff14bot;

namespace AEAssist.AI.Ninja.GCD
{
    public class NinjaGCD_BaseGCD : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            return await NinjaSpellHelper.BaseGCDCombo(Core.Me.CurrentTarget);
        }
    }
}