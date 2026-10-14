using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using PolyhydraGames.Core.Interfaces;

namespace PolyhydraGames.Core.Models.Analytics
{
    /// <summary>
    /// Service to handle communicating with multiple analytics classes.
    /// </summary>
    ///  
#pragma warning disable S1133 // Planned removal in the next major version; see docs/release-notes/2026-10-10-deprecate-analyticsmanager.md.
    [Obsolete("AnalyticsManager is deprecated and will be removed in the next major version. Use Microsoft.Extensions.Logging.ILogger and System.Diagnostics.Metrics instead. See Core.Models#47.")]
#pragma warning restore S1133
    public class AnalyticsManager : IAnalyticsManager
    {
        public IEnumerable<IAnalytics> Services => SnapshotServices();
        private readonly List<IAnalytics> _services;
        private readonly object _sync = new();

        public AnalyticsManager() => _services = new List<IAnalytics>();

        /// <summary>
        /// Adds the service to the manager.
        /// </summary>
        /// <param name="analytics">Analytics Service.</param>
        public void AddService(IAnalytics analytics)
        {
            lock (_sync) _services.Add(analytics);
        }

        /// <summary>
        /// Runs an action on all registered analytics implementations.
        /// </summary>
        /// <param name="action">Action.</param>
        public void RunAction(Action<IAnalytics> action)
        {
            foreach (var item in SnapshotServices())
                action(item);
        }

        public bool TryRunAction(Action<IAnalytics> action)
        {
            var succeeded = true;
            foreach (var item in SnapshotServices())
            {
                try { action(item); }
                catch (Exception ex)
                {
                    succeeded = false;
                    Trace.TraceError("Analytics service action failed for {0}: {1}", item.GetType().FullName, ex);
                }
            }
            return succeeded;
        }

        public async Task RunActionAsync(Func<IAnalytics, Task> action, CancellationToken cancellationToken = default)
        {
            foreach (var item in SnapshotServices())
            {
                cancellationToken.ThrowIfCancellationRequested();
                await action(item).ConfigureAwait(false);
            }
        }

        public async Task<bool> TryRunActionAsync(Func<IAnalytics, Task> action, CancellationToken cancellationToken = default)
        {
            var succeeded = true;
            foreach (var item in SnapshotServices())
            {
                cancellationToken.ThrowIfCancellationRequested();
                try { await action(item).ConfigureAwait(false); }
                catch (Exception ex)
                {
                    succeeded = false;
                    Trace.TraceError("Analytics service action failed for {0}: {1}", item.GetType().FullName, ex);
                }
            }
            return succeeded;
        }

        private IAnalytics[] SnapshotServices()
        {
            lock (_sync) return _services.ToArray();
        }

    }
}
