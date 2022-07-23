using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;

namespace AEAssist.AI.Samurai.Ability
{
    public class SamuraiAbility_KaeshiNamikiri : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.KaeshiNamikiri.IsReady()) return -1;
            var bd = AIRoot.GetBattleData<SamuraiBattleData>();
            if (bd.CurrPhase == SamuraiPhase.EvenMinutesBurstPhase)
            {
                if (AIRoot.GetBattleData<SamuraiBattleData>().CurrCombo == SamuraiComboStages.OgiNamiKiri)
                {
                    return 0;
                }
            }

            return -1;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.KaeshiNamikiri.GetSpellEntity();
            if (spell == null) return null;
            if (await spell.DoAbility())
            {
                AIRoot.GetBattleData<SamuraiBattleData>().CurrCombo = SamuraiComboStages.KaeshiNamikiri;
                return spell;
            }
            return null;
        }
    }
}