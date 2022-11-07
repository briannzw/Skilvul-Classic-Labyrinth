using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameManager gameManager;
    public RemoteConfigFinder remoteConfigFinder;

    public GameObject ballPrefab;
    public Vector3 ballSpawnPoint = new Vector3(0, 1, 0);
    
    public GameObject startBlock;
    public GameObject straightBlock;
    public GameObject cornerBlock;
    public GameObject endBlock;
    public Vector3 endBlockOffset = new Vector3(0, .5f, 0);

    public int blockCount = 10;
    public int blockSize = 20;

    private Vector3Int nextBlockPoint;
    private Vector3Int currentDirection;

    private List<Vector3Int> blockPositions = new List<Vector3Int>();

    private void Awake()
    {
        SpawnBlock(startBlock, transform.position);
        GameObject ballObject = Instantiate(ballPrefab, ballSpawnPoint, Quaternion.identity);
        remoteConfigFinder.phoneGravity = ballObject.GetComponent<PhoneGravity>();
    }

    private void Start()
    {
        GameObject currentBlock = null;
        nextBlockPoint = Vector3Int.FloorToInt(transform.position) + Vector3Int.forward * blockSize;
        currentDirection = Vector3Int.forward;
        
        while(blockPositions.Count < blockCount)
        {
            int randomDirection = Random.Range(-1, 2);
            if (randomDirection == 0)
            {
                currentBlock = SpawnBlock(straightBlock, nextBlockPoint);
            }
            if(randomDirection == 1)
            {
                currentBlock = SpawnBlock(cornerBlock, nextBlockPoint);
                currentDirection = Vector3Int.RoundToInt(Quaternion.Euler(0, 90, 0) * currentDirection);
            }
            if(randomDirection == -1)
            {
                currentBlock = SpawnBlock(cornerBlock, nextBlockPoint);
                currentDirection = Vector3Int.RoundToInt(Quaternion.Euler(0, -90, 0) * currentDirection);
            }
            if (currentBlock == null)
            {
                currentDirection = Vector3Int.RoundToInt(Quaternion.Euler(0, -randomDirection * 90, 0) * currentDirection);
                continue;
            }

            nextBlockPoint += currentDirection * blockSize;
            currentBlock.transform.rotation = Quaternion.LookRotation(currentDirection, Vector3.up);

            if(randomDirection == -1)
                currentBlock.transform.rotation = Quaternion.Euler(0, -90, 0) * currentBlock.transform.rotation;
        }

        //nextBlockPoint = new Vector3(Mathf.Round(nextBlockPoint.x), Mathf.Round(nextBlockPoint.y), Mathf.Round(nextBlockPoint.z));
        GameObject holeObject = null;
        while (holeObject == null)
        {
            holeObject = SpawnBlock(endBlock, nextBlockPoint + endBlockOffset);
            if (!holeObject) continue;
            holeObject.transform.rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
            if(gameManager)
            gameManager.hole = holeObject.GetComponentInChildren<Hole>();
        }
    }

    private GameObject SpawnBlock(GameObject block, Vector3 pos)
    {
        Vector3Int posIndex = Vector3Int.RoundToInt(new Vector3(pos.x / blockSize, pos.y / 10, pos.z / blockSize));
        if (blockPositions.Contains(posIndex))
        {
            pos += new Vector3(0, 6, 0);
            
            GameObject upGO = Instantiate(straightBlock, pos, Quaternion.identity, transform);
            upGO.transform.rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
            upGO.transform.Rotate(-25, 0, 0, Space.Self);
            upGO.transform.localScale = new Vector3(1, 1, 1.5f);

            pos += new Vector3(0, 4, 0);

            posIndex = Vector3Int.RoundToInt(new Vector3(pos.x / blockSize, pos.y / 10, pos.z / blockSize));

            nextBlockPoint += new Vector3Int(0, 10, 0);
            nextBlockPoint += currentDirection * blockSize;

            blockPositions.Add(posIndex);
            // tidak boleh occupy di atas block miring
            //blockPositions.Add(posIndex + new Vector3Int(0, 1, 0));
            //blockPositions.Add(posIndex + new Vector3Int(1, 1, 0));
            //blockPositions.Add(posIndex + new Vector3Int(0, 1, 1));

            return null;
        }

        blockPositions.Add(posIndex);
        return Instantiate(block, pos, Quaternion.identity, transform);
    }
}
