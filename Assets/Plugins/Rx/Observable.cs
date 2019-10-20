using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rx {
    public class Observable<T> : IObservable<T> {
        private Dictionary<IObserver<T>, int> Registered = new Dictionary<IObserver<T>, int>();

        internal class Dispose : IDisposable {
            public readonly Observable<T> Observable;
            public readonly IObserver<T> Observer;

            void IDisposable.Dispose() {
                Observable.Registered.Remove(Observer);
            }

            public Dispose(Observable<T> observable, IObserver<T> observer) {
                Observer = observer;
                Observable = observable;
            }
        }

        public IDisposable Subscribe(IObserver<T> observer) {
            Registered.Add(observer, 0);
            return new Dispose(this, observer);
        }
    }
}
