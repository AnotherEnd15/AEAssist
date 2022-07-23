using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Astrologian.Ability
{
    internal class AstAbilityHalfPlay : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {

            if (!SpellsDefine.Play.IsReady()) return -1;
            if (AEAssist.DataBinding.Instance.FinalBurst)
                return 0;
            if (AIRoot.Instance.CloseBurst)
                return -2;
            if (ActionResourceManager.CostTypesStruct.offset_C == 0 || ActionResourceManager.CostTypesStruct.offset_C == 112 || ActionResourceManager.CostTypesStruct.offset_C == 128) return -2;
            
            if (SpellsDefine.Divination.GetSpellEntity().Cooldown.TotalSeconds > 55 && SpellsDefine.Divination.GetSpellEntity().Cooldown.TotalSeconds < 65)
            {
                //if (SettingMgr.GetSetting<AstSettings>().AstHalfCard)
                //{
                    //SettingMgr.GetSetting<AstSettings>().AstHalfCard = false;
                    //return 0;
                //}
                if (SpellsDefine.Draw.IsMaxChargeReady(1))
                {
                    return 0;
                }

            }
            
            //if (SpellsDefine.Draw.IsMaxChargeReady(0.1f))
            //{
                //LogHelper.Debug("即将溢出，发卡");
                //return 0;
            //}
            return -3;

        }

        public async Task<SpellEntity> Run()
        {
            if (Core.Me.HasAura(AurasDefine.BalanceDrawn))
            {
                LogHelper.Debug("发近战太阳神");
                var spell = SpellsDefine.Play.GetSpellEntity();
                var ret = await AstSpellHelper.CastMeleeCardHalf();
                return ret ? spell : null;
                //return AstSpellHelper.CastMeleeCard();
            }
            if (Core.Me.HasAura(AurasDefine.BoleDrawn))
            {
                LogHelper.Debug("发远程世界树");
                var spell = SpellsDefine.Play.GetSpellEntity();
                var ret = await AstSpellHelper.CastRangedCardHalf();
                return ret ? spell : null;
                //return AstSpellHelper.CastRangedCard();
            }
            if (Core.Me.HasAura(AurasDefine.ArrowDrawn))
            {
                LogHelper.Debug("发近战放浪神");
                var spell = SpellsDefine.Play.GetSpellEntity();
                var ret = await AstSpellHelper.CastMeleeCardHalf();
                return ret ? spell : null;
                //return AstSpellHelper.CastMeleeCard();
            }
            if (Core.Me.HasAura(AurasDefine.SpearDrawn))
            {
                LogHelper.Debug("发近战战争神");
                var spell = SpellsDefine.Play.GetSpellEntity();
                var ret = await AstSpellHelper.CastMeleeCardHalf();
                return ret ? spell : null;
                //return AstSpellHelper.CastMeleeCard();
            }
            if (Core.Me.HasAura(AurasDefine.EwerDrawn))
            {
                LogHelper.Debug("发远程河流神");
                var spell = SpellsDefine.Play.GetSpellEntity();
                var ret = await AstSpellHelper.CastRangedCardHalf();
                return ret ? spell : null;
                //return AstSpellHelper.CastRangedCard();
            }
            if (Core.Me.HasAura(AurasDefine.SpireDrawn))
            {
                LogHelper.Debug("发远程建筑神");
                var spell = SpellsDefine.Play.GetSpellEntity();
                var ret = await AstSpellHelper.CastRangedCardHalf();
                return ret ? spell : null;
                //return AstSpellHelper.CastRangedCard();
            }

            return null;
        }
    }
}
