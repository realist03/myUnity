using UnityEngine;

namespace Es.InkPainter.Sample
{

    [RequireComponent(typeof(Collider), typeof(MeshRenderer))]
    public class CollisionPost : MonoBehaviour
    {
        public Character.chaColor shellCurColor = Character.chaColor.None;
        public Character character;

        public Color red;
        public Color blue;

        [SerializeField]
        private Brush brush = null;

        public void Awake()
        {
            InitColor();
            GetComponent<MeshRenderer>().material.color = brush.Color;
        }

        public void OnCollisionEnter(Collision collision)
        {
            var canvas = collision.transform.GetComponent<InkCanvas>();

            if (canvas != null)
            {
                var posX = Mathf.FloorToInt(gameObject.transform.position.x);
                var posY = Mathf.FloorToInt(gameObject.transform.position.y);
                var posZ = Mathf.FloorToInt(gameObject.transform.position.z);

                if (Mapping.painted.ContainsKey(new Vector2(posX, posZ)))
                {
                    Mapping.painted[new Vector2(posX, posZ)] = shellCurColor;
                }
                else
                {
                    Mapping.painted.Add(new Vector2(posX, posZ), character.curColor);
                    //Debug.Log("dic:" + posX + "," + posZ);
                }
                canvas.Paint(brush, new Vector3(posX+0.5f,posY,posZ+0.5f));
            }
            Destroy(gameObject);
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