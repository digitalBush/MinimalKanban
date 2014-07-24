using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Domain;

namespace Denormalizers
{
    public static class Denormalizer
    {
        internal static ILifetimeScope Container;
        public static void UseContainer(ILifetimeScope container)
        {
            Container = container;
        }
    }

    public interface IDenormalizer<TState>
    {
        //Marker Interface
    }
    
    public abstract class Denormalizer<TState> : IDenormalizer<TState> where TState: class, new()
    {
        protected void WithState(Func<TState, TState> change)
        {
            using (var scope = Denormalizer.Container.BeginLifetimeScope())
            {
                var repository = scope.Resolve<IStateRepository>();
                var ui = scope.Resolve<IUIEventDispatcher>();

                //Lock?
                var key = GetType().FullName;
                var startingState = repository.Get<TState>(key) ?? new TState();
                var newState = change(startingState);
                repository.Save(key, newState);
                //Unlock?

                ui.Notify(GetType().Name, newState);

            }
        }

        protected T Query<T>(Func<TState, T> transformation)
        {
            using (var scope = Denormalizer.Container.BeginLifetimeScope())
            {
                var repository = scope.Resolve<IStateRepository>();
                var key = GetType().FullName;
                return transformation(repository.Get<TState>(key));
            }
        }
    }

    public abstract class Denormalizer<TKey, TState> : IDenormalizer<TState>
    {
        protected void WithState(TKey key, Func<TState, TState> transform)
        {
            WithState(new[]{key},(k, v)=>transform(v));
        }
        protected void WithState(IEnumerable<TKey> keys, Func<TKey, TState, TState> transform)
        {
            //Lock across all incoming keys?
            using (var scope = Denormalizer.Container.BeginLifetimeScope())
            {
                var repository = scope.Resolve<IStateRepository>();
                var ui = scope.Resolve<IUIEventDispatcher>();

                foreach (var key in keys)
                {
                    var typedKey = GetType().FullName + ":" + key;
                    var startingState = repository.Get<TState>(typedKey);
                    var newState = transform(key, startingState);
                    repository.Save(typedKey, newState);

                    ui.Notify(GetType().Name, key.ToString(), newState);
                }
            }
        }

        protected void DependsOn<TQuery>(Action<TQuery> dependency)
        {
            //How to track dependency graph?
            //IOC type problem where to run x projection, we also need to build up y and z projections

            //We might be able to handle 1:1 here, 1:n might be very difficult to express
            //You might event need to issue a query to see what keys to use based on some data
            // * This is bad because it creates a dependency on the denormalizer that feeds that query 
            //   that will have to always be re-projected together
        }


        protected IEnumerable<T> Query<T>(IEnumerable<TKey> keys, Func<TKey, TState, T> transformation)
        {
            using (var scope = Denormalizer.Container.BeginLifetimeScope())
            {
                var repository = scope.Resolve<IStateRepository>();

                var typedKeys = keys.ToDictionary(key => GetType().FullName + ":" + key, key => key);
                return repository
                    .GetList<TState>(typedKeys.Keys)
                    .Select(x => transformation(typedKeys[x.Key], x.Value));
            }
        }

        protected TState Query(TKey key)
        {
            return Query(key, x => x);
        }

        protected T Query<T>(TKey key, Func<TState, T> transform)
        {
            using (var scope = Denormalizer.Container.BeginLifetimeScope())
            {
                var repository = scope.Resolve<IStateRepository>();

                var typedKey = GetType().FullName + ":" + key;
                return transform(repository.Get<TState>(typedKey));
            }
        }
    }

    
}