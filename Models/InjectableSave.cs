using System;
using PolyhydraGames.Core.Interfaces;

namespace PolyhydraGames.Core.Models
{
    public abstract class InjectableSave : IInjectableSave
    {
        private Action? _invokeSave;

        public void Save()
        {
            Action? invokeSave = this._invokeSave;
            if (invokeSave == null)
                return;
            invokeSave();
        }

        public void Initialize(Action invokeSave) => this._invokeSave = invokeSave;
    }
}
