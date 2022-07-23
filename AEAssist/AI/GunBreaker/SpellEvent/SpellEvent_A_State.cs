using AEAssist.Define;
using AEAssist.Helper;

namespace AEAssist.AI.GunBreaker.SpellEvent
{
    [SpellEvent(SpellsDefine.SolidBarrel)]
    public class SpellEvent_A_State : ISpellEvent
    {
        public void Run(uint spellId)
        {
            if (SpellsDefine.NoMercy.GetSpellEntity().SpellData.Cooldown.TotalMilliseconds < 3000)
                AIRoot.GetBattleData<GunBreakerBattleData>().A_State = true;
        }
    }
}