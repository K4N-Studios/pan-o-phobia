using System.Collections;

public class CoroutineRunner : Singleton<CoroutineRunner>
{
    public void RunCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
