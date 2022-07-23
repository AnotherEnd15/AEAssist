using System.Globalization;
using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;

namespace AEAssist.AI.Samurai.GCD
{
    public class SamuraiGCD_CoolDownPhase : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (AIRoot.GetBattleData<SamuraiBattleData>().CurrPhase != SamuraiPhase.CooldownPhase)
            {
                LogHelper.Info("Not in CooldownPhase");
                return -1;
            }

            if (SamuraiSpellHelper.SenCounts() == 3)
            {
                return -1;
            }
            
            LogHelper.Info("We are in COOLDOWn NOW");
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            // CoolDownPhase
            AIRoot.GetBattleData<SamuraiBattleData>().burstingShintenCount = 0;
            AIRoot.GetBattleData<SamuraiBattleData>().burstingMeikyoShisuiCount = 0;
            var spell = SamuraiSpellHelper.CoolDownPhaseGCD(Core.Me.CurrentTarget);
            if (spell == null)
                return null;
            var ret = await spell.DoGCD();
            if (ret)
            {
                AIRoot.GetBattleData<SamuraiBattleData>().higanBanaCount = 0;
                return spell;
            }
            return null;
        }
    }
}