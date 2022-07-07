using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;

namespace AEAssist.AI.Samurai.Ability
{
    public class SamuraiAbility_TsubameGaeshi : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.TsubameGaeshi.IsReady())
            {
                return -1;
            }

            if (lastSpell != SpellsDefine.MidareSetsugekka.GetSpellEntity())
            {
                return -1;
            }

            AIRoot.GetBattleData<SamuraiBattleData>().Bursting = true;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.TsubameGaeshi.GetSpellEntity();
            if (spell == null) return null;
            if (await spell.DoAbility())
                return spell;
            return null;
        }
    }
}