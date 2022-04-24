﻿// -----------------------------------
// 
// 模块说明：机工 90级起手
// 
// 创建人员：AE
// 创建日期：2022-04-14
// -----------------------------------

using System;
using System.Collections.Generic;
using AEAssist.AI;
using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Opener;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist.AI
{
    [Opener(ClassJobType.Machinist,70)]
    public class Opener_MCH_70 : IOpener
    {
        public int Check()
        {
            if (ActionResourceManager.Machinist.Heat >= 50)
                return -1;
            if (ActionResourceManager.Machinist.Battery >= 50)
                return -2;
            if (ActionResourceManager.Machinist.OverheatRemaining.TotalMilliseconds>0)
                return -3;
            if (!SpellsDefine.BarrelStabilizer.IsReady())
                return -4;
            var automationQueen = MCHSpellHelper.GetAutomatonQueen();
            if (!automationQueen.IsReady())
                return -6;
            if (!SpellsDefine.Hypercharge.CoolDownInGCDs(4))
                return -7;
            if (!SpellsDefine.Wildfire.CoolDownInGCDs(3))
                return -8;
            return 0;
        }

        public int StepCount => 11;
        
        [OpenerStep(0)]
        SpellQueueSlot Step0()
        {
            var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();

            slot.GCDSpellId = MCHSpellHelper.GetDrillIfWithAOE().Id;
            slot.Abilitys.Enqueue((SpellsDefine.GaussRound.Id,SpellTargetType.CurrTarget));
            slot.Abilitys.Enqueue((SpellsDefine.Ricochet.Id,SpellTargetType.CurrTarget));
            return slot;
        }
        [OpenerStep(1)]
        SpellQueueSlot Step1()
        {
            var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();

            var air = MCHSpellHelper.GetAirAnchor();
            slot.GCDSpellId = air.Id;
            slot.Abilitys.Enqueue((SpellsDefine.BarrelStabilizer.Id,SpellTargetType.CurrTarget));
            return slot;
        }
        [OpenerStep(2)]
        SpellQueueSlot Step2()
        {
            var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();
            //todo: 根据情况返回AOE版本?
            slot.GCDSpellId = SpellsDefine.HeatedSplitShot.Id;
            return slot;
        }
        [OpenerStep(3)]
        SpellQueueSlot Step3()
        {
            var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();

            slot.GCDSpellId = SpellsDefine.HeatedSlugShot.Id;
            slot.Abilitys.Enqueue((SpellsDefine.GaussRound.Id,SpellTargetType.CurrTarget));
            slot.Abilitys.Enqueue((SpellsDefine.Ricochet.Id,SpellTargetType.CurrTarget));

            return slot;
        }
        [OpenerStep(4)]
        SpellQueueSlot Step4()
        {
            var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();

            slot.GCDSpellId = SpellsDefine.HeatedCleanShot.Id;
            slot.Abilitys.Enqueue((SpellsDefine.Hypercharge.Id,SpellTargetType.CurrTarget));
            slot.Abilitys.Enqueue((SpellsDefine.Wildfire.Id,SpellTargetType.CurrTarget));

            return slot;
        }
        
        [OpenerStep(5)]
        SpellQueueSlot Step6()
        {
            var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();

            //todo: 根据情况返回AOE版本?
            
            slot.GCDSpellId = MCHSpellHelper.GetUnderHyperChargeGCD().Id;
            return slot;
        }
        
        [OpenerStep(6)]
        SpellQueueSlot Step7()
        {
            var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();

            //todo: 根据情况返回AOE版本?
            
            slot.GCDSpellId = MCHSpellHelper.GetUnderHyperChargeGCD().Id;
            slot.Abilitys.Enqueue((SpellsDefine.GaussRound.Id, SpellTargetType.CurrTarget));
            return slot;
        }
        
        [OpenerStep(7)]
        SpellQueueSlot Step8()
        {
            var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();

            //todo: 根据情况返回AOE版本?
            
            slot.GCDSpellId = MCHSpellHelper.GetUnderHyperChargeGCD().Id;
            slot.Abilitys.Enqueue((SpellsDefine.Ricochet.Id, SpellTargetType.CurrTarget));

            return slot;
        }
        
              
        [OpenerStep(8)]
        SpellQueueSlot Step9()
        {
            var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();

            //todo: 根据情况返回AOE版本?
            
            slot.GCDSpellId = MCHSpellHelper.GetUnderHyperChargeGCD().Id;
            slot.Abilitys.Enqueue((SpellsDefine.GaussRound.Id, SpellTargetType.CurrTarget));
            return slot;
        }
        
             
        [OpenerStep(9)]
        SpellQueueSlot Step10()
        {
            var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();

            //todo: 根据情况返回AOE版本?
            
            slot.GCDSpellId = MCHSpellHelper.GetUnderHyperChargeGCD().Id;
            slot.Abilitys.Enqueue((SpellsDefine.Ricochet.Id, SpellTargetType.CurrTarget));

            return slot;
        }

        [OpenerStep(10)]
        SpellQueueSlot Step11()
        {
            var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();


            slot.GCDSpellId = SpellsDefine.Drill.Id;
    
            return slot;
        }
    }
}