using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Dancer.GCD
{
    public class DancerGCD_SaberDance : IAIHandler
    {
        public int Check(SpellEntity lastGCD)
        {
            if (!SpellsDefine.SaberDance.IsUnlock())
            {
                return -10;
            }
            if (!AEAssist.DataBinding.Instance.UseSaberDance)
            {
                return -3;
            }
            
            if (Core.Me.HasAura(AurasDefine.StandardStep) ||
                Core.Me.HasAura(AurasDefine.TechnicalStep))
            {
                return -2;
            }
            if (ActionResourceManager.Dancer.Esprit < 50)
            {
                return -1;
            }
            if (AEAssist.DataBinding.Instance.FinalBurst) return 2;

            if (ActionResourceManager.Dancer.Esprit >= 85)
            {
                return 1;
            }

            if (AEAssist.DataBinding.Instance.UseFlourish)
            {
                if (SpellsDefine.Flourish.AbilityCoolDownInNextXGCDsWindow(1) && (!Core.Me.HasMyAura(AurasDefine.FlourshingFlow) &&
                                                                !Core.Me.HasMyAura(AurasDefine.FlourishingSymmetry)))
                {
                    return 1;
                }
                // if (SpellsDefine.Flourish.AbilityCoolDownInNextXGCDsWindow(2) && (!Core.Me.HasMyAura(AurasDefine.FlourshingFlow) &&
                //         !Core.Me.HasMyAura(AurasDefine.FlourishingSymmetry)))
                // {
                //     if (ActionResourceManager.Dancer.Esprit < 95)
                //     {
                //         return -5;
                //     }
                // }
            }
            
            if (!AIRoot.Instance.CloseBurst)
            {
                if (SpellsDefine.TechnicalStep.CoolDownInGCDs(1) && !SpellsDefine.TechnicalStep.IsReady())
                {
                    return 2;
                }
            }

            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.SaberDance.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoGCD();
            if (ret)
                return spell;
            return null;
        }
    }
}