using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Objects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AEAssist.AI.Astrologian
{
    internal class AstSpellHelper
    {
        public static SpellEntity GetBaseGcd()
        {
            return SpellsDefine.Malefic.GetSpellEntity();
        }
        public static SpellEntity GetGravity()
        {
            if (!SpellsDefine.Gravity.IsUnlock())
            {
                LogHelper.Debug("Gravity not unlocked. skipping.");
                return null;
            }
            return SpellsDefine.Gravity.GetSpellEntity();
        }
        public static SpellEntity GetCombust()
        {
            if (!SpellsDefine.Combust.IsUnlock())
            {
                LogHelper.Debug("Combust not unlocked. skipping.");
                return null;
            }
            return SpellsDefine.Combust.GetSpellEntity();
        }
        private static int GetCombustAura()
        {
            LogHelper.Debug("Checking if Combust is unlocked...");
            if (!SpellsDefine.Combust.IsUnlock())
            {
                LogHelper.Debug("Combust not unlocked...");
                return 0;
            }
            LogHelper.Debug("Checking if Combust2 is unlocked...");
            if (!SpellsDefine.Combust2.IsUnlock())
            {
                LogHelper.Debug("Combust2 not unlocked.. trying to use Combust instead.. ");
                return AurasDefine.Combust;
            }
            LogHelper.Debug("Checking if Combust3 is unlocked...");
            if (!SpellsDefine.Combust3.IsUnlock())
            {
                LogHelper.Debug("Combust3 not unlocked.. trying to use Combust32 instead.. ");
                return AurasDefine.Combust2;
            }
            return AurasDefine.Combust3;
        }
        public static bool IsTargetHasAuraCombust(Character target)
        {
            var id = GetCombustAura();
            LogHelper.Debug("Checking if target has Combust: " + target.EnglishName);
            return id == 0 || target.HasMyAuraWithTimeleft((uint)id);
        }
        public static void RecordCombust()
        {
            var targetId = Core.Me.CurrentTarget.ObjectId;
            AIRoot.GetBattleData<AstBattleData>().lastCombustWithObj[targetId] = true;
        }
        public static void RemoveRecordCombust()
        {
            var targetId = Core.Me.CurrentTarget.ObjectId;
            AIRoot.GetBattleData<AstBattleData>().lastCombustWithObj[targetId] = false;
        }
        public static bool IsTargetNeedCombust(Character target, int timeLeft)
        {
            var CombustId = GetCombustAura();
            LogHelper.Debug("Checking if target need Combust id: " + CombustId);
            if (CombustId == 0) return false;

            var ttkAero = SettingMgr.GetSetting<AstSettings>().TTK_Aero;
            if (Core.Me.ClassLevel < 46)
            {
                ttkAero = 18;
            }
            bool NormalCheck()
            {
                if (DataBinding.Instance.EarlyDecisionMode)
                    timeLeft += SettingMgr.GetSetting<GeneralSettings>().ActionQueueMs;
                return !target.HasMyAuraWithTimeleft((uint)CombustId, timeLeft);
            }

            if (AIRoot.GetBattleData<AstBattleData>().IsTargetLastCombust()) return NormalCheck();
            if (ttkAero > 0 && target.HasMyAuraWithTimeleft((uint)CombustId, ttkAero * 1000) &&
                TTKHelper.IsTargetTTK(target, ttkAero, false))
                return NormalCheck();

            return NormalCheck();
        }
        private static int GetWeight(Character c)
        {
            switch (c.CurrentJob)
            {
                case ClassJobType.Astrologian:
                    return SettingMgr.GetSetting<AstSettings>().AstCardWeight;

                case ClassJobType.Monk:
                case ClassJobType.Pugilist:
                    return SettingMgr.GetSetting<AstSettings>().MnkCardWeight;

                case ClassJobType.BlackMage:
                case ClassJobType.Thaumaturge:
                    return SettingMgr.GetSetting<AstSettings>().BlmCardWeight;

                case ClassJobType.Dragoon:
                case ClassJobType.Lancer:
                    return SettingMgr.GetSetting<AstSettings>().DrgCardWeight;

                case ClassJobType.Samurai:
                    return SettingMgr.GetSetting<AstSettings>().SamCardWeight;

                case ClassJobType.Machinist:
                    return SettingMgr.GetSetting<AstSettings>().MchCardWeight;

                case ClassJobType.Summoner:
                case ClassJobType.Arcanist:
                    return SettingMgr.GetSetting<AstSettings>().SmnCardWeight;

                case ClassJobType.Bard:
                case ClassJobType.Archer:
                    return SettingMgr.GetSetting<AstSettings>().BrdCardWeight;

                case ClassJobType.Ninja:
                case ClassJobType.Rogue:
                    return SettingMgr.GetSetting<AstSettings>().NinCardWeight;

                case ClassJobType.RedMage:
                    return SettingMgr.GetSetting<AstSettings>().RdmCardWeight;

                case ClassJobType.Dancer:
                    return SettingMgr.GetSetting<AstSettings>().DncCardWeight;

                case ClassJobType.Paladin:
                case ClassJobType.Gladiator:
                    return SettingMgr.GetSetting<AstSettings>().PldCardWeight;

                case ClassJobType.Warrior:
                case ClassJobType.Marauder:
                    return SettingMgr.GetSetting<AstSettings>().WarCardWeight;

                case ClassJobType.DarkKnight:
                    return SettingMgr.GetSetting<AstSettings>().DrkCardWeight;

                case ClassJobType.Gunbreaker:
                    return SettingMgr.GetSetting<AstSettings>().GnbCardWeight;

                case ClassJobType.WhiteMage:
                case ClassJobType.Conjurer:
                    return SettingMgr.GetSetting<AstSettings>().WhmCardWeight;

                case ClassJobType.Scholar:
                    return SettingMgr.GetSetting<AstSettings>().SchCardWeight;

                case ClassJobType.Reaper:
                    return SettingMgr.GetSetting<AstSettings>().RprCardWeight;

                case ClassJobType.Sage:
                    return SettingMgr.GetSetting<AstSettings>().SgeCardWeight;

                case ClassJobType.BlueMage:
                    return SettingMgr.GetSetting<AstSettings>().BluCardWeight;
            }

            return c.CurrentJob == ClassJobType.Adventurer ? 70 : 0;
        }
        private static int GetHalfWeight(Character c)
        {
            switch (c.CurrentJob)
            {
                case ClassJobType.Astrologian:
                    return SettingMgr.GetSetting<AstSettings>().AstHalfCardWeight;

                case ClassJobType.Monk:
                case ClassJobType.Pugilist:
                    return SettingMgr.GetSetting<AstSettings>().MnkHalfCardWeight;

                case ClassJobType.BlackMage:
                case ClassJobType.Thaumaturge:
                    return SettingMgr.GetSetting<AstSettings>().BlmHalfCardWeight;

                case ClassJobType.Dragoon:
                case ClassJobType.Lancer:
                    return SettingMgr.GetSetting<AstSettings>().DrgHalfCardWeight;

                case ClassJobType.Samurai:
                    return SettingMgr.GetSetting<AstSettings>().SamHalfCardWeight;

                case ClassJobType.Machinist:
                    return SettingMgr.GetSetting<AstSettings>().MchHalfCardWeight;

                case ClassJobType.Summoner:
                case ClassJobType.Arcanist:
                    return SettingMgr.GetSetting<AstSettings>().SmnHalfCardWeight;

                case ClassJobType.Bard:
                case ClassJobType.Archer:
                    return SettingMgr.GetSetting<AstSettings>().BrdHalfCardWeight;

                case ClassJobType.Ninja:
                case ClassJobType.Rogue:
                    return SettingMgr.GetSetting<AstSettings>().NinHalfCardWeight;

                case ClassJobType.RedMage:
                    return SettingMgr.GetSetting<AstSettings>().RdmHalfCardWeight;

                case ClassJobType.Dancer:
                    return SettingMgr.GetSetting<AstSettings>().DncHalfCardWeight;

                case ClassJobType.Paladin:
                case ClassJobType.Gladiator:
                    return SettingMgr.GetSetting<AstSettings>().PldHalfCardWeight;

                case ClassJobType.Warrior:
                case ClassJobType.Marauder:
                    return SettingMgr.GetSetting<AstSettings>().WarHalfCardWeight;

                case ClassJobType.DarkKnight:
                    return SettingMgr.GetSetting<AstSettings>().DrkHalfCardWeight;

                case ClassJobType.Gunbreaker:
                    return SettingMgr.GetSetting<AstSettings>().GnbHalfCardWeight;

                case ClassJobType.WhiteMage:
                case ClassJobType.Conjurer:
                    return SettingMgr.GetSetting<AstSettings>().WhmHalfCardWeight;

                case ClassJobType.Scholar:
                    return SettingMgr.GetSetting<AstSettings>().SchHalfCardWeight;

                case ClassJobType.Reaper:
                    return SettingMgr.GetSetting<AstSettings>().RprHalfCardWeight;

                case ClassJobType.Sage:
                    return SettingMgr.GetSetting<AstSettings>().SgeHalfCardWeight;

                case ClassJobType.BlueMage:
                    return SettingMgr.GetSetting<AstSettings>().BluHalfCardWeight;
            }

            return c.CurrentJob == ClassJobType.Adventurer ? 70 : 0;
        }
        private static float GetCurrentHealthPercent(Character c)
        {
            return c.CurrentHealthPercent;
        }
        public static async Task<bool> CastMeleeCard()
        {

            if (GroupHelper.InParty)
            {
                var skillTarget = GroupHelper.CastableAlliesWithin30.Where(a => !a.HasMyAura(AurasDefine.TheArrow) && !a.HasMyAura(AurasDefine.TheBalance) && !a.HasMyAura(AurasDefine.TheSpear) && a.CurrentHealth > 0 && (a.IsTank() || a.IsMeleeDps())).OrderBy(GetWeight);
                LogHelper.Debug(Convert.ToString(skillTarget));
                LogHelper.Debug(Convert.ToString(skillTarget.FirstOrDefault()));
                var skillt = GroupHelper.CastableAlliesWithin30;
                foreach (Character chara in skillTarget)
                    LogHelper.Debug(Convert.ToString(chara));
                foreach (Character chara in skillt)
                    LogHelper.Debug(Convert.ToString(chara) + Convert.ToString(chara.IsMeleeDps()));
                if (skillTarget.FirstOrDefault() == null)
                    return false;

                //return await Spells.Play.Cast(ally.FirstOrDefault());
                //var skillTarget = GroupHelper.CastableAlliesWithin30.FirstOrDefault(r => r.CurrentHealth > 0 && r.CurrentHealthPercent <= SettingMgr.GetSetting<WhiteMageSettings>().TetragrammatonHp);
                if (!SpellsDefine.Draw.IsUnlock()) return false;
                var spell = new SpellEntity(SpellsDefine.Play, skillTarget.FirstOrDefault() as BattleCharacter);
                return await spell.DoAbility();
                //await CastTetragrammaton(skillTarget);
            }
            else
            {
                if (!Core.Me.HasAura(AurasDefine.TheArrow) && !Core.Me.HasAura(AurasDefine.TheBalance) && !Core.Me.HasAura(AurasDefine.TheBole) && !Core.Me.HasAura(AurasDefine.TheEwer) && !Core.Me.HasAura(AurasDefine.TheSpear) && !Core.Me.HasAura(AurasDefine.TheSpire))
                {
                    if (!SpellsDefine.Draw.IsUnlock()) return false;
                    var spell = new SpellEntity(SpellsDefine.Play, Core.Me as BattleCharacter);
                    return await spell.DoAbility();
                }
                
            }
            return false;
        }
        public static async Task<bool> CastRangedCard()
        {

            if (GroupHelper.InParty)
            {
                var skillTarget = GroupHelper.CastableAlliesWithin30.Where(a => !a.HasMyAura(AurasDefine.TheBole) && !a.HasMyAura(AurasDefine.TheEwer) && !a.HasMyAura(AurasDefine.TheSpire) && a.CurrentHealth > 0 && (a.IsRangedDpsCard())).OrderBy(GetWeight);
                LogHelper.Debug(Convert.ToString(skillTarget));
                LogHelper.Debug(Convert.ToString(skillTarget.FirstOrDefault()));
                var skillt = GroupHelper.CastableAlliesWithin30;
                foreach (Character chara in skillt)
                    LogHelper.Debug(Convert.ToString(chara) + Convert.ToString(chara.IsRangedDpsCard()));
                foreach (Character chara in skillTarget)
                    LogHelper.Debug(Convert.ToString(chara));
                if (skillTarget.FirstOrDefault() == null)
                    return false;

                //return await Spells.Play.Cast(ally.FirstOrDefault());
                //var skillTarget = GroupHelper.CastableAlliesWithin30.FirstOrDefault(r => r.CurrentHealth > 0 && r.CurrentHealthPercent <= SettingMgr.GetSetting<WhiteMageSettings>().TetragrammatonHp);
                if (!SpellsDefine.Draw.IsUnlock()) return false;
                var spell = new SpellEntity(SpellsDefine.Play, skillTarget.FirstOrDefault() as BattleCharacter);
                AIRoot.GetBattleData<AstBattleData>().AstNum = AIRoot.GetBattleData<AstBattleData>().AstNum + 1;
                return await spell.DoAbility();
                //await CastTetragrammaton(skillTarget);
            }
            else
            {
                if (!Core.Me.HasAura(AurasDefine.TheArrow) && !Core.Me.HasAura(AurasDefine.TheBalance) && !Core.Me.HasAura(AurasDefine.TheBole) && !Core.Me.HasAura(AurasDefine.TheEwer) && !Core.Me.HasAura(AurasDefine.TheSpear) && !Core.Me.HasAura(AurasDefine.TheSpire))
                {
                    if (!SpellsDefine.Draw.IsUnlock()) return false;
                    var spell = new SpellEntity(SpellsDefine.Play, Core.Me as BattleCharacter);
                    return await spell.DoAbility();
                }
            }
            return false;
        }
        public static async Task<bool> CastMeleeCardHalf()
        {

            if (GroupHelper.InParty)
            {
                var skillTarget = GroupHelper.CastableAlliesWithin30.Where(a => !a.HasMyAura(AurasDefine.TheArrow) && !a.HasMyAura(AurasDefine.TheBalance) && !a.HasMyAura(AurasDefine.TheSpear) && a.CurrentHealth > 0 && (a.IsTank() || a.IsMeleeDps())).OrderBy(GetHalfWeight);
                LogHelper.Debug(Convert.ToString(skillTarget));
                LogHelper.Debug(Convert.ToString(skillTarget.FirstOrDefault()));
                var skillt = GroupHelper.CastableAlliesWithin30;
                foreach (Character chara in skillTarget)
                    LogHelper.Debug(Convert.ToString(chara));
                foreach (Character chara in skillt)
                    LogHelper.Debug(Convert.ToString(chara) + Convert.ToString(chara.IsMeleeDps()));
                if (skillTarget.FirstOrDefault() == null)
                    return false;

                //return await Spells.Play.Cast(ally.FirstOrDefault());
                //var skillTarget = GroupHelper.CastableAlliesWithin30.FirstOrDefault(r => r.CurrentHealth > 0 && r.CurrentHealthPercent <= SettingMgr.GetSetting<WhiteMageSettings>().TetragrammatonHp);
                if (!SpellsDefine.Draw.IsUnlock()) return false;
                var spell = new SpellEntity(SpellsDefine.Play, skillTarget.FirstOrDefault() as BattleCharacter);
                AIRoot.GetBattleData<AstBattleData>().AstNum = AIRoot.GetBattleData<AstBattleData>().AstNum + 1;
                return await spell.DoAbility();
                //await CastTetragrammaton(skillTarget);
            }
            else
            {
                if (!Core.Me.HasAura(AurasDefine.TheArrow) && !Core.Me.HasAura(AurasDefine.TheBalance) && !Core.Me.HasAura(AurasDefine.TheBole) && !Core.Me.HasAura(AurasDefine.TheEwer) && !Core.Me.HasAura(AurasDefine.TheSpear) && !Core.Me.HasAura(AurasDefine.TheSpire))
                {
                    if (!SpellsDefine.Draw.IsUnlock()) return false;
                    var spell = new SpellEntity(SpellsDefine.Play, Core.Me as BattleCharacter);
                    return await spell.DoAbility();
                }
            }
            return false;
        }
        public static async Task<bool> CastRangedCardHalf()
        {

            if (GroupHelper.InParty)
            {
                var skillTarget = GroupHelper.CastableAlliesWithin30.Where(a => !a.HasMyAura(AurasDefine.TheBole) && !a.HasMyAura(AurasDefine.TheEwer) && !a.HasMyAura(AurasDefine.TheSpire) && a.CurrentHealth > 0 && (a.IsRangedDpsCard())).OrderBy(GetHalfWeight);
                LogHelper.Debug(Convert.ToString(skillTarget));
                LogHelper.Debug(Convert.ToString(skillTarget.FirstOrDefault()));
                var skillt = GroupHelper.CastableAlliesWithin30;
                foreach (Character chara in skillt)
                    LogHelper.Debug(Convert.ToString(chara) + Convert.ToString(chara.IsRangedDpsCard()));
                foreach (Character chara in skillTarget)
                    LogHelper.Debug(Convert.ToString(chara));
                if (skillTarget.FirstOrDefault() == null)
                    return false;

                //return await Spells.Play.Cast(ally.FirstOrDefault());
                //var skillTarget = GroupHelper.CastableAlliesWithin30.FirstOrDefault(r => r.CurrentHealth > 0 && r.CurrentHealthPercent <= SettingMgr.GetSetting<WhiteMageSettings>().TetragrammatonHp);
                if (!SpellsDefine.Draw.IsUnlock()) return false;
                var spell = new SpellEntity(SpellsDefine.Play, skillTarget.FirstOrDefault() as BattleCharacter);
                AIRoot.GetBattleData<AstBattleData>().AstNum = AIRoot.GetBattleData<AstBattleData>().AstNum + 1;
                return await spell.DoAbility();
                //await CastTetragrammaton(skillTarget);
            }
            else
            {
                if (!Core.Me.HasAura(AurasDefine.TheArrow) && !Core.Me.HasAura(AurasDefine.TheBalance) && !Core.Me.HasAura(AurasDefine.TheBole) && !Core.Me.HasAura(AurasDefine.TheEwer) && !Core.Me.HasAura(AurasDefine.TheSpear) && !Core.Me.HasAura(AurasDefine.TheSpire))
                {
                    if (!SpellsDefine.Draw.IsUnlock()) return false;
                    var spell = new SpellEntity(SpellsDefine.Play, Core.Me as BattleCharacter);
                    return await spell.DoAbility();
                }
            }
            return false;
        }
        public static async Task<SpellEntity> CastResPriority()
        {
            var priority = SettingMgr.GetSetting<AstSettings>().AstResPriority;
            var deadAllies = GroupHelper.DeadAllies;

            switch (priority)
            {
                // Healer>Tanks>DPS
                case 0:
                    LogHelper.Debug("Healer>Tanks>DPS-RESSING");
                    foreach (var deadAlly in deadAllies)
                    {
                        // check if the player already ressed.
                        LogHelper.Debug("checking if the player already rezzed if so skipping.");
                        if (deadAlly.HasAura(AurasDefine.Raise)) continue;

                        // check if the distance from the player is more than 30

                        // if (deadAlly.Distance(Core.Me) >= 40) continue;
                        if (!ActionManager.CanCastOrQueue(SpellsDefine.Ascend.GetSpellEntity().SpellData, deadAlly)) continue;

                        if (deadAlly.IsDps())
                        {
                            // check if there is tank that's dead too.
                            if (deadAllies.Any(deadTanks => deadTanks.IsTank()))
                            {
                                if (deadAllies.Any(deadHealer => deadHealer.IsHealer()))
                                {
                                    LogHelper.Debug("Trying to swift res the healer.");
                                    await CastAscendToTarget(deadAlly);
                                    return null;
                                }
                                LogHelper.Debug("Trying to swift res the tank.");
                                await CastAscendToTarget(deadAlly);
                                return null;
                            }
                            LogHelper.Debug("Trying to swift res the dps.");
                            await CastAscendToTarget(deadAlly);
                            return null;
                        }

                        if (deadAlly.IsTank())
                        {
                            // check if there is healers that are dead too.
                            if (deadAllies.Any(deadHealer => deadHealer.IsHealer()))
                            {
                                LogHelper.Debug("Trying to swift res the healer.");
                                await CastAscendToTarget(deadAlly);
                                return null;
                            }
                            LogHelper.Debug("Trying to swift res the tank.");
                            await CastAscendToTarget(deadAlly);
                            return null;
                        }

                        if (!deadAlly.IsHealer()) continue;
                        LogHelper.Debug("Trying to swift res the healer.");
                        await CastAscendToTarget(deadAlly);
                        return null;
                    }
                    return null;
                // Tanks>Healer>DPS
                case 1:
                    LogHelper.Debug("Tanks>Healer>DPS-RESSING");
                    foreach (var deadAlly in deadAllies)
                    {
                        if (deadAlly.IsDps())
                        {
                            // check if there is healers that are dead too.
                            if (deadAllies.Any(deadHealer => deadHealer.IsHealer()))
                            {
                                if (deadAllies.Any(deadTanks => deadTanks.IsTank()))
                                {
                                    LogHelper.Debug("Trying to swift res the tank.");
                                    await CastAscendToTarget(deadAlly);
                                    return null;
                                }
                                LogHelper.Debug("Trying to swift res the healer.");
                                await CastAscendToTarget(deadAlly);
                                return null;
                            }
                            LogHelper.Debug("Trying to swift res the dps.");
                            await CastAscendToTarget(deadAlly);
                            return null;
                        }

                        if (deadAlly.IsHealer())
                        {
                            // check if there is healers that are dead too.
                            if (deadAllies.Any(deadTanks => deadTanks.IsTank()))
                            {
                                LogHelper.Debug("Trying to swift res the tank.");
                                await CastAscendToTarget(deadAlly);
                                return null;
                            }
                            LogHelper.Debug("Trying to swift res the healer.");
                            await CastAscendToTarget(deadAlly);
                            return null;
                        }

                        if (!deadAlly.IsTank()) continue;
                        LogHelper.Debug("Trying to swift res the tank.");
                        await CastAscendToTarget(deadAlly);
                        return null;
                    }
                    return null;
                // DPS>Healer>Tanks
                case 2:
                    foreach (var deadAlly in deadAllies)
                    {
                        // check if the player already ressed.
                        LogHelper.Debug("checking if the player already rezzed if so skipping.");
                        if (deadAlly.HasAura(AurasDefine.Raise)) continue;

                        // check if the distance from the player is more than 30
                        if (deadAlly.Distance(Core.Me) >= 40) continue;

                        if (deadAlly.IsTank())
                        {
                            // check if there is dps that's dead too.
                            if (deadAllies.Any(deadHealer => deadHealer.IsHealer()))
                            {
                                if (deadAllies.Any(deadDps => deadDps.IsDps()))
                                {
                                    LogHelper.Debug("Trying to swift res the dps.");
                                    await CastAscendToTarget(deadAlly);
                                    return null;
                                }
                                LogHelper.Debug("Trying to swift res the healer.");
                                await CastAscendToTarget(deadAlly);
                                return null;
                            }
                            LogHelper.Debug("Trying to swift res the tanks.");
                            await CastAscendToTarget(deadAlly);
                            return null;
                        }

                        if (deadAlly.IsHealer())
                        {
                            // check if there is dps that are dead too.
                            if (deadAllies.Any(deadDps => deadDps.IsDps()))
                            {
                                LogHelper.Debug("Trying to swift res the DPS.");
                                await CastAscendToTarget(deadAlly);
                                return null;
                            }
                            LogHelper.Debug("Trying to swift res the healer.");
                            await CastAscendToTarget(deadAlly);
                            return null;
                        }

                        if (!deadAlly.IsDps()) continue;
                        LogHelper.Debug("Trying to swift res the DPS.");
                        await CastAscendToTarget(deadAlly);
                        return null;
                    }
                    return null;
            }
            return null;
        }
        public static async Task CastAscendToTarget(Character target)
        {
            if (!SpellsDefine.Ascend.IsUnlock()) return;
            await CastSwiftCast();
            var spell = new SpellEntity(SpellsDefine.Ascend, target as BattleCharacter);
            await spell.DoGCD();
        }
        public static async Task CastSwiftCast()
        {
            LogHelper.Debug("Checking if we have swiftcast aura or Swiftcast recentlyUsed");
            if (Core.Me.HasAura(AurasDefine.Swiftcast) || SpellsDefine.Swiftcast.RecentlyUsed()) return;
            LogHelper.Debug("Swiftcast can be used: using.");
            var spell = SpellsDefine.Swiftcast.GetSpellEntity();
            var ret = await spell.DoAbility();
        }
        public static async Task<bool> CastEssentialDignity()
        {

            if (GroupHelper.InParty)
            {
                var skillTarget = GroupHelper.CastableAlliesWithin30.Where(r => r.CurrentHealth > 0 && r.CurrentHealthPercent <= 30f).OrderBy(GetCurrentHealthPercent);
                //await CastDivineBenison(skillTarget);
                if (!SpellsDefine.EssentialDignity.IsUnlock()) return false;
                var spell = new SpellEntity(SpellsDefine.EssentialDignity, skillTarget.FirstOrDefault() as BattleCharacter);
                return await spell.DoAbility();
            }
            return false;

        }
        public static async Task<bool> CastCelestialIntersection()
        {

            if (GroupHelper.InParty)
            {
                var skillTarget = GroupHelper.CastableAlliesWithin30.Where(r => r.CurrentHealth > 0 && r.IsTank()).OrderBy(GetCurrentHealthPercent);
                //await CastDivineBenison(skillTarget);
                if (!SpellsDefine.CelestialIntersection.IsUnlock()) return false;
                var spell = new SpellEntity(SpellsDefine.CelestialIntersection, skillTarget.FirstOrDefault() as BattleCharacter);
                return await spell.DoAbility();
            }
            return false;

        }
        public static async Task<bool> CastExaltation()
        {

            if (GroupHelper.InParty)
            {
                var skillTarget = GroupHelper.CastableAlliesWithin30.Where(r => r.CurrentHealth > 0 && r.IsTank()).OrderBy(GetCurrentHealthPercent);
                //await CastDivineBenison(skillTarget);
                if (!SpellsDefine.Exaltation.IsUnlock()) return false;
                var spell = new SpellEntity(SpellsDefine.Exaltation, skillTarget.FirstOrDefault() as BattleCharacter);
                return await spell.DoAbility();
            }
            return false;

        }
        public static async Task<bool> CastSynastry()
        {

            if (GroupHelper.InParty)
            {
                var skillTarget = GroupHelper.CastableAlliesWithin30.Where(r => r.CurrentHealth > 0 && r.IsTank()).OrderBy(GetCurrentHealthPercent);
                //await CastDivineBenison(skillTarget);
                if (!SpellsDefine.Synastry.IsUnlock()) return false;
                var spell = new SpellEntity(SpellsDefine.Synastry, skillTarget.FirstOrDefault() as BattleCharacter);
                return await spell.DoAbility();
            }
            return false;

        }

        public static async Task<bool> CastAspectedBenefic()
        {
            if (GroupHelper.InParty)
            {
                var skillTarget = GroupHelper.CastableAlliesWithin30.Where(r => r.CurrentHealth > 0 && !r.HasAura(AurasDefine.AspectedBenefic)).OrderBy(GetCurrentHealthPercent);
                //await CastDivineBenison(skillTarget);
                if (!SpellsDefine.AspectedBenefic.IsUnlock()) return false;
                var spell = new SpellEntity(SpellsDefine.AspectedBenefic, skillTarget.FirstOrDefault() as BattleCharacter);                
                return await spell.DoGCD();
            }
            return false;
        }
        public static async Task<bool> CastNeutralSect()
        {
            if (!SpellsDefine.NeutralSect.IsUnlock()) return false;
            var spell1 = new SpellEntity(SpellsDefine.NeutralSect, Core.Me as BattleCharacter);
            //var spell2 = new SpellEntity(SpellsDefine.AspectedHelios, Core.Me as BattleCharacter);
            //await spell1.DoAbility();            
            return await spell1.DoGCD();
        }
    }
}
