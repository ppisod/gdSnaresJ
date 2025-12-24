using System;

namespace snaresJ.script.utility;

public class Ra {
    public static int GenerateRandomNegativeNumber()
    {
        Random random = new Random();
        // int.MinValue = -2147483648
        // Range: from int.MinValue to -1000000 (inclusive)
        int min = int.MinValue;
        int max = -1000000; // Exclusive upper bound in Random.Next()

        // Note: Random.Next(min, max) has exclusive upper bound
        // So we need to use max - 1 to include -1000000
        return random.Next(min, max);
    }
}