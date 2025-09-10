using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    private Dictionary<Guid, Coroutine> coroutineDict = new Dictionary<Guid, Coroutine>(100);

    public new Guid StartCoroutine(IEnumerator coroutineSource) {
        Guid coroutineID;
        do {
            coroutineID = Guid.NewGuid();
        } while (coroutineDict.ContainsKey(coroutineID));

        Coroutine coroutine = base.StartCoroutine(MemoryManageRoutine(coroutineSource, coroutineID));
        coroutineDict.Add(coroutineID, coroutine);
        return coroutineID;
    }
    public Guid Wait(float delay, Action callback) {
        return StartCoroutine(WaitRoutine(delay, callback));
    }
    public Guid WaitFrame(int frame, Action callback) {
        return StartCoroutine(WaitFrameRoutine(frame, callback));
    }
    public void Wait(Guid id, float delay, Action callback) {
        if (!coroutineDict.TryAdd(id, base.StartCoroutine(MemoryManageRoutine(WaitRoutine(delay, callback), id)))) {
            Debug.LogError($"coroutine ID 등록 실패");
        }
    }
    public Guid WaitReal(float delay, Action callback) {
        return StartCoroutine(WaitRealRoutine(delay, callback));
    }

    public void StopCoroutine(Guid coroutineID) {
        if (coroutineDict.TryGetValue(coroutineID, out var targetCoroutine)) {
            StopCoroutine(targetCoroutine);
            coroutineDict.Remove(coroutineID);
        }
    }

    public void ClearCoroutines() {
        StopAllCoroutines();
        coroutineDict.Clear();
    }

    private IEnumerator WaitRoutine(float delay, Action callback) {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }
    private IEnumerator WaitRealRoutine(float delay, Action callback) {
        yield return new WaitForSecondsRealtime(delay);
        callback?.Invoke();
    }
    private IEnumerator WaitFrameRoutine(int targetFrame, Action callback) {
        int currentFrame = 0;
        while (currentFrame < targetFrame) {
            yield return null;
            ++currentFrame;
        }
        callback?.Invoke();
    }
    private IEnumerator MemoryManageRoutine(IEnumerator mainRoutine, Guid routineID) {
        yield return mainRoutine;
        coroutineDict.Remove(routineID);
    }
}
