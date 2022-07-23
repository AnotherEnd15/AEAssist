using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;

namespace AEAssist.AI.Samurai.GCD
{
    public class SamuraiGCD_KaeshiSetsugekka : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            // Only use it during odd or even bursts.
            if (AIRoot.GetBattleData<SamuraiBattleData>().CurrPhase == SamuraiPhase.OddMinutesBurstPhase
                || AIRoot.GetBattleData<SamuraiBattleData>().CurrPhase == SamuraiPhase.EvenMinutesBurstPhase)
            {
                if (AIRoot.GetBattleData<SamuraiBattleData>().CurrCombo == SamuraiComboStages.MidareSetsuGekka
                    && SpellsDefine.TsubameGaeshi.GetSpellEntity().SpellData.Charges >= 1
                   )
                {
                    return 0;
                }
            } 
            
            return -1;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.KaeshiSetsugekka.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoGCD();
            if (ret)
            {
                AIRoot.GetBattleData<SamuraiBattleData>().CurrCombo = SamuraiComboStages.KaeshiSetsugekka;
                return spell;
            }
            return null;
        }
    }
}