namespace snaresJ.script.utility;

public static class EasingFunctions {

    public static float QuadOut (float t)
    {
        // Equivalent to: return -t * (t - 2);
        return t * (2f - t);
    }

}