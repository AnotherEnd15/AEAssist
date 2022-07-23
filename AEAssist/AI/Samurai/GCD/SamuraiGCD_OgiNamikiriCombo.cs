using AEAssist.AI.Samurai.SpellQueue;
using AEAssist.Define;
using ff14bot;
using ff14bot.Objects;
using System.Threading.Tasks;
using ff14bot.Managers;

namespace AEAssist.AI.Samurai.GCD
{
    public class SamuraiGCD_OgiNamikiriCombo : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            var bd = AIRoot.GetBattleData<SamuraiBattleData>();
            if (bd.CurrPhase == SamuraiPhase.EvenMinutesBurstPhase)
            {
                if (Core.Me.HasAura(AurasDefine.OgiReady) && !MovementManager.IsMoving)
                {
                    return 0;
                }
            }
            
            // incase we have it ready and it's on odd phase for some reason??
            if (Core.Me.HasAura(AurasDefine.OgiReady) && !MovementManager.IsMoving)
            {
                return 0;
            }
            
            return -1;
        }

        public async Task<SpellEntity> Run()
        {
            AISpellQueueMgr.Instance.Apply<SpellQueue_OgiNamikiriCombo>();
            await Task.CompletedTask;
            return null;
        }
        
    }
}