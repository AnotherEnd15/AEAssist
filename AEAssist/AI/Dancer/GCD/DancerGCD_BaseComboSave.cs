using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Dancer.GCD
{
    public class DancerGCD_BaseComboSave : IAIHandler
    {
        public int Check(SpellEntity lastGCD)
        {
            if (ActionManager.ComboTimeLeft > 0 &&
                ActionManager.ComboTimeLeft < 2.5f)
            {
                if (ActionManager.LastSpellId == SpellsDefine.Windmill)
                {
                    if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 3))
                    {
                        return 1;
                    }
                }
                if (ActionManager.LastSpellId == SpellsDefine.Cascade)
                {
                    if (!TargetHelper.CheckNeedUseAOEByMe(5, 5, 3))
                    {
                        return 1;
                    }
                }
            }

            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Fountain.GetSpellEntity();
            if (ActionManager.LastSpellId == SpellsDefine.Windmill)
            {
                spell = SpellsDefine.Bladeshower.GetSpellEntity();
            }
            if (spell == null)
                return null;
            var ret = await spell.DoGCD();
            if (ret)
                return spell;
            return null;
        }
    }
}