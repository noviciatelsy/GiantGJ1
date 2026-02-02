using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class InteractableScanner : MonoBehaviour
{
    [Header("Scan Settings")]
    [SerializeField] private float scanRadius = 2.0f;                          // 扫描半径
    [SerializeField] private float scanInterval = 0.2f;                        // 扫描间隔（秒），比如 0.2 就是每 0.2 秒扫一次
    [SerializeField] private LayerMask scanMask = ~0;                          // 扫描层（建议只勾可交互层）
    [SerializeField] private QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore;

    [Header("Origin Offset")]
    [SerializeField] private Vector3 originOffset = new Vector3(0f, 1.0f, 0f); // 球心偏移（比如胸口高度）

    [Header("Performance")]
    [SerializeField] private int overlapBufferSize = 32;                       // OverlapSphereNonAlloc 缓冲大小

    public IInteractable currentInteractableObject
    {
        get;
        private set;
    }

    private Collider[] _overlapBuffer;

    // 复用容器：避免 GC
    private readonly List<MonoBehaviour> _monoBuffer = new List<MonoBehaviour>(8);

    // 计时器
    private float _scanTimer = 0f;

    private void Awake()
    {
        overlapBufferSize = Mathf.Max(1, overlapBufferSize);
        _overlapBuffer = new Collider[overlapBufferSize];
    }

    private void OnDisable()
    {
        currentInteractableObject = null;
        _scanTimer = 0f;
    }

    private void Update()
    {
        // 防止填 0 或负数导致疯狂扫描
        float interval = Mathf.Max(0.02f, scanInterval);

        _scanTimer += Time.deltaTime;

        if (_scanTimer < interval)
        {
            return;
        }

        // 到点了：扫一次
        _scanTimer = 0f;
        ScanOnce();
    }

    private void ScanOnce()
    {
        Vector3 originPos = transform.position + originOffset;

        int hitCount = Physics.OverlapSphereNonAlloc(
            originPos,
            scanRadius,
            _overlapBuffer,
            scanMask,
            triggerInteraction
        );

        IInteractable bestInteractable = null;
        float bestSqrDistance = float.PositiveInfinity;

        for (int i = 0; i < hitCount; i++)
        {
            Collider col = _overlapBuffer[i];

            if (col == null)
            {
                continue;
            }

            // 排除自己（角色自身的碰撞体）
            if (col.transform == transform || col.transform.IsChildOf(transform))
            {
                continue;
            }

            // 找到实现 IInteractable 的组件（允许脚本挂在父物体）
            if (!TryGetInteractableFromCollider(col, out IInteractable interactable))
            {
                continue;
            }

            // 用碰撞体表面最近点计算距离（更符合“贴近”的手感）
            Vector3 closestPoint = col.ClosestPoint(originPos);
            float sqrDist = (closestPoint - originPos).sqrMagnitude;

            if (sqrDist < bestSqrDistance)
            {
                bestSqrDistance = sqrDist;
                bestInteractable = interactable;
            }
        }

        currentInteractableObject = bestInteractable;
    }

    private bool TryGetInteractableFromCollider(Collider col, out IInteractable interactable)
    {
        interactable = null;

        _monoBuffer.Clear();

        // 用 MonoBehaviour 作中间列表，再用 is IInteractable 判断（版本兼容更稳）
        col.GetComponentsInParent(true, _monoBuffer);

        for (int i = 0; i < _monoBuffer.Count; i++)
        {
            MonoBehaviour mb = _monoBuffer[i];

            if (mb == null)
            {
                continue;
            }

            if (mb is IInteractable found)
            {
                interactable = found;
                return true;
            }
        }

        return false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 originPos = transform.position + originOffset;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(originPos, scanRadius);
    }
#endif
}

public interface IInteractable
{
    public void Interact();
}
