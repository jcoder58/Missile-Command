using System;
using System.Collections.Generic;
using UnityEngine;

using static UnityEngine.Time;

namespace Graphics {
    public class Tweener2d {
        public static IEnumerable<Vector2> LinearTweener(Vector2 start, Vector2 end, float velocity, float minDistance = 0.1f) {
            var lastMove = time;
            Vector2 direction = end - start;
            Vector2 next = start;
            var lastDelta = float.MaxValue;
            while (true) {
                yield return next;
                var delta = Vector2.Distance(end, next);
                if (delta > lastDelta)
                    break;
                lastDelta = delta;
                var deltaTime = time - lastMove;
                var deltaDir = direction * velocity * deltaTime;
                next += deltaDir;
            }
            yield return end;
        }

        public static IEnumerable<float> LinearTweener(float start, float end, float velocity, float minDistance = 0.1f) {
            var lastMove = time;
            var direction = end - start;
            var next = start;
            var lastDelta = float.MaxValue;
            while (true) {
                yield return next;
                var delta = Math.Abs(end - next);
                if (delta < minDistance)
                    break;
                lastDelta = delta;
                var deltaTime = time - lastMove;
                var deltaDir = direction * velocity * deltaTime;
                next += deltaDir;
            }
            yield return end;
        }
    }
}
