using System.Linq;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;

namespace AEAssist.AI.Dancer.SpellQueue
{
    public class SpellQueueSlot_DanceStep : IAISpellQueueSlot
    {
        public int Check(int index)
        {
            var bdls = AIRoot.GetBattleData<BattleData>().lastGCDSpell;
            if (bdls == SpellsDefine.DoubleStandardFinish.GetSpellEntity() ||
                bdls == SpellsDefine.QuadrupleTechnicalFinish.GetSpellEntity() ||
                (!Core.Me.HasAura(AurasDefine.StandardStep) && !Core.Me.HasAura(AurasDefine.TechnicalStep))
               )
            {
                return -10;
            }


            if (bdls == SpellsDefine.StandardStep.GetSpellEntity() ||
                bdls == SpellsDefine.TechnicalStep.GetSpellEntity() ||
                Core.Me.HasAura(AurasDefine.StandardStep) ||
                Core.Me.HasAura(AurasDefine.TechnicalStep))
            {
                return 0;
            }

            return -4;
        }

        public void Fill(SpellQueueSlot slot)
        {
            slot.SetBreakCondition(()=>this.Check(0));
            if (ActionResourceManager.Dancer.CurrentStep != ActionResourceManager.Dancer.DanceStep.Finish)
            {
                foreach (var v in ActionResourceManager.Dancer.Steps.SkipWhile(step => step != ActionResourceManager.Dancer.CurrentStep))
                {
                    if(v == ActionResourceManager.Dancer.DanceStep.Finish)
                        continue;
                    var spell = DancerSpellHelper.GetDanceStep(v);
                    LogHelper.Info($"Queue Step: {v} {spell.SpellData.LocalizedName}");
                    slot.EnqueueGCD((spell.Id, SpellTargetType.Self));
                }
            }
        }
    }
}