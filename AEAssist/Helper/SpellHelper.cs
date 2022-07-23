using AEAssist.AI;
using AEAssist.Define;
using AEAssist.Rotations.Core;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Objects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            var ret = CanCastGCD(spell, target);
            LogHelper.Debug($"22222222  {spell.LocalizedName} {target.Name} ret: {ret}");
            if (ret < 0)
            {
                return false;
            }

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
            LogHelper.Debug($"333333333  {spell.LocalizedName} {target.Name}");
            if (spell.AdjustedCastTime != TimeSpan.Zero)
            {
                if (!await Coroutine.Wait(spell.AdjustedCastTime - TimeSpan.FromMilliseconds(300),
                    () => Core.Me.IsCasting))
                    return false;
            }
            else
                await Coroutine.Sleep(300);
            SpellEventMgr.Instance.Run(spell.Id);
            LogHelper.Debug($"4444444444  {spell.LocalizedName} {target.Name}");
            return true;
        }


        public static int CanCastGCD(SpellData spell, GameObject target)
        {
            if (!ActionManager.HasSpell(spell.Id))
                return -1;

            if (!GameSettingsManager.FaceTargetOnAction)
                GameSettingsManager.FaceTargetOnAction = true;

            if (spell.GroundTarget)
            {
                if (!ActionManager.CanCastLocation(spell.Id, target.Location))
                    return -2;
            }
            else
            {
                if (AEAssist.DataBinding.Instance.EarlyDecisionMode && !SpellsDefine.IgnoreEarlyDecisionSet.Contains(spell.Id))
                {
                    if (!ActionManager.CanCastOrQueue(spell, target))
                        return -3;
                }
                else
                {
                    if (!ActionManager.CanCast(spell, target))
                        return -4;
                }
            }

            return 0;
        }

        public static async Task<bool> CastAbility(SpellData spell, GameObject target, int waitTime = 0)
        {
            if (waitTime == 0)
                waitTime = SettingMgr.GetSetting<GeneralSettings>().AnimationLockMs;

            if (!ActionManager.HasSpell(spell.Id))
                return false;
            if (!GameSettingsManager.FaceTargetOnAction)
                GameSettingsManager.FaceTargetOnAction = true;


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




            var needTime = (int)spell.AdjustedCastTime.TotalMilliseconds + waitTime;
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

            if (spellData.Charges >= 1)
                return true;
            LogHelper.Debug(
                $" {spellData.LocalizedName} Charge {spellData.Charges} MaxCharge {spellData.MaxCharges}!");

            var time = 0;
            if (AEAssist.DataBinding.Instance.EarlyDecisionMode)
                time = SettingMgr.GetSetting<GeneralSettings>().ActionQueueMs;

            if (spellData.SpellType == SpellType.Ability && !SpellsDefine.AbilityAsGCDSet.Contains(spellData.Id))
                time = 100;

            if (spellData.Cooldown.TotalMilliseconds > time)
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

        public static bool AbilityCoolDownInNextXGCDsWindow(this SpellData spellData, int count)
        {
            var baseGCDTime = RotationManager.Instance.GetBaseGCDSpell().AdjustedCooldown.TotalMilliseconds;
            var TargetSpellCoolDown = spellData.Cooldown.TotalMilliseconds;
            //come up in this window
            var delta = TimeHelper.Now() - (AIRoot.GetBattleData<BattleData>().lastCastTime + 300);
            if (delta + TargetSpellCoolDown <
                baseGCDTime * (count + 1) - SettingMgr.GetSetting<GeneralSettings>().ActionQueueMs)
            {
                return true;
            }
            return false;
        }

        public static bool AbilityCoolDownInNextXGCDsWindow(this uint spellId, int count)
        {
            var SpellData = spellId.GetSpellEntity().SpellData;
            return SpellData.AbilityCoolDownInNextXGCDsWindow(count);
        }

        public static int AbilityComesInNextXGCDsWindow(this SpellData spellData)
        {
            var baseGCDTime = RotationManager.Instance.GetBaseGCDSpell().AdjustedCooldown.TotalMilliseconds;
            var TargetSpellCoolDown = spellData.Cooldown.TotalMilliseconds;
            if (spellData.Cooldown == TimeSpan.Zero)
            {
                return 0;
            }
            var delta = TimeHelper.Now() - AIRoot.GetBattleData<BattleData>().lastCastTime;
            int count = 0;
            while (true)
            {
                if (delta + TargetSpellCoolDown <
                    baseGCDTime * (count + 1) - SettingMgr.GetSetting<GeneralSettings>().ActionQueueMs)
                {
                    return count;
                }
                count++;
            }
        }
        
        public static int AbilityComesInNextXGCDsWindow(this uint spellId)
        {
            var SpellData = spellId.GetSpellEntity().SpellData;
            return SpellData.AbilityComesInNextXGCDsWindow();
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