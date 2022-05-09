﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AEAssist.AI;
using AEAssist.Define;
using AEAssist.Rotations.Core;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist.Helper
{
    public static class SpellHelper
    {
        private static readonly Dictionary<uint, SpellEntity> SpellEntities = new Dictionary<uint, SpellEntity>();

        public static SpellEntity GetSpellEntity(this uint id)
        {
            if (id == 0)
                return null;
            if (!SpellEntities.TryGetValue(id, out var entity))
            {
                if (DataManager.GetSpellData(id) == null)
                    return null;
                entity = new SpellEntity(id);
                if (SpellsDefine.TargetIsSelfs.Contains(id))
                    entity.SpellTargetType = SpellTargetType.Self;
                SpellEntities[id] = entity;
            }

            return entity;
        }

        public static async Task<bool> CastGCD(SpellData spell, GameObject target)
        {
            if (!CanCastGCD(spell, target))
                return false;
            if (spell.GroundTarget)
            {
                if (!ActionManager.DoActionLocation(spell.Id, target.Location))
                    return false;
            }
            else
            {
                if (!ActionManager.DoAction(spell, target))
                    return false;
            }

            if (spell.AdjustedCastTime != TimeSpan.Zero)
                if (!await Coroutine.Wait(spell.AdjustedCastTime - TimeSpan.FromMilliseconds(300), () => Core.Me.IsCasting))
                    return false;

            SpellEventMgr.Instance.Run(spell.Id);

            return true;
        }


        public static bool CanCastGCD(SpellData spell, GameObject target)
        {
            if (!ActionManager.HasSpell(spell.Id))
                return false;

            if (!GameSettingsManager.FaceTargetOnAction)
                GameSettingsManager.FaceTargetOnAction = true;

            if (spell.GroundTarget)
            {
                if (!ActionManager.CanCastLocation(spell.Id, target.Location))
                    return false;
            }
            else
            {
                if (AEAssist.DataBinding.Instance.EarlyDecisionMode && !SpellsDefine.IgnoreEarlyDecisionSet.Contains(spell.Id))
                {
                    if (!ActionManager.CanCastOrQueue(spell, target))
                        return false;
                }
                else
                {
                    if (!ActionManager.CanCast(spell, target))
                        return false;
                }
            }

            return true;
        }

        public static async Task<bool> CastAbility(SpellData spell, GameObject target, int waitTime = 0)
        {
            if (waitTime == 0)
                waitTime = SettingMgr.GetSetting<GeneralSettings>().AnimationLockMs;

            if (!ActionManager.HasSpell(spell.Id))
                return false;
            if (!GameSettingsManager.FaceTargetOnAction)
                GameSettingsManager.FaceTargetOnAction = true;

            if (!ActionManager.DoAction(spell, target))
                return false;

            var needTime = (int) spell.AdjustedCastTime.TotalMilliseconds + waitTime;
            SpellEventMgr.Instance.Run(spell.Id);
            if (needTime <= 10)
                return true;

            await Coroutine.Sleep(needTime);
            return true;
        }

        public static bool IsUnlock(this SpellData spellData)
        {
            if (Core.Me.ClassLevel < spellData.LevelAcquired
                || !ActionManager.HasSpell(spellData.Id))
                return false;

            if (AIRoot.GetBattleData<BattleData>().LockSpellId.Contains(spellData.Id))
                return false;

            return true;
        }

        public static bool IsUnlock(this uint spellId)
        {
            var spellData = spellId.GetSpellEntity().SpellData;
            return spellData.IsUnlock();
        }

        public static bool IsUnlock(this SpellEntity spellId)
        {
            var spellData = spellId.SpellData;
            return spellData.IsUnlock();
        }

        public static bool IsReady(this SpellData spellData)
        {
            if (!spellData.IsUnlock())
                return false;

            if (spellData.RecentlyUsed())
                return false;

            if (spellData.MaxCharges >= 1)
            {
                if (spellData.Charges >= 1)
                    return true;
                LogHelper.Debug(
                    $" {spellData.LocalizedName} Charge {spellData.Charges} MaxCharge {spellData.MaxCharges}!");

                if (spellData.SpellType == SpellType.Ability)
                    return false;
                var time = 0;
                if (AEAssist.DataBinding.Instance.EarlyDecisionMode)
                    time = SettingMgr.GetSetting<GeneralSettings>().AnimationLockMs;
                if (spellData.Cooldown.TotalMilliseconds > time)
                    return false;
                return true;
            }

            if (spellData.SpellType == SpellType.Ability)
                if (spellData.Cooldown.TotalMilliseconds > 0)
                    return false;

            return true;
        }

        public static bool IsReady(this uint spellId)
        {
            var spellData = spellId.GetSpellEntity().SpellData;
            return spellData.IsReady();
        }

        public static bool IsReady(this SpellEntity spell)
        {
            var spellData = spell.SpellData;
            return spellData.IsReady();
        }


        public static bool IsMaxChargeReady(this SpellData spellData, float delta = 0.5f)
        {
            var checkMax = spellData.MaxCharges - delta;
            if (!spellData.IsUnlock()
                || spellData.Charges < checkMax)
                return false;
            return true;
        }

        public static bool IsMaxChargeReady(this uint spellId, float delta = 0.5f)
        {
            var spellData = spellId.GetSpellEntity().SpellData;
            return spellData.IsMaxChargeReady(delta);
        }

        public static bool IsMaxChargeReady(this SpellEntity spellId, float delta = 0.5f)
        {
            var spellData = spellId.SpellData;
            return spellData.IsMaxChargeReady(delta);
        }

        public static bool CoolDownInGCDs(this SpellData spellData, int count)
        {
            var baseGCD = RotationManager.Instance.GetBaseGCDSpell().AdjustedCooldown.TotalMilliseconds;
            if (spellData.Cooldown.TotalMilliseconds <= baseGCD * count) return true;

            return false;
        }

        public static bool CoolDownInGCDs(this uint spellId, int count)
        {
            var SpellData = spellId.GetSpellEntity().SpellData;
            return SpellData.CoolDownInGCDs(count);
        }


        public static Task<bool> DoGCD(this uint spellId)
        {
            return spellId.GetSpellEntity().DoGCD();
        }

        public static Task<bool> DoAbility(this uint spellId)
        {
            return spellId.GetSpellEntity().DoAbility();
        }

        public static bool RecentlyUsed(this uint spellId)
        {
            return spellId.GetSpellEntity().RecentlyUsed();
        }
        
        
        public static uint GetInterruptSpell(ClassJobType job)
        {
            switch (job)
            {
                case ClassJobType.Machinist:
                    case ClassJobType.Bard:
                    case ClassJobType.Dancer:
                    return SpellsDefine.HeadGraze;
                case ClassJobType.Paladin:
                    case ClassJobType.DarkKnight:
                    case ClassJobType.Warrior:
                    case ClassJobType.Gunbreaker:
                    return SpellsDefine.Interject;
                
            }

            return 0;
        }

        public static uint GetLastComboSpell()
        {
            if (ActionManager.LastSpell == null)
                return 0;
            var mask = ActionManager.GetMaskedAction(ActionManager.LastSpell.Id);
            if (mask != null)
                return mask.Id;
            return ActionManager.LastSpell.Id;
        }
    }
}