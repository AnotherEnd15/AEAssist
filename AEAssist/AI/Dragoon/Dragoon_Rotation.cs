using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Rotations.Core;
using ff14bot.Enums;
using System.Threading.Tasks;

namespace AEAssist.AI.Dragoon
{
    [Job(ClassJobType.Dragoon)]
    public class Dragoon_Rotation : IRotation
    {
        public void Init()
        {
            //CountDownHandler.Instance.AddListener(1500,
            //    () => PotionHelper.UsePotion(SettingMgr.GetSetting<GeneralSettings>().MindPotionId));


            AEAssist.DataBinding.Instance.EarlyDecisionMode = SettingMgr.GetSetting<DragoonSettings>().EarlyDecisionMode;
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