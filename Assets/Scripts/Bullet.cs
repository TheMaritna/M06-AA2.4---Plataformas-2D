using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum Direction
    {
        Right,
        Left,
        Up,
        Down
    }

    public Direction direction;
    public float speed = 10f;
    public float lifeTime = 5f;

    private Vector2 moveDir;

    void Start()
    {
        switch (direction)
        {
            case Direction.Right:
                moveDir = Vector2.right;
                break;

            case Direction.Left:
                moveDir = Vector2.left;
                break;

            case Direction.Up:
                moveDir = Vector2.up;
                break;

            case Direction.Down:
                moveDir = Vector2.down;
                break;
        }
        Destroy(this.gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(moveDir * speed * Time.deltaTime * PlayerTime.TIME);
    }
}