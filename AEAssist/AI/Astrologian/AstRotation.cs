using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Rotations.Core;
using ff14bot.Enums;
using System.Threading.Tasks;

namespace AEAssist.AI.Astrologian
{
    [Job(ClassJobType.Astrologian)]
    public class AstRotation : IRotation
    {
        public void Init()
        {
            CountDownHandler.Instance.AddListener(15000, AstSpellHelper.CastNeutralSect);
            CountDownHandler.Instance.AddListener(2500, () =>
            PotionHelper.UsePotion(SettingMgr.GetSetting<GeneralSettings>().MindPotionId));
            CountDownHandler.Instance.AddListener(1500, () => AstSpellHelper.GetBaseGcd().DoGCD());
            AEAssist.DataBinding.Instance.EarlyDecisionMode = SettingMgr.GetSetting<AstSettings>().EarlyDecisionMode;
            LogHelper.Info("EarlyDecisionMode: " + AEAssist.DataBinding.Instance.EarlyDecisionMode);
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
            return AstSpellHelper.GetBaseGcd();
        }
    }
}
