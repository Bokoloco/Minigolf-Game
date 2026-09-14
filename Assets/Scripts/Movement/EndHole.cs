using UnityEngine;

public class EndHole : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hello?");
        if (collision.gameObject.tag.Equals("Ball"))
        {
            BallMovement _ballMovementComponent = collision.gameObject.GetComponent<BallMovement>();
            if (_ballMovementComponent != null)
            {
                Debug.Log("Strokes: " + _ballMovementComponent.GetStrokeCount());
                //_ballMovementComponent.GetStrokeCount();
            }
        }
    }
}
