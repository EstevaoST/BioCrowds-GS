using Biocrowds.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    private Collider _collider;
    private MeshRenderer _meshRenderer;

    [Header("Initial Spawner Settings")]
    public int initialNumberOfAgents;
    public bool initialRemoveWhenGoalReached;
    public List<GameObject> initialAgentsGoalList;
    public List<float> initialWaitList;

    [Header("Repeating Spawner Settings")]
    public bool limitRepeatingSpawn = false;
    public int quantityLimitToSpawn = 1;
    public float cycleLenght = 1.0f;    
    public int quantitySpawnedEachCycle;
    public bool repeatingRemoveWhenGoalReached;
    public List<GameObject> repeatingGoalList;
    public List<float> repeatingWaitList;
    private float cycleCounter = 0.0f;
    private bool cycleReady = false;

    [Header("Entering Spawner Settings")]
    public bool setGoalOnEnter = false;
    public bool teleportToGoalOnEnter = false;
    public List<GameObject> enteringGoalList;
    public List<Agent> teleportBuffer;

    public bool CycleReady { get => cycleReady;  }
    public bool Finished => (limitRepeatingSpawn && quantityLimitToSpawn <= 0) || quantitySpawnedEachCycle == 0;
    public Collider Collider => _collider;

    private void Awake()
    {
        if (_collider == null)
            _collider = GetComponent<Collider>();
        if (_meshRenderer == null)
            _meshRenderer = GetComponent<MeshRenderer>();

        cycleCounter = 0.0f;
        cycleReady = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Debug.Log(GetRandomPoint());
    }

    public void UpdateSpawnCounter(float dt)
    {
        if (cycleLenght == 0.0f || quantitySpawnedEachCycle == 0 || (limitRepeatingSpawn && quantityLimitToSpawn == 0))
            return;

        cycleCounter += dt;
        if (cycleCounter >= cycleLenght)
        {
            cycleCounter -= cycleLenght;
            cycleReady = true;
        }
    }

    public void ResetCycleReady()
    {
        cycleReady = false;
    }

    public Vector3 GetRandomPoint(float height = 0.0f)
    {
        Vector3 point = new Vector3(Random.Range(0.0f,1.0f), 1, Random.Range(0.0f, 1.0f));
        Vector3 min, max;
        if (_collider.enabled)
        {
            min = _collider.bounds.min;
            max = _collider.bounds.max;
        }
        else
        {
            min = new Vector3(-transform.lossyScale.x, 0     , -transform.lossyScale.z) + transform.position;
            max = new Vector3( transform.lossyScale.x, height,  transform.lossyScale.z) + transform.position;
        }
        point = new Vector3(Mathf.Lerp(min.x, max.x, point.x), Mathf.Lerp(min.y, max.y, point.y), Mathf.Lerp(min.z, max.z, point.z));
        point = transform.TransformDirection(point);
        return point;
    }
    public bool IsInsideArea(Vector3 point, float height = 0.0f)
    {
        Bounds bounds;
        if (_collider.enabled)
            bounds = _collider.bounds;            
        else
            bounds = new Bounds(transform.position, transform.lossyScale);

        return bounds.Contains(point);
    }
    public void ShowMesh(bool _show)
    {
        _meshRenderer.enabled = _show;
    }

    public void AgentEntered(Agent agent)
    {
        if (setGoalOnEnter)
        {
            agent.goalsList.Clear();
            agent.goalsList.AddRange(enteringGoalList);
            agent.goalIndex = 0;
        }

        if (teleportToGoalOnEnter)
        {
            teleportBuffer.Add(agent);
        }
    }
}
