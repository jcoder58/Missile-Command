using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Graphics {

    public enum Space {
        World,
        Local,
        Screen,
    }

    public struct SpacePoint {
        public readonly Space Space;
        public readonly Vector3 Position;
        public readonly GameObject Obj;

        public SpacePoint(Space space, Vector3 pos, GameObject obj = null) {
            if (space == Space.Local && obj == null)
                throw new ArgumentException("Transform must be specified for Local Space");
            Space = space;
            Position = pos;
            Obj = obj;
        }

        public override string ToString() =>
            $"Spacepoint {Position} in {Space}";

        public SpacePoint ChangeZ(float z) =>
            new SpacePoint(Space, new Vector3(Position.x, Position.y, z), Obj);

        public SpacePoint ToWorldSpace() {
            switch (Space) {
                case Space.World:
                    return this;
                case Space.Screen:
                    return new SpacePoint(Space.World,
                        Camera.main.ScreenToWorldPoint(Position));
                case Space.Local:
                default:
                    throw new NotImplementedException();
            }
        }

        public SpacePoint ToLocalSpace(GameObject obj) {
            switch (Space) {
                case Space.World:
                    var l1 = obj.transform.TransformPoint(Position);
                    return new SpacePoint(Space.Local, l1, obj);
                case Space.Screen:
                    var w1 = Camera.main.ScreenToWorldPoint(Position);
                    var l2 = obj.transform.TransformPoint(w1);
                    return new SpacePoint(Space.Local, l2, obj);
                case Space.Local:
                default:
                    throw new NotImplementedException();
            }
        }

    }

    public static class SpacePointStatic {
        public static SpacePoint ScreenPoint(float x, float y) =>
            new SpacePoint(Space.Screen, new Vector3(x, y, 0));

        public static SpacePoint MousePosition =>
            new SpacePoint(Graphics.Space.Screen, Input.mousePosition);

        public static Vector2 Delta(SpacePoint start, SpacePoint end) {
            var s = start.ToWorldSpace().Position;
            var e = end.ToWorldSpace().Position;
            var change = (e - s).normalized;
            return new Vector2(change.x, change.y);
        }

    }
}
