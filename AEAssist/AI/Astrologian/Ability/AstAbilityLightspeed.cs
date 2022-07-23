using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;

namespace AEAssist.AI.Astrologian.Ability
{
    internal class AstAbilityLightspeed : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SettingMgr.GetSetting<AstSettings>().LighSpeedToggle) return -3;
            if (!SpellsDefine.Lightspeed.IsReady()) return -1;
            if (!SettingMgr.GetSetting<AstSettings>().Lightspeed)
            {
                return -3;
            }
            if (AIRoot.Instance.CloseBurst)
                return -2;
            if (SpellsDefine.Divination.GetSpellEntity().Cooldown.TotalSeconds > 3) return -2;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Lightspeed.GetSpellEntity();
            if (spell == null) return null;
            var ret = await spell.DoAbility();
            return ret ? spell : null;
        }
    }
}
