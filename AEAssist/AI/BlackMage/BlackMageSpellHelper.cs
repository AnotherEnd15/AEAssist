using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;
using System;
using System.Linq;

namespace AEAssist.AI.BlackMage
{
    public static class BlackMageHelper
    {
        public static bool CanCastFire4()
        {
            if (Core.Me.CurrentMana >= 2400 &&
                BlackMageHelper.IsMaxAstralStacks())
            {
                // not sure what numbders excatly to put here
                if (Core.Me.HasAura(AurasDefine.LeyLines) &&
                    ActionResourceManager.BlackMage.StackTimer.TotalMilliseconds > 5000)
                {
                    return true;
                }
                else if (ActionResourceManager.BlackMage.StackTimer.TotalMilliseconds > 5500)
                {
                    return true;
                }
            }

            return false;
        }

        public static ushort PolyglotTimer => ActionResourceManager.CostTypesStruct.timer;


        public static bool IsTargetNeedThunder(Character target, int timeLeft)
        {
            // check thunder 4,3,2,1
            var thunder4 = AurasDefine.Thunder4;
            var thunder3 = AurasDefine.Thunder3;
            var thunder2 = AurasDefine.Thunder2;
            var thunder1 = AurasDefine.Thunder;

            var lastGCDSpell = BlackMageHelper.GetLastSpell();
            if (lastGCDSpell == SpellsDefine.Thunder4 ||
                lastGCDSpell == SpellsDefine.Thunder3 ||
                lastGCDSpell == SpellsDefine.Thunder2 ||
                lastGCDSpell == SpellsDefine.Thunder)
            {
                return false;
            }

            // if not enough time left
            if (target.HasMyAura((uint)thunder4) &&
                !target.HasMyAuraWithTimeleft((uint)thunder4, timeLeft))
            {
                return true;
            }

            if (target.HasMyAura((uint)thunder3) &&
                !target.HasMyAuraWithTimeleft((uint)thunder3, timeLeft))
            {
                return true;
            }

            if (target.HasMyAura((uint)thunder2) &&
                !target.HasMyAuraWithTimeleft((uint)thunder2, timeLeft))
            {
                return true;
            }

            if (target.HasMyAura((uint)thunder1) &&
                !target.HasMyAuraWithTimeleft((uint)thunder1, timeLeft))
            {
                return true;
            }

            // if target has no dot from me, target needs dot
            if (!target.HasMyAura((uint)thunder4) &&
                !target.HasMyAura((uint)thunder3) &&
                !target.HasMyAura((uint)thunder2) &&
                !target.HasMyAura((uint)thunder1)
               )
            {
                return true;
            }

            return false;
        }

        public static bool LearnedParadox()
        {
            return Core.Me.ClassLevel >= ConstValue.ParadoxLevelAcquired;
        }

        public static bool IsParadoxReady()
        {
            if (!SpellsDefine.Paradox.IsUnlock())
            {
                return false;
            }
            // 1 for have echochian
            // 0 for no have echochian
            // 3 for have paradox and echochian
            // 2 for only have echochian
            if (ActionResourceManager.CostTypesStruct.offset_F > 2)
            {
                return true;
            }

            return false;
        }

        public static SpellEntity GetBlizzard3()
        {
            if (DataBinding.Instance.UseAOE)
            {
                var aoeCount = TargetHelper.GetNearbyEnemyCount(Core.Me.CurrentTarget, 25, 5);
                if (aoeCount >= ConstValue.BlackMageAOECount)
                {
                    if (SpellsDefine.HighBlizzardII.IsUnlock())
                    {
                        return SpellsDefine.HighBlizzardII.GetSpellEntity();
                    }

                    // blizzard2 100 * 3 > blizzard3 260
                    if (aoeCount >= 3)
                    {
                        if (SpellsDefine.Blizzard2.IsUnlock())
                        {
                            return SpellsDefine.Blizzard2.GetSpellEntity();
                        }
                    }
                }
            }

            if (SpellsDefine.Blizzard3.IsUnlock())
            {
                return SpellsDefine.Blizzard3.GetSpellEntity();
            }

            return null;
        }

        public static SpellEntity GetDespair()
        {
            if (DataBinding.Instance.UseAOE)
            {
                var aoeCount = TargetHelper.GetNearbyEnemyCount(Core.Me.CurrentTarget, 25, 5);
                if (aoeCount >= ConstValue.BlackMageAOECount)
                {
                    if (SpellsDefine.Flare.IsUnlock())
                    {
                        return SpellsDefine.Flare.GetSpellEntity();
                    }
                }
            }

            if (SpellsDefine.Despair.IsUnlock())
            {
                return SpellsDefine.Despair.GetSpellEntity();
            }

            return null;
        }

