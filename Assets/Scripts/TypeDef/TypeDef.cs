using System.Runtime.InteropServices;

namespace EnumDef
{
    // 맵마다 특성이 필요한경우,
    public enum ROUNDTYPE
    {
        NONE,
        LONGJUMP,
    }

    public enum STATE
    {
        LOADING = 0,
        PLAYING,
        ENDING
    }

    public enum BLOCKTYPE
    {
        NONE,
        MOVEX,
        MOVEY,
        DOOR,
        KEY,
    }
}


