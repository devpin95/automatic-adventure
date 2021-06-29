using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System.Linq;

public class SpawnManager : MonoBehaviour
{
    public LevelData levelData;
    public Devices _devices;
    public GameData _gameData;
    public CEvent_Int waveStartedEvent;
    public CEvent_Int stintStartedEvent;
    [Header("Spawn Area Attributes")] public Vector3 spawnCenter;
    [FormerlySerializedAs("spawnAreaMajorWidth")] public float spawnGridColumns;
    [FormerlySerializedAs("spawnAreaMinorWidth")] public float spawnGridRows;

    public EnemyAttributes ballLauncherAttributes;

    public LayerMask enemyLayer;
    
    // private int numEnemies = 0;

    private float _waitTimeDuration = 5.0f;
    private float _waitTime = 0.0f;

    private int wave = 0;
    private int enemiesLeftInWave = 0;
    private bool _waveOver = false;
    private bool _stintOver = false;
    private bool _firstWaveBack = true;

    private bool _hasDeferredWaves = false;
    private int _currentDeferredWave = 0;
    private int _deferredWaveCount = 0;
    private Level _currentLevel;
    private List<List<WaveGroup>> _deferredWaves = new List<List<WaveGroup>>();

    private bool[,] _grid;
    private int _gridCols;
    private int _gridRows;
    private int _gridThreshold = 100;
    private Vector2 _nextOpenSlot;
    private bool _gridInitialized = false;
    [SerializeField] private Vector2 highlightGridBlock = Vector2.zero;

    private PoolManager _poolManager;

    public CEvent stintOverEvent;

    private bool _playerDeviceReady = false;

    // Start is called before the first frame update
    void Start()
    {
        _playerDeviceReady = _devices.IsReady;
        wave = _gameData.Wave;
        _waveOver = true;
        _poolManager = GameObject.Find("PoolManager").GetComponent<PoolManager>();

        _gridCols = (int) spawnGridColumns * 2;
        _gridRows = (int) spawnGridRows * 2;
        _grid = new bool[_gridRows, _gridCols];
        ResetGrid();
        _nextOpenSlot = Vector2.zero;
        
        stintStartedEvent.Raise(wave / 10); // int division
    }

    // Update is called once per frame
    void Update()
    {
        if (_stintOver) return;

        if (_hasDeferredWaves)
        {
            Debug.Log("Checking spawn area...");
            if (!EnemiesInSpawnArea())
            {
                Debug.Log("No more enemies in the spawn area, spawning deferred wave " + _currentDeferredWave);
                // we need to spawn the next deferred wave
                foreach (var group in _currentLevel.waveGroups.Where(t => t.deferredWave == _currentDeferredWave))
                {
                    SpawnGroup(group);
                }

                ++_currentDeferredWave;

                if (_currentDeferredWave >= _deferredWaveCount)
                {
                    _hasDeferredWaves = false;
                    _currentDeferredWave = 0;
                    _deferredWaveCount = 0;
                    _currentLevel = null;
                    _deferredWaves = new List<List<WaveGroup>>();
                }
            }
        }

        if (_waveOver)
        {
            _waitTime += Time.deltaTime;

            if (_waitTime > _waitTimeDuration)
            {
                if (wave != 0 && wave % 10 == 0 && !_firstWaveBack)
                {
                    // we need to go to the upgrade scene
                    _stintOver = true;
                    stintOverEvent.Raise();
                }
                else
                {
                    if (_gridInitialized)
                    {
                        // _grid = new bool[_gridRows, _gridCols];
                        ResetGrid();
                    }

                    _hasDeferredWaves = false;
                    _currentDeferredWave = 0;
                    _deferredWaveCount = 0;
                    _currentLevel = null;
                    _deferredWaves = new List<List<WaveGroup>>();
                    
                    SpawnEnemies();
                }

                _waitTime = 0.0f;
            }
        }
    }

