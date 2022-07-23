using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.Dancer.Ability
{
    public class DancerAbility_FanDance4 : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.FanDanceIV.IsUnlock())
            {
                return -10;
            }
            if (!Core.Me.HasAura(AurasDefine.FourfoldFanDance) && !SpellsDefine.Flourish.RecentlyUsed())
            {
                return -1;
            }
            if (AEAssist.DataBinding.Instance.FinalBurst) return 2;

            if (Core.Me.HasAura(AurasDefine.Devilment))
            {
                return 1;
            }
            if (!Core.Me.HasAura(AurasDefine.Devilment))
            {
                double doubletime = SpellsDefine.Devilment.GetSpellEntity().SpellData.Cooldown.TotalMilliseconds + AIRoot.Instance.GetGCDDuration() * 2;
                int time = (int)doubletime;
                if (Core.Me.HasMyAuraWithTimeleft(AurasDefine.FourfoldFanDance, time) || SpellsDefine.Flourish.RecentlyUsed())
                {
                    return -2;
                }
            }

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.FanDanceIV.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoAbility();
            if (ret)
                return spell;
            return null;
        }
    }
}