using System.Collections.Generic;
using System.Threading.Tasks;
using AEAssist.AI.Scholar.Ability;
using AEAssist.AI.Scholar.GCD;
using ff14bot.Enums;
namespace AEAssist.AI.Scholar
{
    [Job(ClassJobType.Scholar)]
    public class Scholar_AIPriorityQueue : IAIPriorityQueue
    {
        public List<IAIHandler> GCDQueue { get; } = new List<IAIHandler>()
        {
            //new ScholarGCD_BldeofXX(),
            //new ScholarGCD_HolySpirit(),
            //new ScholarGCD_Ranged(),
            new ScholarGCD_SummonEos(),
            new ScholarGCD_Dot(),
            new ScholarGCD_Base()
        };

        public List<IAIHandler> AbilityQueue { get; } = new List<IAIHandler>()
        {
            new ScholarAbility_ChainStratagem(),//连环计
            new ScholarAbility_Dissipation(),//转化
            new ScholarAbility_Aetherflow(),//以太超流            
            new ScholarAbility_EnergyDrain2(),//能量吸收
            new ScholarAbility_Protraction(),
            new ScholarAbility_LucidDreaming()//醒梦
        };
        public Task<bool> UsePotion()
        {
            return Task.FromResult(false);
        }
    }
}