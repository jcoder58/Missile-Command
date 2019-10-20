using System;
using System.Collections.Generic;
using System.Linq;

namespace Rx {
    internal class Where<TSource> : Transform<TSource, TSource> {
        private readonly Func<TSource, bool> where;

        public Where(Func<TSource,bool> w) {
            where = w; 
        }
        public override void OnCompleted() {
        }

        public override void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public override void OnNext(TSource value) {
            if (where(value))
                Publish(value);
        }
    }
}
