using System;
using PolyhydraGames.Core.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace PolyhydraGames.Core.Models
{
    public abstract class InjectableSave : IInjectableSaveAsync
    {
        private Func<Task>? _invokeSave;

        public async ValueTask SaveAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Func<Task>? invokeSave = this._invokeSave;
            if (invokeSave == null)
                return;
            await invokeSave().ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
        }

        public void Initialize(Func<Task> invokeSave) => this._invokeSave = invokeSave;
    }
}
