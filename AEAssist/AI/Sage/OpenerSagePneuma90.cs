using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Opener;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using System;
using System.Collections.Generic;

namespace AEAssist.AI.Sage
{
    [Opener(ClassJobType.Sage, 90, "Pneuma Opener (not ready)")]
    public class OpenerSagePneuma90 : IOpener
    {
        public int Check()
        {
            if (!Core.Me.CurrentTarget.IsBoss())
            {
                LogHelper.Debug("Not running Opener current target is not a boss.");
                return -5;
            }
            
            if (PartyManager.NumMembers <= 4 && !Core.Me.CurrentTarget.IsDummy())
            {
                LogHelper.Debug("Not running Opener party is less than or equal to 4..");
                return -5;
            }
            
            if (SageSpellHelper.GetPhlegma() == null)
            {
                LogHelper.Debug("Not running Opener Phlegma is not ready.");
                return -5;
            }

            return 0;
        }

       public List<Action<SpellQueueSlot>> Openers { get; } = new List<Action<SpellQueueSlot>>()
        {
            Step0,
            Step1,
            Step2,
            Step3,
            Step4,
            Step5,
            Step6,
            Step7,
            Step8,
            Step9,
            Step10
        };


        private static void Step0(SpellQueueSlot slot)
        {
            slot.SetGCDQueue((SpellsDefine.Eukrasia, SpellTargetType.Self),
                (SpellsDefine.EukrasianDosisIII, SpellTargetType.CurrTarget));
        }


        private static void Step1(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.DosisIII, SpellTargetType.CurrTarget);
        }


        private static void Step2(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.DosisIII, SpellTargetType.CurrTarget);
        }


        private static void Step3(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.DosisIII, SpellTargetType.CurrTarget);
        }


        private static void Step4(SpellQueueSlot slot)
        {
            if (Core.Me.Distance(Core.Me.CurrentTarget) <= 8.3)
                slot.SetGCD(SpellsDefine.PhlegmaIII, SpellTargetType.CurrTarget);
            else
                slot.SetGCD(SpellsDefine.DosisIII, SpellTargetType.CurrTarget);

        }


        private static void Step5(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.DosisIII, SpellTargetType.CurrTarget);
        }


        private static void Step6(SpellQueueSlot slot)
        {
            if (Core.Me.Distance(Core.Me.CurrentTarget) < 8.3)
                slot.SetGCD(SpellsDefine.PhlegmaIII, SpellTargetType.CurrTarget);
            else
                slot.SetGCD(SpellsDefine.DosisIII, SpellTargetType.CurrTarget);
        }


        private static void Step7(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.DosisIII, SpellTargetType.CurrTarget);
        }


        private static void Step8(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.DosisIII, SpellTargetType.CurrTarget);
        }


        private static void Step9(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.DosisIII, SpellTargetType.CurrTarget);
        }

        private static void Step10(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.ToxikonII, SpellTargetType.CurrTarget);
        }
    }
}