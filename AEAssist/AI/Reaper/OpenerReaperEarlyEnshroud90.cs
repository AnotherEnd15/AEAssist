using System;
using System.Collections.Generic;
using AEAssist.Define;
using AEAssist.Opener;
using ff14bot;
using ff14bot.Enums;

namespace AEAssist.AI.Reaper
{
    [Opener(ClassJobType.Reaper, 90, "Early Enshroud")]
    public class OpenerReaperEarlyEnshroud90 : IOpener
    {
        public int Check()
        {
            // if (PartyManager.NumMembers <= 4 && !Core.Me.CurrentTarget.IsDummy())
            //     return -5;
            // if (!AEAssist.DataBinding.Instance.Burst)
            //     return -100;
            // if (!SpellsDefine.ArcaneCircle.IsReady())
            //     return -1;
            // if (!SpellsDefine.SoulSlice.IsMaxChargeReady(0.1f))
            //     return -2;
            // if (!SpellsDefine.Gluttony.CoolDownInGCDs(7))
            //     return -3;
            // if (!SpellsDefine.Enshroud.IsReady())
            //     return -4;
            // if (!Core.Me.CurrentTarget.IsBoss())
            //     return -5;
            
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
            Step10,
            Step11,
            Step12,
        };


        private static void Step0(SpellQueueSlot slot)
        {
            slot.SetGCD(ReaperSpellHelper.GetShadowOfDeath().Id, SpellTargetType.CurrTarget);
            slot.Abilitys.Enqueue((SpellsDefine.ArcaneCircle, SpellTargetType.Self));
        }


        private static void Step1(SpellQueueSlot slot)
        {
            var id = ReaperSpellHelper.CanUseSoulSlice_Scythe(Core.Me.CurrentTarget).Id;
            slot.SetGCD(id, SpellTargetType.CurrTarget);
        }


        private static void Step2(SpellQueueSlot slot)
        {
            var id = ReaperSpellHelper.CanUseSoulSlice_Scythe(Core.Me.CurrentTarget).Id;
            slot.SetGCD(id, SpellTargetType.CurrTarget);
        }


        private static void Step3(SpellQueueSlot slot)
        {
            slot.UsePotion = true;
        }


        private static void Step4(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.PlentifulHarvest, SpellTargetType.CurrTarget);
            slot.Abilitys.Enqueue((SpellsDefine.Enshroud, SpellTargetType.Self));
        }


        private static void Step5(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.VoidReaping, SpellTargetType.CurrTarget);
        }


        private static void Step6(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.CrossReaping, SpellTargetType.CurrTarget);
        }


        private static void Step7(SpellQueueSlot slot)
        {
            slot.Abilitys.Enqueue((SpellsDefine.LemuresSlice, SpellTargetType.CurrTarget));
            slot.SetGCD(SpellsDefine.VoidReaping, SpellTargetType.CurrTarget);
        }


        private static void Step8(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.CrossReaping, SpellTargetType.CurrTarget);
            slot.Abilitys.Enqueue((SpellsDefine.LemuresSlice, SpellTargetType.CurrTarget));
        }


        private static void Step9(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.Communio, SpellTargetType.CurrTarget);
            slot.Abilitys.Enqueue((SpellsDefine.Gluttony, SpellTargetType.CurrTarget));
        }

        private static void Step10(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.Gibbet, SpellTargetType.CurrTarget);
        }
        
        private static void Step11(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.Gallows, SpellTargetType.CurrTarget);
            slot.Abilitys.Enqueue((SpellsDefine.UnveiledGibbet, SpellTargetType.CurrTarget));
        }
        
        private static void Step12(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.Gibbet, SpellTargetType.CurrTarget);
        }
    }
}