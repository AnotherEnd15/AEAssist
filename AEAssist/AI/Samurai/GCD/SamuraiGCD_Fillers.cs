using AEAssist.Define;
using AEAssist.Rotations.Core;
using System.Threading.Tasks;
using AEAssist.Helper;

namespace AEAssist.AI.Samurai.GCD
{
    public class SamuraiGCD_Fillers : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            // const int minimumGcdOneMin = 26;
            // var baseGcdTime = RotationManager.Instance.GetBaseGCDSpell().AdjustedCooldown.TotalMilliseconds;
            // var battleTime = AIRoot.GetBattleData<BattleData>().CurrBattleTimeInMs;
            // var totalGcdTimeMs = baseGcdTime * minimumGcdOneMin;

            if (AIRoot.GetBattleData<SamuraiBattleData>().CurrPhase == SamuraiPhase.Filler)
            {
                LogHelper.Info("we are in fillers now?");
                return 0;
            }
            return -1;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SamuraiSpellHelper.FillerRotations();
            if (spell == null)
                return null;
            var ret = await spell.DoGCD();
            if (ret)
                return spell;
            return null;
            /*return await SamuraiSpellHelper.FillerRotations();*/
        }
    }
}