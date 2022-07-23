using AEAssist.AI;
using AEAssist.Helper;
using AEAssist.TriggerCond;

namespace AEAssist.TriggerSystem.TriggerCond
{
    public class TriggerCondHandler_EnemyHpPct : ATriggerCondHandler<TriggerCond_EnemyHPPct>
    {
        protected override bool Check(TriggerCond_EnemyHPPct cond)
        {
            if (AIRoot.GetBattleData<BattleData>().GetCondHitTime(cond, out var time))
            {
                if (TimeHelper.Now() >= time + cond.delayTime * 1000) return true;
            }
            else
            {
                var enemys = TargetMgr.Instance.Units;
                foreach (var v in enemys.Values)
                {
                    if (!v.Name.Contains(cond.Name) && v.NpcId.ToString() != cond.Name)
                        continue;
                    if (v.CurrentHealthPercent * 100 <= cond.HpPct)
                    {
                        AIRoot.GetBattleData<BattleData>().RecordCondHitTime(cond);
                        return false;
                    }
                }
            }

            return false;
        }
    }
}
