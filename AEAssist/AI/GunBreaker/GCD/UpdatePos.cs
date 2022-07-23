using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;
using ff14bot;
using System;

namespace AEAssist.AI.GunBreaker.GCD
{
    public class UpdatePos : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            var disc = TargetHelper.GetTargetDistanceFromMeTest(Core.Me, Core.Me.CurrentTarget);
            DataBinding.Instance.Gnbdis = disc;
            DataBinding.Instance.GNBdisg = "目标圈距离：" + disc.ToString("0.00");
            var dis = (int)Math.Round(disc * 100);
            if (Core.Me.HasTarget)
            {
                if (dis<250)
                {
                    MeleePosition.Intance.IsTargetDistanceSafe = System.Windows.Media.Brushes.Green;
                }
                else if(dis>=250 && dis <300)
                {
                    MeleePosition.Intance.IsTargetDistanceSafe = System.Windows.Media.Brushes.Yellow;
                }
                else if(dis>=300 && dis < 340)
                {
                    MeleePosition.Intance.IsTargetDistanceSafe = System.Windows.Media.Brushes.Red;
                }
                else if (dis >= 340)
                    MeleePosition.Intance.IsTargetDistanceSafe = System.Windows.Media.Brushes.DarkRed;
                MeleePosition.Intance.TargetDistance = dis;
            }
            return -1;
        }

        public Task<SpellEntity> Run()
        {
            return null;
        }
    }
}
