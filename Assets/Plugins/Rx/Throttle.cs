using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rx {
    public class Throttle<T> : Transform<T, T> {
        private long LastTime;
        private readonly TimeSpan delay;

        public Throttle(int milliseconds) {
            delay = TimeSpan.FromMilliseconds(milliseconds);
        }

        public override void OnCompleted() {}

        public override void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public override void OnNext(T value) {
            var delta = new TimeSpan(DateTime.Now.Ticks - LastTime);
            if (DateTime.Now.Ticks - LastTime >= delay.Ticks) {
                LastTime = DateTime.Now.Ticks;
                Publish(value);
            }
        }
    }
}
