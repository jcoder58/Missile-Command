using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rx {
    public static class RxStatic {
        private static FrameSource frame = new FrameSource();
        private static MouseCatch primaryMouse;
        private static MouseCatch secondaryMouse;
        private static MouseCatch centerMouse;

        public static void OnFrame() {
            frame.OnFrame();
        }
        public static IObservable<TimeSpan> Frame 
            { get { return frame;  } }

        public static IObservable<MouseData> PrimaryMouse {
            get {
                if (primaryMouse == null) {
                    primaryMouse = new MouseCatch(0);
                    Frame.Subscribe(primaryMouse);
                }
                return primaryMouse as IObservable<MouseData>;
            }
        }

        public static IObservable<TSource> Where<TSource>(this IObservable<TSource> self, Func<TSource,bool> whereFunc) {
            var where = new Where<TSource>(whereFunc);
            self.Subscribe(where);
            return where as IObservable<TSource>;
        }

        public static IObservable<TimeSpan> AnyKey =>
            Frame.Where<TimeSpan>(_ => Input.anyKey);

        public static IObservable<TimeSpan> AnyKeyDown =>
            Frame.Where<TimeSpan>(_ => Input.anyKeyDown);

        public static IObservable<TimeSpan> OnKey(KeyCode key) =>
            AnyKey.Where(_ => Input.GetKey(key));

        public static IObservable<TSource> Throttle<TSource>(this IObservable<TSource> self, int milliseconds) {
            var throttle = new Throttle<TSource>(milliseconds);
            self.Subscribe(throttle);
            return throttle as IObservable<TSource>;
        }

        public static IObserver<T> Sink<T>(this IObservable<T> self, Action<T> next) {
            var sink = new Sink<T>(next);
            self.Subscribe(sink);
            return sink;
        }
    }
}
