using System;
using UnityEngine;

namespace Rx {

    public struct MouseData {
        public TimeSpan FrameTime { get; internal set; }
        public int Button { get; internal set; }
        public bool IsDown { get; internal set; }
        public Vector3 Position {get; internal set; }
    }

    internal class MouseCatch : Transform<TimeSpan, MouseData> {
        private readonly int mouseButton;

        public MouseCatch( int button) {
            mouseButton = button;
        }

        public override void OnCompleted() {
        }

        public override void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public override void OnNext(TimeSpan value) {
            MouseData data = new MouseData {
                FrameTime = value,
                Button = mouseButton,
                Position = Input.mousePosition,
                IsDown = Input.GetMouseButtonDown(mouseButton),
            };

            Publish(data);
        }
    }
}
