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
        public Rigidbody shell;

        public Character.chaColor shellCurColor = Character.chaColor.None;

        public List<RaycastHit> hit = new List<RaycastHit>();


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

        private void Start()
        {
            InitColor();

            shell = GetComponent<Rigidbody>();
        }
        private void Update()
        {

        }

        private void OnCollisionEnter(Collision collision)
        {
            var hit = Physics.RaycastAll(transform.position, transform.forward, 1.2f, 1 << 8);

            Vector3[] intHit = new Vector3[hit.Length];

            for (int i = 0; i < intHit.Length; i++)
            {
                intHit[i].x = Mathf.Floor(hit[i].point.x);
                intHit[i].y = Mathf.Floor(hit[i].point.y);
                intHit[i].z = Mathf.Floor(hit[i].point.z);
            }
            bool success = true;

            foreach (var item in hit)
            {
                //Debug.Log("子弹在物体：" + item.collider.gameObject.name + "发生碰撞");
                var paintObject = item.transform.GetComponent<InkCanvas>();
                if (paintObject != null)
                {
                    var posX = Mathf.FloorToInt(gameObject.transform.position.x);
                    var posY = Mathf.FloorToInt(gameObject.transform.position.z);

                    if(Mapping.painted.ContainsKey(new Vector2(posX, posY)))
                    {
                        Mapping.painted[new Vector2(posX, posY)] = shellCurColor;
                    }
                    else
                    {
                        Mapping.painted.Add(new Vector2(posX, posY), character.curColor);
                        Debug.Log("dic:" + posX + "," + posY);
                    }

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
                    Destroy(gameObject);
                }
                if (!success)
                    Debug.LogError("Failed to paint.");
            }
        }

        void InitColor()
        {
            if (character.curColor == Character.chaColor.Blue)
            {
                shellCurColor = Character.chaColor.Blue;
                brush.Color = blue;
            }
            else
            {
                shellCurColor = Character.chaColor.Red;
                brush.Color = red;
            }
        }
    }
}
