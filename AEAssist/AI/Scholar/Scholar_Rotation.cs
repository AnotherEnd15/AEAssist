using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Rotations.Core;
using ff14bot.Enums;

namespace AEAssist.AI.Scholar
{
    [Job(ClassJobType.Scholar)]
    public class Scholar_Rotation : IRotation
    {
        public void Init()
        {
            CountDownHandler.Instance.AddListener(2500, () =>
            PotionHelper.UsePotion(SettingMgr.GetSetting<GeneralSettings>().MindPotionId));
            CountDownHandler.Instance.AddListener(1500, () => SpellsDefine.SchRuin.DoGCD());            
            AEAssist.DataBinding.Instance.EarlyDecisionMode = SettingMgr.GetSetting<ScholarSettings>().EarlyDecisionMode;
        }
        public Task<bool> PreCombatBuff()
        {
            return Task.FromResult(false);
        }

        public Task<bool> NoTarget()
        {

            return Task.FromResult(false);
        }

        public SpellEntity GetBaseGCDSpell()
        {
            return SpellsDefine.FastBlade.GetSpellEntity();
        }
    }
}