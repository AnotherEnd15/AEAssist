using AEAssist.Define;
using AEAssist.Helper;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Dancer
{
    public class DancerSpellHelper
    {
        // Cascade 瀑泻 ST1 Reverse Cascade 逆瀑泻
        // Fountain 喷泉 ST2 :Fountainfall 坠喷泉 
        // Windmill 风车 AOE1 Rising Windmill 升风车 
        // Bladeshower 落刃雨 AOE2 :Bloodshower 落血雨 
        private static async Task<SpellEntity> UseSingleCombo(GameObject target)
        {
            if (ActionManager.LastSpellId == SpellsDefine.Cascade &&
                SpellsDefine.Fountain.IsUnlock())
            {
                if (await SpellsDefine.Fountain.DoGCD())
                {
                    return SpellsDefine.Fountain.GetSpellEntity();
                }
            }
            if (await SpellsDefine.Cascade.DoGCD())
            {
                return SpellsDefine.Cascade.GetSpellEntity();
            }
            return null;
        }

        private static async Task<SpellEntity> UseAOECombo(GameObject target)
        {
            if (ActionManager.LastSpellId == SpellsDefine.Windmill &&
                SpellsDefine.Bladeshower.IsUnlock())
            {
                if (await SpellsDefine.Bladeshower.DoGCD())
                {
                    return SpellsDefine.Bladeshower.GetSpellEntity();
                }
            }
            if (await SpellsDefine.Windmill.DoGCD())
            {
                return SpellsDefine.Windmill.GetSpellEntity();
            }

            return null;
        }

        public static async Task<SpellEntity> BaseGCDCombo(GameObject target)
        {
            if (SpellsDefine.Windmill.IsUnlock())
            {
                if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 3)) return await UseAOECombo(target);
            }
            return await UseSingleCombo(target);
        }

        private static async Task<SpellEntity> UseProcSingleCombo(GameObject target)
        {
            if (Core.Me.HasAura(AurasDefine.FlourshingFlow) || SpellsDefine.Flourish.RecentlyUsed())
            {
                if (SpellsDefine.Fountainfall.IsUnlock())
                {
                    if (await SpellsDefine.Fountainfall.DoGCD())
                    {
                        return SpellsDefine.Fountainfall.GetSpellEntity();
                    }
                }
            }
            else
            {
                if (await SpellsDefine.ReverseCascade.DoGCD())
                {
                    return SpellsDefine.ReverseCascade.GetSpellEntity();
                }
            }

            return null;
        }

        private static async Task<SpellEntity> UseProcAOECombo(GameObject target)
        {
            if (Core.Me.HasAura(AurasDefine.FlourshingFlow))
            {
                if (SpellsDefine.Bloodshower.IsUnlock())
                {
                    if (await SpellsDefine.Bloodshower.DoGCD())
                    {
                        return SpellsDefine.Bloodshower.GetSpellEntity();
                    }
                }
            }
            else
            {
                if (await SpellsDefine.RisingWindmill.DoGCD())
                {
                    return SpellsDefine.RisingWindmill.GetSpellEntity();
                }
            }

            return null;
        }

        public static async Task<SpellEntity> ProcGCDCombo(GameObject target)
        {
            if (SpellsDefine.RisingWindmill.IsUnlock())
            {
                if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 2)) return await UseProcAOECombo(target);
            }

            return await UseProcSingleCombo(target);
        }

        public static SpellEntity GetDanceStep(ActionResourceManager.Dancer.DanceStep step)
        {
            switch (step)
            {
                case ActionResourceManager.Dancer.DanceStep.Emboite:
                    return SpellsDefine.Emboite.GetSpellEntity();

                case ActionResourceManager.Dancer.DanceStep.Entrechat:
                    return SpellsDefine.Entrechat.GetSpellEntity();

                case ActionResourceManager.Dancer.DanceStep.Jete:
                    return SpellsDefine.Jete.GetSpellEntity();

                case ActionResourceManager.Dancer.DanceStep.Pirouette:
                    return SpellsDefine.Pirouette.GetSpellEntity();
            }

            return null;
        }

        public static async Task PreCombatDanceSteps()
        {
            bool finish = false;
            const int retryInterval = 100; // 25* 100 = GCD CoolDown
            while (!finish)
            {
                try
                {
                    if (ActionResourceManager.Dancer.Steps.Length > 2)
                    {
                        var steps = ActionResourceManager.Dancer.Steps;
                        foreach (var step in steps)
                        {
                            if (step == ActionResourceManager.Dancer.DanceStep.Finish)
                            {
                                finish = true;
                                break;
                            }
                            var spell = DancerSpellHelper.GetDanceStep(step);
                            while (!await spell.DoGCD())
                            {
                                await Coroutine.Sleep(retryInterval);
                            }

                        }
                    }
                }
                catch
                {
                    // ignored
                }
            }

        }

    }
}