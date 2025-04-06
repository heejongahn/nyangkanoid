using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnScript : MonoBehaviour
{
    private bool isGameStarted = false;

    public GameObject blocks;
    public GameObject block;
    public float spawnRate = 2;
    public float heightOffset = 0;

    private float timer = 0;


    // Start is called before the first frame update
    void Start()
    {
        GameEventsScript.Instance.OnChangeIsGameStarted.AddListener(HandleGameStart);
    }

    void OnDestroy()
    {
        GameEventsScript.Instance.OnChangeIsGameStarted.RemoveListener(HandleGameStart);
    }


    // Update is called once per frame
    void Update()
    {
        if (!isGameStarted)
        {
            return;
        }

        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
            return;
        }

        SpawnBlock();
        timer = 0;
    }

    void HandleGameStart(bool _isGameStarted)
    {
        isGameStarted = _isGameStarted;
    }

    void SpawnBlock()
    {

        var createdBlock = Instantiate(
            block,
            new Vector3(
                transform.position.x + Random.Range(-2, 2),
                5,
                0
            ),
            transform.rotation,
            blocks.transform
        );

        var blockScript = createdBlock.GetComponent<BlockScript>();
        blockScript.HandleGameStart(true);
    }
}
