using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Es.InkPainter.Sample
{
    public class Post : MonoBehaviour
    {
        public Color red;
        public Color blue;
        public Character character;

        [System.Serializable]
        private enum UseMethodType
        {
            RaycastHitInfo,
            WorldPoint,
            NearestSurfacePoint,
            DirectUV,
        }

        [SerializeField]
        private Brush brush;

        [SerializeField]
        private UseMethodType useMethodType = UseMethodType.RaycastHitInfo;

        private void Update()
        {
            InitColor();
        }

        private void OnCollisionEnter(Collision collision)
        {
            var hit = Physics.SphereCastAll(transform.position,0.3f,transform.forward,03f,1<<8);
            bool success = true;

            foreach (var item in hit)
            {
                Debug.Log("子弹在物体："+ item.collider.gameObject.name+"发生碰撞");
                Destroy(gameObject);
                var paintObject = item.transform.GetComponent<InkCanvas>();
                if (paintObject != null)
                    switch (useMethodType)
                    {
                        case UseMethodType.RaycastHitInfo:
                            success = paintObject.Paint(brush, item);
                            break;

                        case UseMethodType.WorldPoint:
                            success = paintObject.Paint(brush, item.point);
                            break;

                        case UseMethodType.NearestSurfacePoint:
                            success = paintObject.PaintNearestTriangleSurface(brush, item.point);
                            break;

                        case UseMethodType.DirectUV:
                            if (!(item.collider is MeshCollider))
                                Debug.LogWarning("Raycast may be unexpected if you do not use MeshCollider.");
                            success = paintObject.PaintUVDirect(brush, item.textureCoord);
                            break;
                    }
                if (!success)
                    Debug.LogError("Failed to paint.");
            }
        }

        void InitColor()
        {
            if (character.isBlue)
            {
                brush.Color = blue;
            }
            else
            {
                brush.Color = red;
            }
        }
    }
}
