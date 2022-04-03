/// <summary>
///     PlaySession is the only static class in Sentinels.
///     It stores information that needs to be shared between scenes.
/// </summary>
public static class PlaySession
{
    // Determines if the current gameplay is a practice or the real game.
    // The practice play is harder than the actual play, but with much more health points, allowing for a longer
    // gameplay.
    public static bool isPractice;
}