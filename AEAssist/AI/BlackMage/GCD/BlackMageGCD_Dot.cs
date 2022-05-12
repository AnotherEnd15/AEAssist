﻿using System;
using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist.AI.BlackMage.GCD
{
    public class BlackMageGCD_Dot: IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            // if setting use DOT
            if (!AEAssist.DataBinding.Instance.UseDot)
                return -1;


            if (DotBlacklistHelper.IsBlackList(Core.Me.CurrentTarget as Character))
                return -10;

            if (Core.Me.CurrentMana < 400)
            {
                return -1;
            }
            
            
            // prevent casting same spell
            var BattleData = AIRoot.GetBattleData<BattleData>();
            if (BattleData.lastGCDSpell == SpellsDefine.Thunder.GetSpellEntity() ||
                BattleData.lastGCDSpell == SpellsDefine.Thunder2.GetSpellEntity() ||
                BattleData.lastGCDSpell == SpellsDefine.Thunder3.GetSpellEntity() ||
                BattleData.lastGCDSpell == SpellsDefine.Thunder4.GetSpellEntity()
               )
            {
                return -1;
            }

            // if we need to dot
            if (BlackMageHelper.IsTargetNeedThunder(Core.Me.CurrentTarget as Character, 5000))
            {
                // if we are in fire
                if (ActionResourceManager.BlackMage.AstralStacks > 0)
                {
                    // if we have more than 10 seconds left in fire
                    if (ActionResourceManager.BlackMage.StackTimer.TotalMilliseconds > 10000)
                    {
                        return 1;
                    }

                    if (BlackMageHelper.InstantCasting() &&
                        ActionResourceManager.BlackMage.StackTimer.TotalMilliseconds > 5000)
                    {
                        return 3;
                    }
                }
                // if we are in ice, cast straight away
                if (ActionResourceManager.BlackMage.UmbralStacks > 0)
                {
                    return 2;
                }
            }
            
            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = BlackMageHelper.GetThunder();
            if (spell == null)
                return null;
            var ret = await spell.DoGCD();
            if (ret)
                return spell;
            return null;
        }
    }
}