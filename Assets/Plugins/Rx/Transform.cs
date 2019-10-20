using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rx {
    public abstract class Transform<TSource, TSink> : Source<TSink>, IObserver<TSource> {
        public abstract void OnCompleted();
        public abstract void OnError(Exception error);

        public abstract void OnNext(TSource value);
    }
}
