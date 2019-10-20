using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Graphics {
    using Obj = UnityEngine.Object;
    public static class GraphicsObjects {
        public static GameObject NewGameObject(string name, params Type[] components) {
            var go = new GameObject(name, components);
            return go;
        }

        public static GameObject Instantiate(string objName) {
            var obj = GameObject.Find(objName);
            return Obj.Instantiate<GameObject>(obj);
        }

        public static GameObject NewGameObject(params Type[] components) =>
            NewGameObject("", components);

        public static Material NewMaterial(Shader shader) =>
            new Material(shader);

        //TODO: Test
        public static Shader Color =>
            Shader.Find("Unit/Color");
    }
}
