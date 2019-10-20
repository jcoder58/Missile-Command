using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugGameScript : MonoBehaviour {
    public float LineWidth = 0.1f;
    private LineRenderer Renderer;
    public Collider2D Collider;

    private Vector3[] Lines = new Vector3[0];

    // Start is called before the first frame update
    void Start() {
        Renderer = GetComponent<LineRenderer>();
        Renderer.startWidth = Renderer.endWidth = LineWidth;

    }

    // Update is called once per frame
    void Update() {
        if (Collider == null)
            return;
        if (Collider is BoxCollider2D) {
            Lines = GetBoxPoints(Collider as BoxCollider2D);
        } else if (Collider is CircleCollider2D) {
            Lines = GetCircleColliderPoints(Collider as CircleCollider2D);
        }
        if (Lines.Length > 0) {
            Renderer.positionCount = Lines.Length;
            Renderer.SetPositions(Lines);
        }
    }

    private Vector3[] GetBoxPoints(BoxCollider2D collider) {
        Vector2 scale = collider.size;
        scale *= 0.5f;
        Vector3[] points = new Vector3[5];

        points[0] = collider.transform.TransformPoint(new Vector3(-scale.x + collider.offset.x, scale.y + collider.offset.y, 0));
        points[1] = collider.transform.TransformPoint(new Vector3(scale.x + collider.offset.x, scale.y + collider.offset.y, 0));
        points[2] = collider.transform.TransformPoint(new Vector3(scale.x + collider.offset.x, -scale.y + collider.offset.y, 0));
        points[3] = collider.transform.TransformPoint(new Vector3(-scale.x + collider.offset.x, -scale.y + collider.offset.y, 0));
        points[4] = points[0];

        return points;
    }

    Vector3[] GetCircleColliderPoints(CircleCollider2D collider, int segments = 32) {
        float radius = collider.radius;
        float angle = collider.transform.rotation.z;
        float segmentSize = 360f / segments;
        Vector3[] circlePoints = new Vector3[segments + 3];

        //drawing the angle line
        circlePoints[0] = collider.transform.TransformPoint(new Vector3(collider.offset.x, collider.offset.y));
        circlePoints[1] = collider.transform.TransformPoint(new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * radius + collider.offset.x, Mathf.Sin(Mathf.Deg2Rad * angle) * radius + collider.offset.y));

        for (int i = 0; i < segments; i++) {
            Vector3 p = collider.transform.TransformPoint(new Vector3(Mathf.Cos(Mathf.Deg2Rad * (i * segmentSize + angle)) * radius + collider.offset.x, Mathf.Sin(Mathf.Deg2Rad * (i * segmentSize + angle)) * radius + collider.offset.y));
            circlePoints[i + 2] = p;
        }

        circlePoints[segments + 2] = circlePoints[1];
        return circlePoints;
    }

}
