using AEAssist.Define;
using AEAssist.Helper;
using System.Linq;
using System.Threading.Tasks;

namespace AEAssist.AI.Astrologian.Ability
{
    internal class AstAbilityEssentialDignity : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SettingMgr.GetSetting<AstSettings>().Heal)
            {
                return -5;
            }
            
            if (!SpellsDefine.EssentialDignity.IsReady()) return -1;
            var skillTarget = GroupHelper.CastableAlliesWithin30.FirstOrDefault(r => r.CurrentHealth > 0 && r.CurrentHealthPercent <= 30f);
            if (skillTarget == null)
            {
                return -2;
            }
            if (!SpellsDefine.EssentialDignity.IsMaxChargeReady())
            {
                return -1;
            }
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.EssentialDignity.GetSpellEntity();
            var ret = await AstSpellHelper.CastEssentialDignity();
            return ret ? spell : null;            
        }
    }
}
