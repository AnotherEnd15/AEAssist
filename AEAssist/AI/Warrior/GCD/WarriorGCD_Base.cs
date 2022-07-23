using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist.AI.Warrior.GCD
{
    public class WarriorGCD_Base : IAIHandler
    {
        uint spell;
        static public uint GetSpell()
        {
            var aoeChecker = TargetHelper.CheckNeedUseAOE(5, 5, ConstValue.WhiteMageAOECount);       
            
            if (aoeChecker && SpellsDefine.Overpower.IsUnlock())//判断是否需要AOE 并且 AOE技能(超压斧)是否已学习
                return GetAOE();

            if (ActionResourceManager.Warrior.BeastGauge >= 50 || Core.Me.HasMyAuraWithTimeleft(AurasDefine.InnerRelease, 1000))
            //if (ActionResourceManager.Warrior.BeastGauge >= 50)
            //兽魂量普是否大于等于50 或者 原初的解放BUFF大于1秒
            return FellCleave();//裂石飞环
            else
                return GetSingleTarget();
        }
        public static uint FellCleave()//裂石飞环
        {
            if (SpellsDefine.FellCleave.IsUnlock())//裂石飞环是否已学习
                return SpellsDefine.FellCleave;
            else
                if (SpellsDefine.InnerBeast.IsUnlock())//原初之魂是否已学习
                    return SpellsDefine.InnerBeast;
                else
                    return SpellsDefine.HeavySwing;
        }
        public int Check(SpellEntity lastSpell)
        {
            spell = GetSpell();
            //解放CD快好了就打3连怎么写，技能CD……
            //LogHelper.Debug("look this：" + spell.ToString());
            if (!spell.IsReady())
                return -1;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoGCD()) return spell.GetSpellEntity();

            return null;
        }



        public static uint GetSingleTarget()
        {   
            var tar = Core.Me as Character;
            //LogHelper.Debug(tar.HasMyAuraWithTimeleft(AurasDefine.SurgingTempest, 10000).ToString());
            switch (ActionManager.LastSpellId) 
            {
                case SpellsDefine.HeavySwing:
                    if (SpellsDefine.Maim.IsUnlock())
                        return SpellsDefine.Maim;//凶残裂
                    else return SpellsDefine.HeavySwing;//重劈
                case SpellsDefine.Maim:
                    if (SpellsDefine.StormsEye.IsUnlock())
                        if (tar.HasMyAuraWithTimeleft(AurasDefine.SurgingTempest, 30000))//战场风暴持续时间是否大于30秒
                            return SpellsDefine.StormsPath;//绿斩 暴风斩
                        else return SpellsDefine.StormsEye;//红斩 暴风碎
                    else
                        return SpellsDefine.HeavySwing;

                default:
                    return SpellsDefine.HeavySwing;
            }
            
        }

        public static uint GetRoyalAuthority()
        {
            if (SpellsDefine.RoyalAuthority.IsUnlock())
                return SpellsDefine.RoyalAuthority;
            return SpellsDefine.RageofHalone;
        }
        public static uint GetAOE()
        {
            if (ActionResourceManager.Warrior.BeastGauge >= 50)
                if (SpellsDefine.Decimate.IsUnlock())
                    return SpellsDefine.Decimate;//地毁人亡
                else
                    return SpellsDefine.SteelCyclone;//钢铁旋风

            if (ActionManager.LastSpellId == SpellsDefine.Overpower && SpellsDefine.MythrilTempest.IsUnlock())
                return SpellsDefine.MythrilTempest;//秘银风暴

            return SpellsDefine.Overpower;//超压斧

        }

    }
}