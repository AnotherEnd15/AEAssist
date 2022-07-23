using AEAssist.AI.Dragoon.Ability;
using AEAssist.AI.Dragoon.GCD;
using ff14bot.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AEAssist.AI.Dragoon
{
    [Job(ClassJobType.Dragoon)]
    public class Dragoon_AIPriorityQueue : IAIPriorityQueue
    {
        public List<IAIHandler> GCDQueue { get; } = new List<IAIHandler>()
        {
            new DragoonGCD_FangAndClaw(),//龙爪龙牙
            new DragoonGCD_WheelingThrust(),//龙尾大回旋
            new DragoonGCD_Base()//基础连击-樱花连
        };

        public List<IAIHandler> AbilityQueue { get; } = new List<IAIHandler>()
        {
            new DragoonAbility_Base()
        };
        public Task<bool> UsePotion()
        {
            return Task.FromResult(false);
        }
    }
}