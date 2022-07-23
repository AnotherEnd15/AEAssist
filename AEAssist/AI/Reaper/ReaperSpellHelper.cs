using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Reaper
{
    public static class ReaperSpellHelper
    {
        private static async Task<SpellEntity> UseAOECombo(GameObject target)
        {
            if (AIRoot.GetBattleData<ReaperBattleData>().CurrCombo != ReaperComboStages.NightmareScythe
                || ActionManager.ComboTimeLeft <= 0)
            {
                if (await SpellsDefine.SpinningScythe.DoGCD())
                {
                    AIRoot.GetBattleData<ReaperBattleData>().CurrCombo = ReaperComboStages.NightmareScythe;
                    return SpellsDefine.SpinningScythe.GetSpellEntity();
                }
            }
            else if (await SpellsDefine.NightmareScythe.DoGCD())
            {
                AIRoot.GetBattleData<ReaperBattleData>().CurrCombo = ReaperComboStages.SpinningScythe;
                return SpellsDefine.NightmareScythe.GetSpellEntity();
            }

            return null;
        }

        private static async Task<SpellEntity> UseSingleCombo(GameObject target)
        {
            if (ActionManager.ComboTimeLeft > 0)
            {
                if (AIRoot.GetBattleData<ReaperBattleData>().CurrCombo == ReaperComboStages.InfernalSlice)
                {
                    if (await SpellsDefine.InfernalSlice.DoGCD())
                    {
                        AIRoot.GetBattleData<ReaperBattleData>().CurrCombo = ReaperComboStages.Slice;
                        return SpellsDefine.InfernalSlice.GetSpellEntity();
                    }
                }
                else if (AIRoot.GetBattleData<ReaperBattleData>().CurrCombo == ReaperComboStages.WaxingSlice)
                {
                    if (await SpellsDefine.WaxingSlice.DoGCD())
                    {
                        AIRoot.GetBattleData<ReaperBattleData>().CurrCombo = ReaperComboStages.InfernalSlice;
                        return SpellsDefine.WaxingSlice.GetSpellEntity();
                    }
                }
            }

            if (await SpellsDefine.Slice.DoGCD())
            {
                AIRoot.GetBattleData<ReaperBattleData>().CurrCombo = ReaperComboStages.WaxingSlice;
                return SpellsDefine.Slice.GetSpellEntity();
            }

            return null;
        }

        public static async Task<SpellEntity> BaseGCDCombo(GameObject target)
        {
            if (TargetHelper.CheckNeedUseAOE(target, 5, 5)) return await UseAOECombo(target);

            return await UseSingleCombo(target);
        }

        public static SpellEntity CanUseSoulSlice_Scythe(GameObject target)
        {
            if (TargetHelper.CheckNeedUseAOE(target, 5, 5)) return SpellsDefine.SoulScythe.GetSpellEntity();

            return SpellsDefine.SoulSlice.GetSpellEntity();
        }

        public static SpellEntity Gibbit_Gallows(GameObject target)
        {
            if (!Core.Me.HasAura(AurasDefine.SoulReaver)) return null;

            if (SpellsDefine.Guillotine.IsUnlock() && TargetHelper.CheckNeedUseAOE(8, 8))
                return SpellsDefine.Guillotine.GetSpellEntity();

            if (Core.Me.HasAura(AurasDefine.EnhancedGibbet)) return SpellsDefine.Gibbet.GetSpellEntity();

            if (Core.Me.HasAura(AurasDefine.EnhancedGallows)) return SpellsDefine.Gallows.GetSpellEntity();

            if (SettingMgr.GetSetting<ReaperSettings>().GallowsPrefer)
                return SpellsDefine.Gallows.GetSpellEntity();
            return SpellsDefine.Gibbet.GetSpellEntity();
        }

        public static SpellEntity GetEnshroudGCDSpell(GameObject target)
        {
            if (!Core.Me.HasAura(AurasDefine.Enshrouded))
                return null;

            if (ActionResourceManager.Reaper.LemureShroud < 2
                && SpellsDefine.Communio.IsUnlock())
                return SpellsDefine.Communio.GetSpellEntity();

            if (TargetHelper.CheckNeedUseAOE(target, 8, 8)) return SpellsDefine.GrimReaping.GetSpellEntity();

            if (Core.Me.HasAura(AurasDefine.EnhancedVoidReaping))
                return SpellsDefine.VoidReaping.GetSpellEntity();

            if (Core.Me.HasAura(AurasDefine.EnhancedCrossReaping)) return SpellsDefine.CrossReaping.GetSpellEntity();

            return SpellsDefine.VoidReaping.GetSpellEntity();
        }

        public static int CheckCanUsePlentifulHarvest()
        {
            if (!SpellsDefine.PlentifulHarvest.IsUnlock())
                return -200;

            if (SpellsDefine.PlentifulHarvest.RecentlyUsed())
                return -300;

            var time = 0;
            if (AEAssist.DataBinding.Instance.EarlyDecisionMode)
                time = SettingMgr.GetSetting<GeneralSettings>().AnimationLockMs;

            // 死亡祭祀
            if (Core.Me.HasMyAuraWithTimeleft(AurasDefine.BloodsownCircle, time))
                return -201;

            // 妖异
            if (Core.Me.HasAura(AurasDefine.SoulReaver))
                return -202;

            // 死亡祭品
            if (!Core.Me.HasAura(AurasDefine.ImmortalSacrifice)) return -203;

            // 50点蓝条以上,而且神秘环buff没有接近消失,就延后
            if (ActionResourceManager.Reaper.ShroudGauge > 50 &&
                !Core.Me.ContainsMyInEndAura(AurasDefine.ArcaneCircle, 3000))
                return -204;

            return 200;
        }

        public static bool CheckIfNeedTrueNorth()
        {
            var target = Core.Me.CurrentTarget;
            var targetSpell = Gibbit_Gallows(target);

            if (targetSpell == SpellsDefine.Guillotine.GetSpellEntity())
                return false;

            if (target.IsFlanking && targetSpell == SpellsDefine.Gallows.GetSpellEntity()) return true;

            if (target.IsBehind && targetSpell == SpellsDefine.Gibbet.GetSpellEntity()) return true;

            return false;
        }

        public static async Task<SpellEntity> UseTruthNorth()
        {
            if (!AEAssist.DataBinding.Instance.UseTrueNorth)
                return null;
            if (!CheckIfNeedTrueNorth())
                return null;
            if (await SpellsDefine.TrueNorth.DoAbility()) return SpellsDefine.TrueNorth.GetSpellEntity();

            return null;
        }

        public static int ReadyToEnshroud()
        {
            if (AIRoot.Instance.CloseBurst)
                return -100;
            if (!DataBinding.Instance.UseEnshroud)
                return -99;
            if (SpellsDefine.Enshroud.RecentlyUsed() || Core.Me.HasAura(AurasDefine.Enshrouded))
                return -101;
            if (IfHasSoulReaver())
                return -102;
            if (!SpellsDefine.PlentifulHarvest.RecentlyUsed() && ActionResourceManager.Reaper.ShroudGauge < 50)
                return -103;

            if (TTKHelper.IsTargetTTK(Core.Me.CurrentTarget as Character))
                return -104;
            if (!Core.Me.CanAttackTargetInRange(Core.Me.CurrentTarget))
                return -105;

            var coolDown = SpellsDefine.ArcaneCircle.GetSpellEntity().Cooldown.TotalMilliseconds;

            if (AEAssist.SettingMgr.GetSetting<ReaperSettings>().DoubleEnshroudPrefer && !SpellsDefine.ArcaneCircle.IsUnlock())
            {
                return -106;
            }

            if (AEAssist.SettingMgr.GetSetting<ReaperSettings>().DoubleEnshroudPrefer
                && SpellsDefine.PlentifulHarvest.IsUnlock()
                && ActionResourceManager.Reaper.ShroudGauge < 90
                && coolDown > 5000
                && coolDown < ConstValue.ReaperDoubleEnshroudMaxCheckTime)
                return -107;

            if (PrepareEnterDoubleEnshroud())
                // 连击不能断,太亏
                if (ActionManager.ComboTimeLeft > 0.1f && ActionManager.ComboTimeLeft < 27) // 2*2.5 + 4*1.5+ 2*2.5+4*1.5+2.5 =24.5, and give 1 gcd to move
                    return -200;

            return 0;
        }

        public static bool PrepareEnterDoubleEnshroud()
        {
            if (AIRoot.Instance.CloseBurst)
                return false;
            if (!SpellsDefine.ArcaneCircle.IsUnlock() || !SpellsDefine.Enshroud.IsUnlock())
                return false;
            var coolDown = SpellsDefine.ArcaneCircle.GetSpellEntity().Cooldown.TotalMilliseconds;
            if (AEAssist.SettingMgr.GetSetting<ReaperSettings>().DoubleEnshroudPrefer
                && SpellsDefine.PlentifulHarvest.IsUnlock())
                if (coolDown <= 5000)
                    return true;

            return false;
        }

        public static bool IfHasSoulReaver()
        {
            if (SpellsDefine.Gluttony.RecentlyUsed()
                || SpellsDefine.BloodStalk.RecentlyUsed()
                || SpellsDefine.GrimSwathe.RecentlyUsed()
                || Core.Me.HasAura(AurasDefine.SoulReaver))
                return true;
            return false;
        }

        public static SpellEntity GetShadowOfDeath()
        {
            var spell = SpellsDefine.ShadowOfDeath;
            if (TargetHelper.CheckNeedUseAOE(5, 5)) spell = SpellsDefine.WhorlOfDeath;
            return spell.GetSpellEntity();
        }
        
        public static bool GibbetPOSCheck()
        {
            // Gibbet
            if (Core.Me.HasAura(AurasDefine.EnhancedGibbet) && !Core.Me.CurrentTarget.IsFlanking)
            {
                return true;
            }
            return false;
        }
        
        public static bool GallowsPOSCheck()
        {
            // Gallows
            if (Core.Me.HasAura(AurasDefine.EnhancedGallows) && !Core.Me.CurrentTarget.IsBehind)
            {
                return true;
            }
            
            return false;
        }
    }
}