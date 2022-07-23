using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Threading.Tasks;


namespace AEAssist.AI.Astrologian.Ability
{
    internal class AstAbilityRedraw : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!Core.Me.HasAura(AurasDefine.ClarifyingDraw))
            {
                return -1;
            }
            //if (ActionResourceManager.Astrologian.Arcana == ActionResourceManager.Astrologian.AstrologianCard.Balance || ActionResourceManager.Astrologian.Arcana == ActionResourceManager.Astrologian.AstrologianCard.Arrow || ActionResourceManager.Astrologian.Arcana == ActionResourceManager.Astrologian.AstrologianCard.Spear)
            //{
                //LogHelper.Debug("近战卡不抽");
                //return -2;
            //}
            if (ActionResourceManager.CostTypesStruct.offset_C == 1 || ActionResourceManager.CostTypesStruct.offset_C == 3 || ActionResourceManager.CostTypesStruct.offset_C == 4)
            {
                if (ActionResourceManager.CostTypesStruct.offset_C == 1)
                {
                    if (ActionResourceManager.CostTypesStruct.offset_D != 1 || ActionResourceManager.CostTypesStruct.offset_E != 1)
                    {
                        return -2;
                    }
                }

                if (ActionResourceManager.CostTypesStruct.offset_C == 3)
                {
                    if (ActionResourceManager.CostTypesStruct.offset_D != 2 || ActionResourceManager.CostTypesStruct.offset_E != 2)
                    {
                        return -2;
                    }
                }

                if (ActionResourceManager.CostTypesStruct.offset_C == 4)
                {
                    if (ActionResourceManager.CostTypesStruct.offset_D != 3 || ActionResourceManager.CostTypesStruct.offset_E != 3)
                    {
                        return -2;
                    }
                }
            }
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Redraw.GetSpellEntity();
            if (spell == null) return null;
            var ret = await spell.DoAbility();
            return ret ? spell : null;
        }
    }
}
