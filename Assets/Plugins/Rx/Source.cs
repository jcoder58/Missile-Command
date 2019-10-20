using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rx {
    public class Source<T> : IObservable<T> {
        private readonly HashSet<IObserver<T>> observers = new HashSet<IObserver<T>>();

        public void Publish( T value) {
            foreach (var o in observers)
                o.OnNext(value);
        }

        public void End() {
            foreach (var o in observers)
                o.OnCompleted();
        }

        public void Error( Exception error) {
            foreach (var o in observers)
                o.OnError(error);
        }

        public IDisposable Subscribe(IObserver<T> observer) {
            observers.Add(observer);

            return new Dispose(observer, this);
        }

        private class Dispose : IDisposable {
            IObserver<T> observer;
            Source<T> source;

            void IDisposable.Dispose() {
                source.observers.Remove(observer);
            }

            public Dispose(IObserver<T> o, Source<T> s) {
                observer = o;
                source = s;
            }
        }
    }
}