        public static SpellEntity GetXenoglossy()
        {
            if (DataBinding.Instance.UseAOE)
            {
                var aoeCount = TargetHelper.GetNearbyEnemyCount(Core.Me.CurrentTarget, 25, 5);
                if (aoeCount >= ConstValue.BlackMageAOECount)
                {
                    if (SpellsDefine.Foul.IsUnlock())
                    {
                        return SpellsDefine.Foul.GetSpellEntity();
                    }
                }
            }

            if (SpellsDefine.Xenoglossy.IsUnlock())
            {
                return SpellsDefine.Xenoglossy.GetSpellEntity();
            }

            return null;
        }

        public static uint GetLastSpell()
        {
            return ActionManager.LastSpell.Id;
        }

        public static bool CanMove()
        {
            // var BattleData = AIRoot.GetBattleData<BattleData>();
            if (Core.Me.HasAura(AurasDefine.Swiftcast) ||
                Core.Me.HasAura(AurasDefine.Triplecast) ||
                SpellsDefine.Triplecast.RecentlyUsed() ||
                SpellsDefine.Swiftcast.RecentlyUsed()
               )
                return true;
            if (Core.Me.IsCasting)
            {
                if (Core.Me.CastingSpellId == SpellsDefine.Fire4)
                {
                    var CurrentMana = Core.Me.CurrentMana;
                    //if currentMana != BattleData.Mana
                    //we can move
                    return true;
                }
                else
                {
                    // if ActionManager.LastSpell.Id; != 上一个ActionManager.LastSpell.Id; 
                    // we can move
                    MovementManager.MoveForwardStart();
                    MovementManager.MoveStop();
                }
            }

            return false;
        }

        public static bool test()
        {
            return true;
        }

        public static void GetPartyMembers()
        {
            var members = PartyManager.AllMembers.ToList();
            foreach (var member in members)
            {
                var name = member.Name.ToString();
                var job = member.Class.ToString();
                var health = member.CurrentHealth / member.MaxHealth;
            }
        }

        public static bool WillPolyglotOverflow(TimeSpan time)
        {
            var currentPolyglot = ActionResourceManager.BlackMage.PolyglotCount;
            // will have another one with Amplifier
            if (SpellsDefine.Amplifier.GetSpellEntity().Cooldown < time)
            {
                currentPolyglot += 1;
            }
            if (ActionResourceManager.BlackMage.PolyglotTimer < time)
            {
                currentPolyglot += 1;
            }

            if (currentPolyglot > 2)
            {
                return true;
            }

            return false;
        }

        public static SpellEntity GetParadox()
        {
            // if we learned paradox, go only paradox
            if (SpellsDefine.Paradox.IsUnlock())
            {
                return SpellsDefine.Paradox.GetSpellEntity();
            }

            // if we have not learned paradox, replace fire paradox with fire, nothing to go in ice
            if (SpellsDefine.Fire.IsUnlock())
            {
                return SpellsDefine.Fire.GetSpellEntity();
            }

            return null;
        }

        public static SpellEntity GetBlizzard4()
        {
            if (DataBinding.Instance.UseAOE)
            {
                var aoeCount = TargetHelper.GetNearbyEnemyCount(Core.Me.CurrentTarget, 25, 5);
                // freeze 120 * 3 > blizzard4 310
                if (aoeCount >= 3)
                {
                    if (SpellsDefine.Freeze.IsUnlock())
                    {
                        return SpellsDefine.Freeze.GetSpellEntity();
                    }
                }
            }

            if (SpellsDefine.Blizzard4.IsUnlock())
            {
                return SpellsDefine.Blizzard4.GetSpellEntity();
            }

            return null;
        }

        public static SpellEntity GetThunder()
        {
            if (DataBinding.Instance.UseAOE)
            {
                var aoeCount = TargetHelper.GetNearbyEnemyCount(Core.Me.CurrentTarget, 25, 5);
                // thunder4 20 * 2 > thunder 3 35
                if (aoeCount >= ConstValue.BlackMageAOECount)
                {
                    if (SpellsDefine.Thunder4.IsUnlock())
                    {
                        return SpellsDefine.Thunder4.GetSpellEntity();
                    }

                    if (SpellsDefine.Thunder2.IsUnlock())
                    {
                        return SpellsDefine.Thunder2.GetSpellEntity();
                    }

                    // thunder2 15 * 3 > thunder2 35
                    else
                    {
                        if (SpellsDefine.Thunder2.IsUnlock())
                        {
                            return SpellsDefine.Thunder2.GetSpellEntity();
                        }
                    }
                }
            }

            if (SpellsDefine.Thunder3.IsUnlock())
            {
                return SpellsDefine.Thunder3.GetSpellEntity();
            }

            if (SpellsDefine.Thunder.IsUnlock())
            {
                return SpellsDefine.Thunder.GetSpellEntity();
            }

            return null;
        }

