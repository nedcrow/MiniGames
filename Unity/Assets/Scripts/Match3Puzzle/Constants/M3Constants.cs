public enum E3MTileType
{
    None = -1,
    Apple = 0,
    Banana = 1,
    Grape = 2,
    Orange = 3,
    S1 = 4,
    S2 = 5,
    S3 = 6,
    S4 = 7,
    S5 = 8,
    S6 = 9,
}

public enum EGameMode
{
    Eidt = 0,
    Play_Normal = 1,
    Play_Hard = 2,
}

public enum ECapsuleType
{
    Box = 0,
    Bubble = 1,
    Candle = 2,
    Snow = 3,
}

/// <summary>
/// <para> Wait: Select or drag tile </para>
/// <para> Match: Check matching and memory bomb list </para>
/// <para> Effect: Add effect target to bomb list and bomb </para>
/// <para> Spawn: spawn and place new tiles </para>
/// </summary>
public enum EPuzzleState
{
    Wait = 0,
    Match = 1,
    Effect = 2,
    Spawn = 3,
}

public enum EMatchType
{
    None = 0,
    Normal = 1,
    Arrow_V = 2,
    Arrow_H = 3,
    Box = 4,
    Five = 5,
    Straight = 6,
}

/// <summary>
/// normal gravity direction is down.
/// </summary>
public enum EGravity
{
    Up = 0,
    Down = 1,
    Right = 2,
    Left = 3,
}