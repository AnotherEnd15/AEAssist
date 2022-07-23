using System.Windows.Controls;
using AEAssist.Opener;
using ff14bot.Enums;

namespace AEAssist.View
{
    /// <summary>
    /// GNBSettingView.xaml 的交互逻辑
    /// </summary>
    public partial class GunBreakerSettingView : UserControl
    {
        public GunBreakerSettingView()
        {
            InitializeComponent();
            if (OpenerMgr.Instance.JobOpeners.ContainsKey(ClassJobType.Gunbreaker))
            {
                ChooseOpener.ItemsSource = OpenerMgr.Instance.JobOpeners[ClassJobType.Gunbreaker];
                ChooseOpener.SelectedValue = SettingMgr.GetSetting<GunBreakerSettings>().GunBreakerOpener;
                OpenerMgr.Instance.SpecifyOpenerByName[ClassJobType.Gunbreaker] = ChooseOpener.SelectedValue.ToString();
            }
        }

        private void ChooseOpener_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OpenerMgr.Instance.SpecifyOpenerByName[ClassJobType.Gunbreaker] = ChooseOpener.SelectedValue.ToString();
            SettingMgr.GetSetting<GunBreakerSettings>().GunBreakerOpener = ChooseOpener.SelectedValue.ToString();
        }
    }
}
