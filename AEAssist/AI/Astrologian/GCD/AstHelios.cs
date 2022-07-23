using AEAssist.Define;
using AEAssist.Helper;
using System.Linq;
using System.Threading.Tasks;
using ff14bot;
using ff14bot.Managers;

namespace AEAssist.AI.Astrologian.GCD
{
    public class AstHelios : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            
            if (!SettingMgr.GetSetting<AstSettings>().GcdHeal)
            {
                return -5;
            }

            if (MovementManager.IsMoving)
            {
                if (!Core.Me.HasAura(AurasDefine.Lightspeed))
                {
                    return -5;
                }
                
            }
            if (!SpellsDefine.Helios.IsUnlock()) return -1;
            var skillTarget = GroupHelper.CastableAlliesWithin15.Count(r => r.CurrentHealth > 0 && r.CurrentHealthPercent <= 75f);
            if (skillTarget > 2)
            {
                return 0;
            }
            return -10;

        }
        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Helios.GetSpellEntity();
            if (spell == null) return null;
            var ret = await spell.DoGCD();
            return ret ? spell : null;
        }
    }
}
