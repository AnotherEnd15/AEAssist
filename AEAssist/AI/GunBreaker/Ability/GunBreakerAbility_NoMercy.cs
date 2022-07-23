using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.GunBreaker.Ability
{
    public class GunBreakerAbility_NoMercy : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!DataBinding.Instance.Burst)
                return -1;
            if (!SpellsDefine.NoMercy.GetSpellEntity().SpellData.IsReady())
                return -2;
            //if (Core.Me.ClassLevel == 90 && AIRoot.GetBattleData<GunBreakerBattleData>().A_State)
            //    return -5;
            var time = SettingMgr.GetSetting<GeneralSettings>().RegionOfAbility;
            if (TimeHelper.Now() - AIRoot.GetBattleData<BattleData>().lastCastTime < time - 100)
                return -3;

            if (ActionResourceManager.Gunbreaker.SecondaryComboStage > 0)
                return 1;

            if (ActionResourceManager.Gunbreaker.Cartridge != 3)
                if (Core.Me.ClassLevel < 88)
                    return 2;
                else if (SpellsDefine.Bloodfest.GetSpellEntity().SpellData.IsReady())
                    return 3;
                else return -4;

            return 0;
        }
        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.NoMercy.GetSpellEntity();
            if (spell == null) return null;
            if (await spell.DoAbility())
                return spell;
            return null;
        }
    }
}