    private void SpawnEnemies()
    {
        enemiesLeftInWave = 0;
        _firstWaveBack = false;

        if (wave >= levelData.levels.Length)
        {
            Debug.Log("No more waves");
            return;
        }

        _currentLevel = levelData.levels[wave];

        var deferredWaveGroups = _currentLevel.waveGroups.GroupBy(t => t.deferredWave);

        foreach (var thing in deferredWaveGroups)
        {
            _deferredWaves.Add(thing.ToList());
        }

        if (_deferredWaves.Count > 1)
        {
            Debug.Log("This level has " + _deferredWaves.Count + " deferred waves.");
            _hasDeferredWaves = true;
            _currentDeferredWave = 1;
            _deferredWaveCount = _deferredWaves.Count;
        }
        

        // int count = 0;
        //
        // foreach (var group in _currentLevel.waveGroups)
        // {
        //     count += group.count;
        // }

        Debug.Log("Spawning deferred wave 0");
        foreach (var group in _currentLevel.waveGroups.Where(t => t.deferredWave == 0))
        {
            SpawnGroup(group);
        }

        enemiesLeftInWave = GetTotalEnemiesInWave(_currentLevel);
        
        Debug.Log("There is a total of " + enemiesLeftInWave + " enemies across the " + _deferredWaveCount + " deferred waves");
        
        _waveOver = false;
        waveStartedEvent.Raise(wave);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(spawnCenter, new Vector3(spawnGridColumns * 2, 1, spawnGridRows * 2));
        Vector3 center = new Vector3(ballLauncherAttributes.fixedPosition.x, 0, ballLauncherAttributes.fixedPosition.y);
        Vector3 size = new Vector3(ballLauncherAttributes.fixedPosition.z, 1, ballLauncherAttributes.fixedPosition.w);
        Gizmos.DrawCube(center, size);

        if (!_gridInitialized) return;

        int rows = (int) spawnGridRows * 2; // slots in the z direction
        int cols = (int) spawnGridColumns * 2; // slots in the x direction
        
        for (int row = 0; row < rows; ++row)
        {
            for (int col = 0; col < cols; ++col)
            {
                float centerX = col + 0.5f;
                float centerZ = row + 0.5f;
                float trueCenterX = centerX + spawnCenter.x - spawnGridColumns;
                float trueCenterZ = centerZ + spawnCenter.z - spawnGridRows;
                Vector3 scenter = new Vector3(trueCenterX, 0, trueCenterZ);

                if (row == highlightGridBlock.x && col == highlightGridBlock.y)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawCube(scenter, new Vector3(1, 1, 1));
                }
                else if (_grid[row, col])
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(scenter, new Vector3(1, 1, 1));
                }
                else
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireCube(scenter, new Vector3(1, 1, 1));
                }
            }
        }
    }

    public void OnDeviceConnectedResponse()
    {
        _playerDeviceReady = true;
    }

    private Vector3 RandomPosition()
    {
        float randXpos =
            UnityEngine.Random.Range(spawnCenter.x - spawnGridColumns, spawnCenter.x + spawnGridColumns);
        float randZpos =
            UnityEngine.Random.Range(spawnCenter.z - spawnGridColumns, spawnCenter.z + spawnGridColumns);
        return new Vector3(randXpos, 1, randZpos);
    }

    private Vector3 RandomGridPosition(Vector2 size)
    {
        int randXpos, randZpos;
        Vector3 spawnPos = Vector3.zero;
        do
        {
            randXpos =
                (int) UnityEngine.Random.Range(0,
                    _gridRows - Mathf.Floor(size.x / 2));
            randZpos =
                (int) UnityEngine.Random.Range(0, _gridCols - Mathf.Floor(size.y / 2));
            
            // Debug.Log("x[0, " + (_gridRows - Mathf.Floor(size.x / 2)) + "], z[0, " + (_gridCols - Mathf.Floor(size.y / 2)) + "]. Testing " + randXpos + ", " + randZpos);
        } while (!EnemyFits(randXpos, randZpos, size));

        MarkGridSlots(randXpos, randZpos, size);
        spawnPos = SpawnEnemyAt(randXpos, randZpos, size);

        return spawnPos;
    }

    public void EnemyKilledHandler(int val, bool notAFixedEnemy)
    {
        if (!notAFixedEnemy) return;
        
        --enemiesLeftInWave;
        if (enemiesLeftInWave <= 0 && !_waveOver)
        {
            Debug.Log("No More enemies in wave " + wave + ". Moving on to wave " + (wave + 1));
            _waveOver = true;
            ++wave;
            _gameData.Wave = wave;
            print("Wave Over");
        }
    }

    private Vector3 GridPosition(Vector2 size)
    {
        Vector3 spawnPos = Vector3.zero;
        bool fits = false;

        Debug.Log("Starting search at (" + _nextOpenSlot.x + ", " + _nextOpenSlot.y + ")");
        
        for (int row = (int) _nextOpenSlot.x; row < _gridRows; ++row)
        {
            for ( int col = (int) _nextOpenSlot.y; col < _gridCols; ++col )
            {
                if (_grid[row, col] == false && col + (size.y-1) < _gridCols && row + (size.x-1) < _gridRows)
                {
                    Debug.Log("Checking if enemy fits at (" + row + ", " + col + ") with size (" + size.x + ", " + size.y + ")");
                    // there is an empty spot here
                    fits = EnemyFits(row, col, size);
                    
                    if (fits)
                    {
                        // we can spawn the enemy at [col, row]
                        // mark those slots as filled
                        MarkGridSlots(row, col, size);

                        // spawn the enemy in the center of those slots
                        spawnPos = SpawnEnemyAt(row, col, size);
                        break;
                    }
                }
            }
            if (fits) break;
        }

        return spawnPos;
    }

    private void SpawnGroup(WaveGroup group)
    {
        ResetGrid();
        
        float centerX = 0, centerZ = 0, areaHeight = 0, areaWidth = 0, blockWidth = 0, spacing = 0;

        if (group.enemy.isFixed)
        {
            centerX = group.enemy.fixedPosition.x;
            centerZ = group.enemy.fixedPosition.y;
            areaWidth = group.enemy.fixedPosition.z;
            blockWidth = areaWidth / group.count; // the width of a block in the spawn area
            spacing = blockWidth / 2; // half the size of a block
        }
        
        // Debug.Log("Looking for " + group.count + " open slots in the grid");
        for (int i = 0; i < group.count; ++i)
        {
            Quaternion rotation = group.enemy.Prefab.transform.rotation;
            Quaternion modRotation = Quaternion.Euler(group.enemy.rotation);

            if (group.enemy.randomRotation) rotation = UnityEngine.Random.rotation;
            
            Quaternion spawnRotation = rotation * modRotation;

            ObjectPool pool = group.enemy.pool;
            GameObject enemy = pool.GetPooledObject();

            if (enemy)
            {
                if (group.enemy.isFixed)
                {
                    // this enemy has a fixed spawn area
                    // find x center by doing calc starting at x = 0 in 1D
                    float currentBlock = i * blockWidth;
                    float blockXCenter = currentBlock + spacing;
                    float trueXCenter = blockXCenter + centerX - ( areaWidth / 2 );

                    enemy.transform.position = new Vector3(trueXCenter, 1, centerZ);
                }
                else
                {
                    //this enemy can spawn anywhere in the main spawn area
                    // enemy.transform.position = RandomPosition();
                    enemy.transform.position = RandomGridPosition(group.enemy.spawnGridDimensions);
                }

                enemy.transform.rotation = spawnRotation;
                enemy.SetActive(true);
                // Debug.Break();
                
                // if ( group.enemy.countAsEnemy ) enemiesLeftInWave += 1;
            }
        }
    }
    
    private bool EnemyFits(int row, int col, Vector2 size)
    {
        if (col + size.x >= _gridCols || row + size.y >= _gridRows) return false;
        
        bool fits = true;
                    
        // check if all the spots the enemy will take up are empty
        // loop through the rows
        for (int checkRow = row; checkRow < row + size.y; ++checkRow)
        {
            // loop through the columns
            for (int checkCol = col; checkCol < col + size.x; ++checkCol)
            {
                // Debug.Log("Checking (" + row + ", " + col + ")");
                // if the spot is not empty, the enemy will not fit here
                if (_grid[checkRow, checkCol] == true)
                {
                    // get out of the loop
                    fits = false;
                    break;
                }
            }
                        
            // if there are not enough open columns here, we need to move on to the next open spot
            // TODO can we optimize this to move where we look next based on where we found a non empty spot?
            if (!fits) break;
        }

        return fits;
    }

    private Vector3 SpawnEnemyAt(int row, int col, Vector2 size)
    {
        // Debug.Log("Spawning at " + row + ", " + col);
        Vector3 spawnPos = Vector3.zero;
        spawnPos.y = 1;
        
        float centerX = col + (size.x / 2);
        float centerZ = row + (size.y / 2);
        float trueCenterX = centerX + spawnCenter.x - spawnGridColumns;
        float trueCenterZ = centerZ + spawnCenter.z - spawnGridRows;

        spawnPos.x = trueCenterX;
        spawnPos.z = trueCenterZ;

        return spawnPos;
    }

    private void MarkGridSlots(int startRow, int StartCol, Vector2 size)
    {
        // mark all the spots the enemy is going to take up
        for ( int row = startRow; row < startRow + size.y; ++row) 
        {
            for (int col = StartCol; col < StartCol + size.x; ++col)
            {
                _grid[row, col] = true;
            }
        }

        // find the next slot that an enemy is not already using
        bool nextOpenSlotFound = false;
        for (int row = (int) _nextOpenSlot.x; row < _gridRows; ++row)
        {
            for (int col = 0; col < _gridCols; ++col)
            {
                if (_grid[row, col] == false)
                {
                    _nextOpenSlot.x = row;
                    _nextOpenSlot.y = col;
                    nextOpenSlotFound = true;
                    break;
                }
            }
            if (nextOpenSlotFound) break;
        }
    }

    private void ResetGrid()
    {
        for (int i = 0; i < _gridRows; ++i)
        {
            for (int j = 0; j < _gridCols; ++j)
            {
                _grid[i, j] = false;
            }
        }

        _gridInitialized = true;
        _nextOpenSlot.x = 0;
        _nextOpenSlot.y = 0;
    }

    private bool EnemiesInSpawnArea()
    {
        Vector3 bounds = new Vector3(spawnGridColumns, 0.5f, spawnGridRows);
        return Physics.OverlapBox(spawnCenter, bounds, Quaternion.identity, enemyLayer).Length > 0;
    }

    private int GetTotalEnemiesInWave(Level level)
    {
        int count = 0;

        foreach (var group in level.waveGroups)
        {
            if (group.enemy.countAsEnemy)
            {
                count += group.count;
            }
        }

        return count;
    }
}
