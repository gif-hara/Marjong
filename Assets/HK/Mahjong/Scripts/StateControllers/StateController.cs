using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.Mahjong.StateControllers
{
    /// <summary>
    /// <see cref="IState"/>の切り替えを行うクラス
    /// </summary>
    public sealed class StateController<TStateName> : IDisposable
    {
        private readonly Dictionary<TStateName, IState<TStateName>> states;

        private IState<TStateName> current;

        private CompositeDisposable disposable = new();

        public StateController(List<IState<TStateName>> states, TStateName initialStateName, IStateArgument argument = null)
        {
            this.states = states.ToDictionary(x => x.StateName);
            this.Change(initialStateName, argument);
        }

        public void Change(TStateName stateName, IStateArgument argument = null)
        {
            if (this.current != null)
            {
                this.current.Exit();
                this.disposable.Clear();
            }

            Assert.IsTrue(this.states.ContainsKey(stateName), $"{stateName}という{nameof(IState<TStateName>)}は存在しません");
            this.current = this.states[stateName];
            this.current.Enter(this, this.disposable, argument);
        }

        public void Dispose()
        {
        }
    }
}
