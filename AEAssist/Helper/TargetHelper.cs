﻿using AEAssist.AI;
using AEAssist.Define;
using ff14bot;
using ff14bot.Objects;

namespace AEAssist.Helper
{
    public static class TargetHelper
    {
        public static bool ValidAttackUnit(this GameObject unit)
        {
            return unit != null && unit.IsValid && unit.IsTargetable && unit.CanAttack && unit.CurrentHealth > 0;
        }

        public static bool NotInvulnerable(this GameObject unit)
        {
            return unit != null && !unit.HasAnyAura(AurasDefine.Invincibility);
        }

        public static bool CanAttackTargetInRange(this GameObject unit, GameObject target, int range = 3)
        {
            if (!target.ValidAttackUnit())
                return false;

            var combatReach = target.CombatReach + unit.CombatReach;

            if (unit.Distance(target) < range + combatReach - 0.1f)
                return true;

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="targetRange"> 目标和自己的距离</param>
        /// <param name="damageRange"> 目标和他周围单位的距离</param>
        /// <returns></returns>
        public static bool CheckNeedUseAOE(int targetRange, int damageRange, int needCount = 3)
        {
            if (!AEAssist.DataBinding.Instance.UseAOE)
                return false;
            var count = GetNearbyEnemyCount(Core.Me.CurrentTarget, targetRange, damageRange);

            if (count >= needCount)
                return true;
            return false;
        }

        public static int GetNearbyEnemyCount(GameObject target, int targetRange, int damageRange)
        {
            if (target.Distance(Core.Me) >= targetRange)
                return 0;
            var list = TargetMgr.Instance.EnemysIn25;
            var count = 0;
            foreach (var v in list)
                if (v.Value.Distance(target) <= damageRange)
                    count++;

            return count;
        }

        public static bool CheckNeedUseAOE(GameObject target, int targetRange, int damageRange, int needCount = 3)
        {
            if (!AEAssist.DataBinding.Instance.UseAOE)
                return false;
            var count = GetNearbyEnemyCount(target, targetRange, damageRange);

            if (count >= needCount)
                return true;
            return false;
        }
    }
}