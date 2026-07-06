using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PolyhydraGames.Core.Interfaces;

namespace PolyhydraGames.Core.Models.Analytics
{
    /// <summary>
    /// Service to handle communicating with multiple analytics classes.
    /// </summary>
    ///  
    public class AnalyticsManager : IAnalyticsManager{
        public IEnumerable<IAnalytics> Services => _services;
        private readonly List<IAnalytics> _services;

        public AnalyticsManager() => _services = new List<IAnalytics> ();

        /// <summary>
        /// Adds the service to the manager.
        /// </summary>
        /// <param name="analytics">Analytics Service.</param>
        public void AddService(IAnalytics analytics)=> _services.Add (analytics);
		
        /// <summary>
        /// Runs an action on all registered analytics implementations.
        /// </summary>
        /// <param name="action">Action.</param>
        public void RunAction(Action<IAnalytics> action){
            foreach (var item in Services)
                action (item);
        }

        public bool TryRunAction(Action<IAnalytics> action)
        {
            try
            {
                RunAction(action);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task RunActionAsync(Func<IAnalytics, Task> action)
        {
            foreach (var item in Services)
                await action(item).ConfigureAwait(false);
        }

        public async Task<bool> TryRunActionAsync(Func<IAnalytics, Task> action)
        {
            try
            {
                await RunActionAsync(action).ConfigureAwait(false);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
