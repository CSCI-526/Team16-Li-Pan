using UnityEngine;
using TMPro;

public class EnemyCountUI : MonoBehaviour
{
    [SerializeField] private TMP_Text enemyCountText;

    private void Update()
    {
        int count = GameObject.FindGameObjectsWithTag("Enemy").Length;

        enemyCountText.text = "Enemies: " + count;
    }
}