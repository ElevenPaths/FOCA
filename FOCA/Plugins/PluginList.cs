using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using FOCA.ModifiedComponents;

namespace FOCA.Plugins
{
    public class PluginList : IDisposable
    {
        public ThreadSafeList<Plugin> lstPlugins = new ThreadSafeList<Plugin>();

        /// <summary>
        ///     Add a plugin to the current plugins list
        /// </summary>
        /// <param name="plugin"></param>
        public void AddPlugin(Plugin plugin)
        {
            lstPlugins.Add(plugin);
            Start(plugin);
        }

        /// <summary>
        ///     Remove plugin from the current plugins list
        /// </summary>
        /// <param name="plugin"></param>
        public void RemovePlugin(Plugin plugin)
        {
            KillPlugin(plugin);
            lstPlugins.Remove(plugin);
        }

        /// <summary>
        ///     Kill a plugin
        /// </summary>
        /// <param name="plugin"></param>
        private void KillPlugin(Plugin plugin)
        {
            //todo write this method
        }

        /// <summary>
        ///     Get plugin by index
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Plugin GetPlugin(int i)
        {
            return lstPlugins[i];
        }

        /// <summary>
        ///     Get current plugins list length
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return lstPlugins.Count;
        }

        /// <summary>
        ///     Obtain list of currently loaded plugins
        /// </summary>
        /// <returns></returns>
        public ThreadSafeList<Plugin> GetPluginsLoaded()
        {
            return lstPlugins;
        }

        /// <summary>
        ///     Launch a plugin
        /// </summary>
        /// <param name="plugin"></param>
        public void Start(Plugin plugin)
        {
            var t = new Thread(SendSignalStart) {IsBackground = true};
            t.Start(plugin);
        }

        /// <summary>
        ///     Send signal of new domain added to all plugins
        /// </summary>
        /// <param name="oDomain"></param>
        public void OnNewDomain(object oDomain)
        {
            var t = new Thread(_OnNewDomain) {IsBackground = true};
            t.Start(oDomain);
        }

        /// <summary>
        ///     Send signal of new domain added to all plugins
        /// </summary>
        /// <param name="oDomain"></param>
        private void _OnNewDomain(object oDomain)
        {
            foreach (var plugin in lstPlugins)
                SendSignal(plugin, "OnNewDomain", (object[]) oDomain);
        }

        /// <summary>
        ///     Send signal of new URL added to all plugins
        /// </summary>
        /// <param name="oURL"></param>
        public void OnNewURL(object oURL)
        {
            var t = new Thread(_OnNewURL) {IsBackground = true};
            t.Start(oURL);
        }

        /// <summary>
        ///     Send signal of new URL added to all plugins
        /// </summary>
        /// <param name="oUrl"></param>
        private void _OnNewURL(object oUrl)
        {
            foreach (var plugin in lstPlugins)
                SendSignal(plugin, "OnNewURL", (object[]) oUrl);
        }

        /// <summary>
        ///     Send signal of new IP added to all plugins
        /// </summary>
        /// <param name="oIP"></param>
        public void OnNewIP(object oIP)
        {
            var t = new Thread(_OnNewIP) {IsBackground = true};
            t.Start(oIP);
        }

        /// <summary>
        ///     Send signal of new IP added to all plugins
        /// </summary>
        /// <param name="oIp"></param>
        private void _OnNewIP(object oIp)
        {
            foreach (var plugin in lstPlugins)
                SendSignal(plugin, "OnNewIP", (object[]) oIp);
        }

        /// <summary>
        ///     Send signal of new project added to all plugins
        /// </summary>
        /// <param name="oProject"></param>
        public void OnNewProject(object oProject)
        {
            var t = new Thread(_OnNewProject) {IsBackground = true};
            t.Start(oProject);
        }

        /// <summary>
        ///     Send signal of new project added to all plugins
        /// </summary>
        /// <param name="oProject"></param>
        private void _OnNewProject(object oProject)
        {
            foreach (var plugin in lstPlugins)
                SendSignal(plugin, "OnNewProject", (object[]) oProject);
        }

        /// <summary>
        ///     Send signal of new netrange added to all plugins
        /// </summary>
        /// <param name="oNetRange"></param>
        public void OnNewNetrange(object oNetRange)
        {
            var t = new Thread(_OnNewNetrange) {IsBackground = true};
            t.Start(oNetRange);
        }

        /// <summary>
        ///     Send signal of new netrange added to all plugins
        /// </summary>
        /// <param name="oNetRange"></param>
        private void _OnNewNetrange(object oNetRange)
        {
            foreach (var plugin in lstPlugins)
                SendSignal(plugin, "OnNewNetrange", (object[]) oNetRange);
        }

        /// <summary>
        ///     Send signal of new relationship added to all plugins
        /// </summary>
        /// <param name="oRelation"></param>
        public void OnNewRelation(object oRelation)
        {
            var t = new Thread(_OnNewRelation) {IsBackground = true};
            t.Start(oRelation);
        }

        /// <summary>
        ///     Send signal of relationship added to all plugins
        /// </summary>
        /// <param name="oRelation"></param>
        private void _OnNewRelation(object oRelation)
        {
            foreach (var plugin in lstPlugins)
                SendSignal(plugin, "OnNewRelation", (object[]) oRelation);
        }

        /// <summary>
        ///     Send signal of new document added to all plugins
        /// </summary>
        /// <param name="oDocument"></param>
        public void OnNewDocument(object oDocument)
        {
            var t = new Thread(_OnNewDocument) {IsBackground = true};
            t.Start(oDocument);
        }

        /// <summary>
        ///     Send signal of new document added to all plugins
        /// </summary>
        /// <param name="oDocument"></param>
        private void _OnNewDocument(object oDocument)
        {
            foreach (var plugin in lstPlugins)
                SendSignal(plugin, "OnNewDocument", (object[]) oDocument);
        }

        /// <summary>
        ///     Send signal to a plugin
        /// </summary>
        /// <param name="p"></param>
        /// <param name="method"></param>
        /// <param name="param"></param>
        private static void SendSignal(Plugin p, string method, object[] param)
        {
#if PLUGINS
            try
            {
                p.pluginType.InvokeMember(method,
                    BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
                    null, p.pluginInstance, param);
            }
            catch (Exception)
            {
            }
#endif
        }

        /// <summary>
        ///     Send "Start" signal to a plugin
        /// </summary>
        /// <param name="oPlugin"></param>
        private static void SendSignalStart(object oPlugin)
        {
#if PLUGINS
            var p = (Plugin) oPlugin;
            try
            {
                p.pluginType.InvokeMember("Start",
                    BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
                    null, p.pluginInstance, new object[] {"FOCA"});
            }
            catch (Exception)
            {
            }
#endif
        }

        #region IDisposable Support

        private bool disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue) return;
            if (disposing)
            {
                lstPlugins.Dispose();
            }

            lstPlugins = null;

            disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}