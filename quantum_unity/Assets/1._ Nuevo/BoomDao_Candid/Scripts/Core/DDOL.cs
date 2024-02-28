
public class DDOL : Singleton<DDOL>
{
    protected override void _Awake()
    {
        DontDestroyOnLoad(this);
    }

    protected override void _OnDestroy()
    {

    }
}