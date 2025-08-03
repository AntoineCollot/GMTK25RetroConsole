using UnityEngine;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CodePointDatabase : MonoBehaviour
{
    [Header("CodePoints")]
    public List<CodePoint> codePoints;

    public static CodePointDatabase Instance;

    private void Awake()
    {
        Instance = this;
    }

    public bool HasCode(int code)
    {
        return codePoints.Any(c => c.code == code);
    }

    public Vector3 GetSpawnForCode(int code, out Theme areaTheme)
    {
        areaTheme = Theme.None;
        Vector3 spawnPos = GameGrid.GridToWorldPos(codePoints[0].gridPos);
        if (HasCode(code))
        {
            CodePoint selectedPoint = codePoints.First(c => c.code == code);
            spawnPos = GameGrid.GridToWorldPos(selectedPoint.gridPos);
            areaTheme = selectedPoint.areaTheme;
        }
        return spawnPos;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (codePoints == null)
            return;

        Gizmos.color = Color.blue;
        int id = 0;
        foreach(CodePoint p in codePoints)
        {
            Vector3 pos = GameGrid.GridToWorldPos(p.gridPos);
            Gizmos.DrawSphere(pos, 0.3f);
            Handles.Label(pos, "CodePoint "+id.ToString());
            id++;
        }
    }
#endif
}

[System.Serializable]
public struct CodePoint
{
    public int code;
    public Vector2Int gridPos;
    public Theme areaTheme;
}