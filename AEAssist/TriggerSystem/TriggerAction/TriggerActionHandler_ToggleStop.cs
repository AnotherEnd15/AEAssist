using AEAssist.TriggerAction;

namespace AEAssist.TriggerSystem.TriggerAction
{
    public class TriggerActionHandler_ToggleStop : ATriggerActionHandler<TriggerAction_ToggleStop>
    {
        protected override void Handle(TriggerAction_ToggleStop t)
        {
            AEAssist.DataBinding.Instance.Stop = t.value;
        }
    }
}