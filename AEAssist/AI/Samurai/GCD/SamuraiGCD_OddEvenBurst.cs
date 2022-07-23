using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;

namespace AEAssist.AI.Samurai.GCD
{
    public class SamuraiGCD_OddEvenBurst : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            var bd = AIRoot.GetBattleData<SamuraiBattleData>();
            if (SamuraiSpellHelper.SenCounts() == 3)
            {
                return -1;
            }

            if (AIRoot.GetBattleData<SamuraiBattleData>().higanBanaCount < 1)
            {
                if (SamuraiSpellHelper.SenCounts() == 1)
                {
                    return -1;
                }   
            }

            if (bd.CurrPhase == SamuraiPhase.OddMinutesBurstPhase || bd.CurrPhase == SamuraiPhase.EvenMinutesBurstPhase)
            {
                LogHelper.Info("We are in Burst Window now");
                return 0;
            }
            
            return -1;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SamuraiSpellHelper.Burst();
            if (spell == null)
                return null;
            var ret = await spell.DoGCD();
            if (ret)
            {
                return spell;
            }
            return null;
        }
    }
}