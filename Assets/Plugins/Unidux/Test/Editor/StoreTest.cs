﻿using System;
using NUnit.Framework;
using Unidux.Rx;
using UniRx;

namespace Unidux
{
    public class StoreTest
    {
        [Test]
        public void SubscribeTest()
        {
            var store = new Store<State>(new State());
            var observer = new TestObserver<State>();

            store.Subject.Subscribe(observer);
            Assert.AreEqual(0, observer.OnNextCount);
            Assert.AreEqual(0, observer.OnErrorCount);
            Assert.AreEqual(0, observer.OnCompletedCount);

            store.Dispatch(new Action());
            store.ForceUpdate();

            Assert.AreEqual(1, observer.OnNextCount);
            Assert.AreEqual(0, observer.OnErrorCount);
            Assert.AreEqual(0, observer.OnCompletedCount);
        }

        [Test]
        public void ReducerSubscribeTest()
        {
            var store = new Store<State>(new State());
            var reducer = new SampleReducer();
            store.AddReducer<Action>(reducer.Reduce);

            store.Dispatch(new Action());
            Assert.AreEqual(1, reducer.Count);

            store.Dispatch(new Action());
            Assert.AreEqual(2, reducer.Count);

            store.RemoveReducer<Action>(reducer.Reduce);
            store.Dispatch(new Action());
            Assert.AreEqual(2, reducer.Count);
        }

        [Test]
        public void ResetStateChangedTest()
        {
            var store = new Store<State>(new State());
            var count = 0;

            store.Subject.Subscribe(state =>
            {
                count++;
                Assert.IsTrue(state.Changed.IsStateChanged());
            });

            Assert.IsFalse(store.State.Changed.IsStateChanged());
            store.State.Changed.SetStateChanged();

            store.ForceUpdate();
            Assert.IsFalse(store.State.Changed.IsStateChanged());
            Assert.AreEqual(1, count);
        }

        [Serializable]
        class State : StateBase<State>
        {
            public ChangedState Changed { get; set; }

            public State()
            {
                this.Changed = new ChangedState();
            }
        }

        [Serializable]
        class ChangedState : StateElement<ChangedState>
        {
        }

        class Action
        {
        }

        class SampleReducer
        {
            public int Count = 0;

            public State Reduce(State state, Action action)
            {
                Count++;
                return state;
            }
        }
    }
}