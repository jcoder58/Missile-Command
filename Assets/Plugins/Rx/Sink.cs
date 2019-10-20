using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rx {
    internal class Sink<T> : IObserver<T> {

        Action<T> Next;
        Action<Exception> Error;
        Action Completed;

        public void OnCompleted() {
            Completed?.Invoke();
        }

        public void OnError(Exception error) {
            Error?.Invoke(error);
        }

        public void OnNext(T value) {
            Next?.Invoke(value);
        }

        public Sink(Action<T> next, Action complete = null, Action<Exception> error = null) {
            Next = next;
            Completed = complete;
            Error = error;
        }
    }
}
