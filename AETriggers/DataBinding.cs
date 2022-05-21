using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using AEAssist;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using PropertyChanged;

namespace AETriggers
{
    [AddINotifyPropertyChangedInterface]
    public class DataBinding
    {
        public static DataBinding Instance = new DataBinding();

        
        public class Trigger
        {

            public Trigger(string typeName)
            {
                this.TypeName = typeName;
                ParamTooltip = ParamToolTip();
                NeedParam = IsNeedParam();
            }

            public string TypeName { get; set; }
            public string Param { get; set; }
            public string ParamTooltip { get; set; }

            public Visibility NeedParam { get; set; }

            public Visibility IsNeedParam()
            {
                var t = TriggerMgr.Instance.Name2Type[TypeName];
                var attr = TriggerMgr.Instance.AllAttrs[t];
                return attr.NeedParams? Visibility.Visible: Visibility.Hidden;
            }
            

            public string ParamToolTip()
            {
                var t = TriggerMgr.Instance.Name2Type[TypeName];
                var attr = TriggerMgr.Instance.AllAttrs[t];
                var toolTip = attr.ParamTooltip;
                if (attr.Example != null)
                {
                    toolTip += "\nExamples:\n\t" + attr.Example;
                }

                return toolTip;
            }
        }

        public class GroupData
        {
            public ObservableCollection<Trigger> CondTriggers { get; set; } = new ObservableCollection<Trigger>();
            public ObservableCollection<Trigger> ActionTriggers { get; set; } = new ObservableCollection<Trigger>();

            public void AddTrigger(string type)
            {
                var t = TriggerMgr.Instance.Name2Type[type];
                if (TriggerMgr.Instance.AllCondType.Contains(t))
                {
                    CondTriggers.Add(new Trigger(type));
                }
                else
                {
                    ActionTriggers.Add(new Trigger(type));
                }
            }
        }

        

        public void Reset()
        {
            Author = string.Empty;
            Name = string.Empty;
            GroupIds = new ObservableCollection<string>();
            AllGroupData = new Dictionary<string, GroupData>();
            CurrChoosedId = string.Empty;
        }


        public TriggerLine Export()
        {
            try
            {
                var triggleLine = new TriggerLine
                {
                    Author = Author,
                    ConfigVersion = TriggerLine.CurrConfigVersion,
                    CurrZoneId = 0,
                    Name = Name,
                    SubZoneId = 0,
                    TargetJob = TargetJob,
                };
                foreach (var v in AllGroupData)
                {
                    var trigger = new AEAssist.Trigger
                    {
                        Id = v.Key
                    };
                    foreach (var cond in v.Value.CondTriggers)
                    {
                        trigger.TriggerConds.Add( ConvertToData(cond) as ITriggerCond);
                    }
                
                    foreach (var action in v.Value.ActionTriggers)
                    {
                        trigger.TriggerActions.Add( ConvertToData(action) as ITriggerAction);
                    }
                
                    triggleLine.Triggers.Add(trigger);
                }

                return triggleLine;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
        }


        ITriggerBase ConvertToData(Trigger trigger)
        {

            var typeName = trigger.TypeName;
            var values = trigger.Param.Split(',');

            var type = TriggerMgr.Instance.Name2Type[typeName];
            var instance = Activator.CreateInstance(type) as ITriggerBase;
            instance.WriteFromJson(values);
            return instance as ITriggerBase;
        }


        public void Load(TriggerLine triggerLine)
        {
            this.Reset();
            
            this.Author = triggerLine.Author;
            this.Name = triggerLine.Name;
            this.TargetJob = triggerLine.TargetJob;

            foreach (var v in triggerLine.Triggers)
            {
                this.GroupIds.Add(v.Id);
                this.AllGroupData[v.Id] = new GroupData();
                var groupData = this.AllGroupData[v.Id];
                
                foreach (var cond in v.TriggerConds)
                {
                    var attr = TriggerMgr.Instance.AllAttrs[cond.GetType()];
                    string str = string.Empty;
                    var pack = cond.Pack2Json();
                    if (pack != null && pack.Length > 0)
                    {
                        foreach (var p in pack)
                        {
                            str += p+",";
                        }

                        str = str.TrimEnd(',');
                    }

                    groupData.CondTriggers.Add(new Trigger(attr.Name)
                    {
                        Param = str
                    });
                    
                }
                
                foreach (var action in v.TriggerActions)
                {
                    var attr = TriggerMgr.Instance.AllAttrs[action.GetType()];
                    string str = string.Empty;
                    var pack = action.Pack2Json();
                    if (pack != null && pack.Length > 0)
                    {
                        foreach (var p in pack)
                        {
                            str += p+",";
                        }

                        str = str.TrimEnd(',');
                    }

                    groupData.ActionTriggers.Add(new Trigger(attr.Name)
                    {
                        Param = str
                    });
                    
                }
                
            }
        }


        public string Author { get; set; }
        public string Name { get; set; }

        public string TargetJob { get; set; }

        public ObservableCollection<string> GroupIds { get; set; } = new ObservableCollection<string>();
        public Dictionary<string, GroupData> AllGroupData { get; set; } = new Dictionary<string, GroupData>();

        public string CurrChoosedId { get; set; }

    }
    
}