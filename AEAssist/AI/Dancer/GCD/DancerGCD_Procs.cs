using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Dancer.GCD
{
    public class DancerGCD_Procs : IAIHandler
    {
        public int Check(SpellEntity lastGCD)
        {
            if (!SpellsDefine.ReverseCascade.IsUnlock())
            {
                return -10;
            }
            
            if (SpellsDefine.Flourish.RecentlyUsed())
            {
                return 1;
            }
            
            if (!Core.Me.HasAura(AurasDefine.FlourishingSymmetry) &&
                !Core.Me.HasAura(AurasDefine.FlourshingFlow))
            {
                return -1;
            }

            if (ActionResourceManager.Dancer.FourFoldFeathers == 4)
            {
                return -2;
            }


            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            // var spell = SpellsDefine.StandardStep.GetSpellEntity();
            return await DancerSpellHelper.ProcGCDCombo(Core.Me.CurrentTarget);
        }
    }
}