        public static SpellEntity GetFire2()
        {
            if (SpellsDefine.HighFireII.IsUnlock())
            {
                return SpellsDefine.HighFireII.GetSpellEntity();
            }

            if (SpellsDefine.Fire2.IsUnlock())
            {
                return SpellsDefine.Fire2.GetSpellEntity();
            }

            return null;
        }

        public static SpellEntity GetFire4()
        {
            if (SpellsDefine.Fire4.IsUnlock())
            {
                return SpellsDefine.Fire4.GetSpellEntity();
            }

            if (SpellsDefine.Fire.IsUnlock())
            {
                return SpellsDefine.Fire.GetSpellEntity();
            }

            return null;
        }

        public static bool InstantCasting()
        {
            if (Core.Me.HasAura(AurasDefine.Triplecast) || Core.Me.HasAura(AurasDefine.Swiftcast))
            {
                return true;
            }

            if (SpellsDefine.Triplecast.RecentlyUsed() || SpellsDefine.Swiftcast.RecentlyUsed())
            {
                return true;
            }

            return false;
        }

        public static TimeSpan GetSpellCastTimeSpan(SpellEntity spell)
        {
            return spell.SpellData.AdjustedCastTime.Add(
                TimeSpan.FromMilliseconds(ConstValue.BlackMageLatencyCompensation));
        }

        public static int ThunderCheck()
        {
            // if setting use DOT
            if (!AEAssist.DataBinding.Instance.UseDot)
                return -1;

            // check black list
            if (DotBlacklistHelper.IsBlackList(Core.Me.CurrentTarget as Character))
                return -20;

            if (!SpellsDefine.Thunder.IsUnlock())
            {
                return -2;
            }

            if (Core.Me.CurrentMana < SpellsDefine.Thunder.GetSpellEntity().SpellData.Cost)
            {
                return -2;
            }

            // prevent casting same spell
            var bdls = AIRoot.GetBattleData<BattleData>().lastGCDSpell;

            if (bdls == SpellsDefine.Thunder.GetSpellEntity() ||
                bdls == SpellsDefine.Thunder2.GetSpellEntity() ||
                bdls == SpellsDefine.Thunder3.GetSpellEntity() ||
                bdls == SpellsDefine.Thunder4.GetSpellEntity()
               )
            {
                return -10;
            }

            // if we need to dot
            if (BlackMageHelper.IsTargetNeedThunder(Core.Me.CurrentTarget as Character, 5000))
            {
                // if we are in fire
                if (ActionResourceManager.BlackMage.AstralStacks > 0)
                {
                    // if we have more than 10 seconds left in fire
                    if (ActionResourceManager.BlackMage.StackTimer >
                        SpellsDefine.Scathe.GetSpellEntity().SpellData.AdjustedCooldown +
                        SpellsDefine.Flare.GetSpellEntity().SpellData.AdjustedCooldown &&
                        Core.Me.CurrentMana > 800 + SpellsDefine.Thunder.GetSpellEntity().SpellData.Cost)
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

        public static bool UmbralHeatsReady()
        {
            // so this shit is, we need lv58 to have umbral hearts, and no requirements
            // and lv58, we need to finish request for have the blizzard4 to single target

            // if we can't even have  umbralhearts, always pass the check
            if (Core.Me.ClassLevel < 58)
            {
                return true;
            }

            var lastGCDSpell = BlackMageHelper.GetLastSpell();
            if (ActionResourceManager.BlackMage.UmbralHearts == 3 ||
                lastGCDSpell == SpellsDefine.Freeze ||
                lastGCDSpell == SpellsDefine.Blizzard4
                )
            {
                return true;
            }

            return false;
        }

        public static bool IsMaxAstralStacks()
        {
            if (ActionResourceManager.BlackMage.AstralStacks == 3)
            {
                return true;
            }

            if (SpellsDefine.Fire2.RecentlyUsed() ||
                SpellsDefine.Fire3.RecentlyUsed() ||
                SpellsDefine.HighFireII.RecentlyUsed()
               )
            {
                return true;
            }

            return false;
        }

        public static bool IsUmbralFinished()
        {
            // if we are in ice
            if (ActionResourceManager.BlackMage.UmbralStacks > 0)
            {
                if (BlackMageHelper.UmbralHeatsReady() &&
                    !BlackMageHelper.IsParadoxReady() &&
                    Core.Me.CurrentMana == 10000)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsGCDOpen()
        {
            if (Core.Me.HasAura(AurasDefine.Triplecast))
            {
                return true;
            }

            if (SpellsDefine.Fire3.RecentlyUsed() ||
                SpellsDefine.Blizzard3.RecentlyUsed())
            {
                return true;
            }

            return false;
        }
    }
}