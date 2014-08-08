using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    using TypePair = Tuple<Type, Type>;
    delegate bool CanHappenTogether(object t1, object t2);

    public class ConflictDetector
    {
        readonly Dictionary<TypePair, CanHappenTogether> _resolvers = new Dictionary<TypePair, CanHappenTogether>();

        public void Allow<T>()
            where T : IEvent
        {
            Allow((T left, T right) => true);
        }


        public void Allow<TLeft, TRight>()
            where TLeft : IEvent
            where TRight : IEvent
        {
            Allow((TLeft left, TRight right) => true);
        }


        public void Allow<TLeft, TRight>(Func<TLeft, TRight, bool> fn)
            where TLeft : IEvent
            where TRight : IEvent
        {
            var pair = new TypePair(typeof(TLeft), typeof(TRight));
            _resolvers.Add(pair, (tl, tr) => fn((TLeft)tl, (TRight)tr));
            if (typeof(TLeft) == typeof(TRight))
                return;

            var pair2 = new TypePair(typeof(TRight), typeof(TLeft));
            _resolvers.Add(pair2, (tr, tl) => fn((TLeft)tl, (TRight)tr));
        }

        public bool HasConflicts(IList<IEvent> committed, IList<IEvent> uncommitted)
        {
            var product = 
                from left in committed
                from right in uncommitted
                select new {Committed = left, Uncommitted = right};

            return product.Any(x => HasConflict(x.Committed, x.Uncommitted));
        }


        bool HasConflict(IEvent committed, IEvent uncommitted)
        {
            CanHappenTogether canHappenTogether;
            var pair = new TypePair(committed.GetType(), uncommitted.GetType());
            if (_resolvers.TryGetValue(pair,out canHappenTogether))
            {
                return !canHappenTogether(committed,uncommitted);
            }
            //TODO: Could try to walk up the type hierarchy before failing

            return true;
        }
    }
}