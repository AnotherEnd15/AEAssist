using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Opener;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using System;
using System.Collections.Generic;

namespace AEAssist.AI.Samurai
{
    [Opener(ClassJobType.Samurai, 90)]
    public class Opener_Samurai_90 : IOpener
    {
        public int Check()
        {
            if (!Core.Me.HasMyAuraWithTimeleft(AurasDefine.MeikyoShisui))
            {
                return -1;
            }
            
            if (!SpellsDefine.MeikyoShisui.IsReady())
                return -2;
            if (!SpellsDefine.Ikishoten.IsReady())
                return -3;
            if (!SpellsDefine.HissatsuSenei.IsReady())
                return -4;
            
            if (PartyManager.NumMembers <= 4 && !Core.Me.CurrentTarget.IsDummy())
                return -5;

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
            Step13,
            Step14,
            Step15,
            Step16,
            Step17,
            Step18,
            Step19,
            Step20,
            Step21,
            Step22,
            Step23,

        };


        private static void Step0(SpellQueueSlot slot)
        {
            LogHelper.Info("Opener_Step0 : Gekko" );
            slot.SetGCD(SpellsDefine.Gekko, SpellTargetType.CurrTarget);
            slot.UsePotion = true;
        }


        private static void Step1(SpellQueueSlot slot)
        {
            LogHelper.Info("Opener_Step1 : Kasha" );
            slot.SetGCD(SpellsDefine.Kasha, SpellTargetType.CurrTarget);
        }

        private static void Step2(SpellQueueSlot slot)
        {
            LogHelper.Info("Opener_Step2 : Ikishoten");
            slot.SetGCD(SpellsDefine.Ikishoten, SpellTargetType.CurrTarget);
            slot.Abilitys.Enqueue((SpellsDefine.Ikishoten, SpellTargetType.CurrTarget));
        }

        private static void Step3(SpellQueueSlot slot)
        {
            LogHelper.Info("Opener_Step3 : Yukikaze");
            slot.SetGCD(SpellsDefine.Yukikaze, SpellTargetType.CurrTarget);
        }
        
        private static void Step4(SpellQueueSlot slot)
        {
            if (ActionResourceManager.Samurai.Kenki >= 25)
            {
                LogHelper.Info("Opener_Step4 : HissatsuShinten");
                slot.Abilitys.Enqueue((SpellsDefine.HissatsuShinten, SpellTargetType.CurrTarget));
            }
        }
        
        private static void Step5(SpellQueueSlot slot)
        {
            LogHelper.Info("Opener_Step5 : MidareSetsugekka");
            slot.SetGCD(SpellsDefine.MidareSetsugekka, SpellTargetType.CurrTarget);
        }
        
        private static void Step6(SpellQueueSlot slot)
        {
            LogHelper.Info("Opener_Step6 : KaeshiSetsugekka");
            slot.SetGCD(SpellsDefine.KaeshiSetsugekka, SpellTargetType.CurrTarget);
        }
        
        private static void Step7(SpellQueueSlot slot)
        {
            LogHelper.Info("Opener_Step7 : HissatsuSenei");
            slot.Abilitys.Enqueue((SpellsDefine.HissatsuSenei, SpellTargetType.CurrTarget));
        }
        
        private static void Step8(SpellQueueSlot slot)
        {
            LogHelper.Info("Opener_Step8 : MeikyoShisui");
            slot.Abilitys.Enqueue((SpellsDefine.MeikyoShisui, SpellTargetType.Self));
        }
        
        private static void Step9(SpellQueueSlot slot)
        {
            LogHelper.Info("Opener_Step9 : Gekko");
            slot.SetGCD(SpellsDefine.Gekko, SpellTargetType.CurrTarget);
        }
        
        private static void Step10(SpellQueueSlot slot)
        {
            if (ActionResourceManager.Samurai.Kenki > 25)
            {
                LogHelper.Info("Opener_Step10 : HissatsuShinten");
                slot.Abilitys.Enqueue((SpellsDefine.HissatsuShinten, SpellTargetType.CurrTarget));
            }
        }
        
        private static void Step11(SpellQueueSlot slot)
        {
            LogHelper.Info("Opener_Step11 : Higanbana");
            slot.SetGCD(SpellsDefine.Higanbana, SpellTargetType.CurrTarget);
        }
        
        private static void Step12(SpellQueueSlot slot)
        {
            LogHelper.Info("Opener_Step12 : Kasha");
            slot.SetGCD(SpellsDefine.Kasha, SpellTargetType.CurrTarget);
        }
        
        private static void Step13(SpellQueueSlot slot)
        {
            if (ActionResourceManager.Samurai.Kenki >= 25)
            {
                LogHelper.Info("Opener_Step13 : HissatsuShinten");
                slot.Abilitys.Enqueue((SpellsDefine.HissatsuShinten, SpellTargetType.CurrTarget));
            }
        }
        
        private static void Step14(SpellQueueSlot slot)
        {
            LogHelper.Info("Opener_Step14 : OgiNamikiri");
            slot.SetGCD(SpellsDefine.OgiNamikiri, SpellTargetType.CurrTarget);
        }
        
        private static void Step15(SpellQueueSlot slot)
        {
            LogHelper.Info("Opener_Step15 : KaeshiNamikiri");
            slot.Abilitys.Enqueue((SpellsDefine.KaeshiNamikiri, SpellTargetType.CurrTarget));
        }
        
        private static void Step16(SpellQueueSlot slot)
        {
            LogHelper.Info("Opener_Step16 : Shoha");
            slot.Abilitys.Enqueue((SpellsDefine.Shoha, SpellTargetType.CurrTarget));
        }
        
        private static void Step17(SpellQueueSlot slot)
        {
            LogHelper.Info("Opener_Step17 : Gekko");
            slot.SetGCD(SpellsDefine.Gekko, SpellTargetType.CurrTarget);
        }
        
        private static void Step18(SpellQueueSlot slot)
        {
            if (ActionResourceManager.Samurai.Kenki >= 25)
            {
                LogHelper.Info("Opener_Step18 : HissatsuShinten");
                slot.Abilitys.Enqueue((SpellsDefine.HissatsuShinten, SpellTargetType.CurrTarget));
            }
        }
        
        private static void Step19(SpellQueueSlot slot)
        {
            LogHelper.Info("Opener_Step19 : Hakaze");
            slot.SetGCD(SpellsDefine.Hakaze, SpellTargetType.CurrTarget);
        }
        
        private static void Step20(SpellQueueSlot slot)
        {
            LogHelper.Info("Opener_Step20 : Yukikaze");
            slot.SetGCD(SpellsDefine.Yukikaze, SpellTargetType.CurrTarget);
        }
        
        private static void Step21(SpellQueueSlot slot)
        {
            if (ActionResourceManager.Samurai.Kenki >= 25)
            {
                LogHelper.Info("Opener_Step21 : HissatsuShinten");
                slot.Abilitys.Enqueue((SpellsDefine.HissatsuShinten, SpellTargetType.CurrTarget));
            }
        }
        
        private static void Step22(SpellQueueSlot slot)
        {
            LogHelper.Info("Opener_Step22 : MidareSetsugekka");
            slot.SetGCD(SpellsDefine.MidareSetsugekka, SpellTargetType.CurrTarget);
        }
        
        private static void Step23(SpellQueueSlot slot)
        {
            LogHelper.Info("Opener_Step22 : KaeshiSetsugekka");
            slot.SetGCD(SpellsDefine.KaeshiSetsugekka, SpellTargetType.CurrTarget);
        }
    }
}