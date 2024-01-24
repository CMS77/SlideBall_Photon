using UnityEngine;

public class GoalScript : MonoBehaviour
{
    public GameManager gameManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SoccerBall"))
        {
            int scoringPlayer = (gameObject.CompareTag("Goal1")) ? 2 : 1;
            gameManager.GoalScored(scoringPlayer);
        }
    }
}
