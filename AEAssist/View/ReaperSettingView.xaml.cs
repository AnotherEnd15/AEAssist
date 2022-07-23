using System.Windows.Controls;
using AEAssist.Opener;
using ff14bot.Enums;

namespace AEAssist.View
{
    public partial class ReaperSettingView : UserControl
    {
        public ReaperSettingView()
        {
            InitializeComponent();
            
            if (OpenerMgr.Instance.JobOpeners.ContainsKey(ClassJobType.Reaper))
            {
                ChooseOpener.ItemsSource = OpenerMgr.Instance.JobOpeners[ClassJobType.Reaper];
                ChooseOpener.SelectedValue = SettingMgr.GetSetting<ReaperSettings>().ReaperOpener;
            }
        }
        
        private void ChooseOpener_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingMgr.GetSetting<ReaperSettings>().ReaperOpener = ChooseOpener.SelectedValue.ToString();
            OpenerMgr.Instance.SpecifyOpenerByName[ClassJobType.Reaper] = ChooseOpener.SelectedValue.ToString();
        }
    }
}