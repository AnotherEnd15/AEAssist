using System;
using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist.AI.Dragoon.Ability
{
    public class DragoonAbility_Base : IAIHandler
    {
        uint spell;
        static public uint GetSpell()
        {
            var target = Core.Me.CurrentTarget as Character;
            //巨龙之眼还没想好怎么做
            //
            if (SpellsDefine.LanceCharge.IsReady()) return SpellsDefine.LanceCharge;//猛枪 好了就用
            if (SpellsDefine.BattleLitany.IsReady()) return SpellsDefine.BattleLitany;//战斗连祷 好了就用
            // LogHelper.Info($"检测: {DataManager.GetSpellData(SpellsDefine.SpineshatterDive).Cooldown.Milliseconds.ToString()}");

            if (!SpellsDefine.VorpalThrust.RecentlyUsed() && ActionManager.LastSpellId == SpellsDefine.VorpalThrust && !Core.Me.HasMyAura(AurasDefine.LifeSurge) && SpellsDefine.LifeSurge.IsReady()) return SpellsDefine.LifeSurge;//打完贯通刺就用龙剑

            if (!(ActionResourceManager.CostTypesStruct.offset_A == 2) && SpellsDefine.Geirskogul.IsReady()) return SpellsDefine.Geirskogul;//武神枪 有2档龙眼进入红血
            if (Core.Me.ClassLevel < 74)
            {
                if (!SpellsDefine.Geirskogul.IsReady() && SpellsDefine.Jump.IsReady()) return SpellsDefine.Jump;//跳跃-高跳 武神枪CD中就用
            }
            else
            {
                if (!SpellsDefine.Geirskogul.IsReady() && SpellsDefine.HighJump.IsReady()) return SpellsDefine.HighJump;//跳跃-高跳 武神枪CD中就用
            }

            if (Core.Me.ClassLevel < 84)
            {
                if (DataManager.GetSpellData(SpellsDefine.SpineshatterDive).Cooldown.Milliseconds <= 0)
                    return SpellsDefine.SpineshatterDive;//破碎冲 好了就用
            }
            else
            {
                if (SpellsDefine.SpineshatterDive.IsReady())
                    return SpellsDefine.SpineshatterDive;
            }

            if (SpellsDefine.DragonfireDive.IsReady()) return SpellsDefine.DragonfireDive;//龙炎冲 好了就用

            if (ActionResourceManager.CostTypesStruct.offset_A == 2 && SpellsDefine.Nastrond.IsReady()) return SpellsDefine.Nastrond;//死者之岸 有红血BUFF就放

            if (Core.Me.HasMyAura(AurasDefine.DiveReady) && SpellsDefine.MirageDive.IsReady()) return SpellsDefine.MirageDive;//幻象冲 有预备buff就用

            if (ActionResourceManager.CostTypesStruct.offset_A == 2 && SpellsDefine.Stardiver.IsReady()) return SpellsDefine.Stardiver;//坠星冲 好了就用 有红血BUFF就放

            if (ActionResourceManager.CostTypesStruct.offset_C == 2 && SpellsDefine.WyrmwindThrust.IsReady()) return SpellsDefine.WyrmwindThrust;//天龙点睛 有BUFF就放

            return 0;
        }
        public int Check(SpellEntity lastSpell)
        {          
            spell = GetSpell();
            LogHelper.Info($"将要释放的技能为: {spell.ToString()}");
            if (spell == SpellsDefine.LifeSurge)
                if (!SettingMgr.GetSetting<DragoonSettings>().LifeSurge) return -1;
            if (spell == SpellsDefine.LanceCharge)
                if (!SettingMgr.GetSetting<DragoonSettings>().LanceCharge) return -1;
            if (spell == SpellsDefine.SpineshatterDive)
                if (!SettingMgr.GetSetting<DragoonSettings>().SpineshatterDive) return -1;
            if (spell == SpellsDefine.DragonfireDive)
                if (!SettingMgr.GetSetting<DragoonSettings>().DragonfireDive) return -1;
            if (spell == SpellsDefine.BattleLitany)
                if (!SettingMgr.GetSetting<DragoonSettings>().BattleLitany) return -1;
            if (spell == SpellsDefine.Jump)
                if (!SettingMgr.GetSetting<DragoonSettings>().Jump) return -1;
            if (spell == SpellsDefine.HighJump)
                if (!SettingMgr.GetSetting<DragoonSettings>().Jump) return -1;
            if (spell == SpellsDefine.WyrmwindThrust)
                if (!SettingMgr.GetSetting<DragoonSettings>().WyrmwindThrust) return -1;
            if (spell == SpellsDefine.Geirskogul)
                if (!SettingMgr.GetSetting<DragoonSettings>().Geirskogul) return -1;

            if (spell == 0) return -5;
            if (!spell.IsReady())
                return -1;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoAbility()) return spell.GetSpellEntity();

            return null;
        }
    }

}