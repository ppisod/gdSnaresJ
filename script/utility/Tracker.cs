using System.Collections.Generic;

namespace snaresJ.script.utility;

public class Tracker <T> : List <T> {
    public int index = 0;

    public T GetCurrent ( bool cont ) {
        if (cont) {index++;return this[index - 1];}
        return this[index];
    }

    public List <T> GetNext ( int n ) {
        var indexBound = index + n;
        if (indexBound >= Count) indexBound = Count - 1;
        return GetRange ( index, indexBound - index + 1 );
    }

    public void Reset ( ) {index = 0;}

    public void IncrementIndex ( int n ) {
        
        index += n;

        if (index >= Count) index = Count - 1;

    }
}