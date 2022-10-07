using System.Runtime.InteropServices;

namespace EnumDef
{
    public enum ANIMAL
    {
        CHICK = 0,
        CHICKEN,
        COW,
        CROCODILE,
        ELEPHANT,
        GORILLA,
        MONKEY,
        PANDA,
        PIG,
        RABBIT,
        SLOTH,
        ZEBRA
    }

    public enum INGAME_STATE
    {
        LOADING = 0,
        PLAYING,
        ENDING
    }

    public enum ROUNDTYPE
    {
        NONE,
        LONGJUMP,
    }

    public enum BLOCKTYPE
    {
        NONE,
        MOVEX,
        MOVEY,
        KEY,
        BALL,
    }
}