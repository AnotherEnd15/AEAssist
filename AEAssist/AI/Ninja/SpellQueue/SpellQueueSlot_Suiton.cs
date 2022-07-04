﻿using AEAssist.Define;
using AEAssist.Helper;

namespace AEAssist.AI.Ninja.SpellQueue
{
    public class SpellQueueSlot_Suiton : IAISpellQueueSlot
    {
        public int Check(int index)
        {
            return NinjaSpellHelper.NinjutsuCheck();
        }

        public void Fill(SpellQueueSlot slot)
        {
            slot.SetBreakCondition(() => this.Check(0));
            var sp1 = SpellsDefine.Chi;
            var sp2 = SpellsDefine.Ten;
            var sp3 = SpellsDefine.Jin;
            if (RandomHelper.RandomBool())
            {
                slot.EnqueueGCD((sp1, SpellTargetType.Self));
                slot.EnqueueGCD((sp2, SpellTargetType.Self));
            }
            else
            {
                slot.EnqueueGCD((sp2, SpellTargetType.Self));
                slot.EnqueueGCD((sp1, SpellTargetType.Self));
            }
            slot.EnqueueGCD((sp3, SpellTargetType.Self));
            slot.EnqueueGCD((SpellsDefine.Suiton, SpellTargetType.CurrTarget));
        }
    }
}