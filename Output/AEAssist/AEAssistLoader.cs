using Clio.Utilities;
using ff14bot;
using ff14bot.AClasses;
using ff14bot.Enums;
using ff14bot.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using TreeSharp;

namespace AEAssistLoader
{
    public class AEAssistLoader : CombatRoutine
    {
        private const string ProjectName = "AEAssist";
        private const string ProjectAssemblyName = "AEAssist.dll";

        private static readonly string ProjectAssembly = Path.Combine(Environment.CurrentDirectory,
            $@"Routines\{ProjectName}\{ProjectAssemblyName}");

        private static readonly string GreyMagicAssembly = Path.Combine(Environment.CurrentDirectory, @"GreyMagic.dll");

        public static readonly HashSet<string> ExternelDlls = new HashSet<string>
        {
            "MongoDB.Bson"
        };

        public Dictionary<string, PropertyInfo> Behaviors = new Dictionary<string, PropertyInfo>();

        public object Entry;

        private bool Loaded;
        public Dictionary<string, MethodInfo> Methods = new Dictionary<string, MethodInfo>();

        public override string Name => ProjectName;

        public override float PullRange => 25;

        public override bool WantButton => true;

        public override ClassJobType[] Class
        {
            get
            {
                switch (Core.Me.CurrentJob)
                {
                    case ClassJobType.Bard:
                    case ClassJobType.Reaper:
                    case ClassJobType.Machinist:
                    case ClassJobType.Dancer:
                        return new[] { Core.Me.CurrentJob };
                    case ClassJobType.Samurai:
                    case ClassJobType.Sage:
                    case ClassJobType.Summoner:
                    case ClassJobType.WhiteMage:
                    case ClassJobType.Astrologian:
                    case ClassJobType.BlackMage:
                    case ClassJobType.Monk:
                    case ClassJobType.Ninja:
                    case ClassJobType.Gunbreaker:
                    case ClassJobType.Paladin:
                    case ClassJobType.Warrior:
                    case ClassJobType.DarkKnight:
                    case ClassJobType.Scholar:
                    case ClassJobType.RedMage:
                    case ClassJobType.Dragoon:


                        Logging.Write(Colors.Red, $@"[AEAssist] {Core.Me.CurrentJob} is only for develop.");
                        return new[] { Core.Me.CurrentJob };
                    default:
                        {
                            Logging.Write(Colors.Red, $@"[AEAssist] {Core.Me.CurrentJob} is not supported.");
                            return new[] { ClassJobType.Adventurer };
                        }
                }
            }
        }

        private static string CompiledAssembliesPath => Path.Combine(Utilities.AssemblyDirectory, "CompiledAssemblies");

        public override Composite RestBehavior
        {
            get
            {
                if (!Behaviors.TryGetValue("RestBehavior", out var prop)) return default;

                return prop.GetValue(Entry, null) as Composite;
            }
        }


        public override Composite PreCombatBuffBehavior
        {
            get
            {
                if (!Behaviors.TryGetValue("PreCombatBuffBehavior", out var prop)) return default;

                return prop.GetValue(Entry, null) as Composite;
            }
        }

        public override Composite PullBehavior
        {
            get
            {
                if (!Behaviors.TryGetValue("PullBehavior", out var prop)) return default;

                return prop.GetValue(Entry, null) as Composite;
            }
        }

        public override Composite HealBehavior
        {
            get
            {
                if (!Behaviors.TryGetValue("HealBehavior", out var prop)) return default;

                return prop.GetValue(Entry, null) as Composite;
            }
        }

        public override Composite CombatBuffBehavior
        {
            get
            {
                if (!Behaviors.TryGetValue("CombatBuffBehavior", out var prop)) return default;

                return prop.GetValue(Entry, null) as Composite;
            }
        }

        public override Composite CombatBehavior
        {
            get
            {
                if (!Behaviors.TryGetValue("CombatBehavior", out var prop)) return default;

                return prop.GetValue(Entry, null) as Composite;
            }
        }

        public override Composite PullBuffBehavior
        {
            get
            {
                if (!Behaviors.TryGetValue("PullBuffBehavior", out var prop)) return default;

                return prop.GetValue(Entry, null) as Composite;
            }
        }

