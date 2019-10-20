using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Graphics {
    public class Quad {

        public static Mesh Make(GameObject obj, SpacePoint blPos, SpacePoint tlPos, SpacePoint trPos, SpacePoint brPos) {
            Vector3[] vertices;
            int[] triangles;

            vertices = new Vector3[4];

            var bl = 0;
            var tl = 1;
            var br = 2;
            var tr = 3;

            vertices[bl] = blPos.ToLocalSpace(obj).Position;
            vertices[tl] = tlPos.ToLocalSpace(obj).Position;
            vertices[br] = brPos.ToLocalSpace(obj).Position;
            vertices[tr] = trPos.ToLocalSpace(obj).Position;

            triangles = new int[6];

            triangles[0] = bl; //BL
            triangles[1] = tl; //TL
            triangles[2] = br; //BR

            triangles[3] = tl; //BL
            triangles[4] = tr; //TL
            triangles[5] = br; //BR

            return ToMesh(vertices, triangles);
        }

        private static Mesh ToMesh(Vector3[] vertices, int[] triangles, bool calcNormals = true) {
            var mesh = new Mesh();
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;

            if (calcNormals)
                mesh.RecalculateNormals();
            return mesh;
        }
    }
}
