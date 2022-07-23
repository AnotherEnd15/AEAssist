using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;

namespace AEAssist.AI.Astrologian.Ability
{
    internal class AstAbilityDivination : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            //if (!SettingMgr.GetSetting<AstSettings>().DivinationToggle) return -3;
            if (!SpellsDefine.Divination.IsReady()) return -1;
            if (AIRoot.GetBattleData<BattleData>().CurrBattleTimeInMs < 4000)
            {
                return -7;
            }           

            if (!SettingMgr.GetSetting<AstSettings>().Divination)
            {
                return -3;
            }
            if (AIRoot.Instance.CloseBurst)
                return -2;
            return 0;

        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Divination.GetSpellEntity();
            if (spell == null) return null;
            //AIRoot.GetBattleData<AstBattleData>().half = true;
            //SettingMgr.GetSetting<AstSettings>().AstHalfCard = true;
            var ret = await spell.DoAbility();
            return ret ? spell : null;
        }
    }
}