        private void LoadAsm()
        {
            RedirectAssembly();
            var path = ProjectAssembly;
            var asm = LoadAssembly(path);

            var entryType = asm.GetType("AEAssist.Entry");

            Entry = Activator.CreateInstance(entryType);
            Behaviors.Clear();
            Methods.Clear();

            AddBehavior(entryType, "RestBehavior");
            AddBehavior(entryType, "PreCombatBuffBehavior");
            AddBehavior(entryType, "PullBehavior");
            AddBehavior(entryType, "HealBehavior");
            AddBehavior(entryType, "CombatBuffBehavior");
            AddBehavior(entryType, "CombatBehavior");
            AddBehavior(entryType, "PullBuffBehavior");

            AddMethod(entryType, "Initialize");
            AddMethod(entryType, "Pulse");
            AddMethod(entryType, "Shutdown");
            AddMethod(entryType, "OnButtonPress");
        }

        public static void RedirectAssembly()
        {
            ResolveEventHandler handler = (sender, args) =>
            {
                var name = Assembly.GetEntryAssembly().GetName().Name;
                var requestedAssembly = new AssemblyName(args.Name);
                return requestedAssembly.Name != name ? null : Assembly.GetEntryAssembly();
            };

            AppDomain.CurrentDomain.AssemblyResolve += handler;

            ResolveEventHandler greyMagicHandler = (sender, args) =>
            {
                var requestedAssembly = new AssemblyName(args.Name);
                return requestedAssembly.Name != "GreyMagic" ? null : Assembly.LoadFrom(GreyMagicAssembly);
            };


            AppDomain.CurrentDomain.AssemblyResolve += greyMagicHandler;

            ResolveEventHandler MaterialDesignHandler = (sender, args) =>
            {
                var requestedAssembly = new AssemblyName(args.Name);
                if (ExternelDlls.Contains(requestedAssembly.Name))
                    return Assembly.LoadFrom(GetAssPath(requestedAssembly.Name));
                return null;
            };
            AppDomain.CurrentDomain.AssemblyResolve += MaterialDesignHandler;
        }

        private static string GetAssPath(string name)
        {
            return Path.Combine(Environment.CurrentDirectory, $@"Routines\{ProjectName}\{name}.dll");
        }

        private static Assembly LoadAssembly(string path)
        {
            if (!File.Exists(path)) return null;

            if (!Directory.Exists(CompiledAssembliesPath)) Directory.CreateDirectory(CompiledAssembliesPath);

            var t = DateTime.Now.Ticks;
            var name = $"{Path.GetFileNameWithoutExtension(path)}{t}.{Path.GetExtension(path)}";
            var pdbPath = path.Replace(Path.GetExtension(path), "pdb");
            var pdb = $"{Path.GetFileNameWithoutExtension(path)}{t}.pdb";
            var capath = Path.Combine(CompiledAssembliesPath, name);
            Logging.Write($"Asm: {capath} origin {path}");
            if (File.Exists(capath))
                try
                {
                    File.Delete(capath);
                }
                catch (Exception)
                {
                    //
                }

            if (File.Exists(pdb))
                try
                {
                    File.Delete(pdb);
                }
                catch (Exception)
                {
                    //
                }

            if (!File.Exists(capath)) File.Copy(path, capath);

            if (!File.Exists(pdb) && File.Exists(pdbPath)) File.Copy(pdbPath, pdb);


            Assembly assembly = null;
            try
            {
                assembly = Assembly.LoadFrom(capath);
            }
            catch (Exception e)
            {
                Logging.WriteException(e);
            }

            return assembly;
        }

        private void AddBehavior(Type type, string name)
        {
            Behaviors.Add(name, type.GetProperty(name));
        }

        private void AddMethod(Type type, string name)
        {
            Methods.Add(name, type.GetMethod(name));
        }

        public override void Initialize()
        {
            base.Initialize();
            try
            {
                LoadAsm();
                Methods["Initialize"].Invoke(Entry, null);
            }
            catch (Exception e)
            {
                Logging.Write(Colors.Red, e.ToString());
            }
        }

        public override void OnButtonPress()
        {
            base.OnButtonPress();
            if (!Methods.TryGetValue("OnButtonPress", out var method)) return;

            method.Invoke(Entry, null);
        }

        public override void Pulse()
        {
            base.Pulse();
            if (!Methods.TryGetValue("Pulse", out var method)) return;

            method.Invoke(Entry, null);
        }

        public override void ShutDown()
        {
            base.ShutDown();
            if (!Methods.TryGetValue("Shutdown", out var method)) return;

            method.Invoke(Entry, null);
        }
    }
}