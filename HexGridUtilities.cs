using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

// TODO add function that returns neighbors:
    // for both 1 location and an array of locations

// For Cube Coordinates (q,r,s) a top-point hexagon grid
// as in https://www.redblobgames.com/grids/hexagons/
public static class HexGridUtilities
{
// fields
    public static Dictionary<(int lower, int upper), (int q, int r, int s)[]> hexGridDistanceLocs;
    private static int distanceLocBoundsLower = 0, distanceLocBoundsUpper = 5;
    public static void initializeHexGridDistanceLocs() {
        // fill hexGridDistanceLocs
        // lower from 0 to n
        // upper from 0 to m
        // store intermediate values as well

        hexGridDistanceLocs.Clear();

        for(int lower = 0; lower <= distanceLocBoundsLower; lower++) {
            for(int upper = lower; upper <= distanceLocBoundsUpper; upper++) {
                // this should work but if it doesn't go back to .Add with if(key in dict.Keys) 
                hexGridDistanceLocs.Add((lower,upper), hexLocationsInDistanceRange(lower,upper));
            }
        }
    }

// getters
    public static int getLower() { return distanceLocBoundsLower; }
    public static int getUpper() { return distanceLocBoundsUpper; }
// public methods
    public static (int q, int r, int s)[] hexLocationsInDistanceRange(int lower, int upper) {
        if(lower > upper) {
            throw new System.ArgumentException("upper bound must be >= lower bound");
        }

        List<(int q, int r, int s)> values = new List<(int q, int r, int s)>();
        // possible optimization:
        // rather than checking for lower on line (*), change the for loops to check before
        // that is, we currently allow all q,r,s in [-upper, upper] (ish) but only check
        // after this that (q,r,s) is such that the max of their absolute values >= lower
            // ex: this excludes the row {(1,-1,0), (1,0,-1)...} when lower = 2  
        int s;
        int max;
        for(int q = -upper; q <= upper; q++) {
            for(int r = math.max(-upper, -upper-q); r <= math.min(upper, upper-q); r++) {
                s = -q-r;    
                max = math.max(math.abs(q), math.max(math.abs(r), math.abs(s)));
                if(lower <=  max && max <= upper) { values.Add((q,r,s)); } 
            }
        }
        return values.ToArray();
    }
    public static void setDistanceLocBounds(int lower, int upper, bool refresh = false) {
        distanceLocBoundsLower = lower;
        distanceLocBoundsUpper = upper;
        if(refresh) { initializeHexGridDistanceLocs(); }
    }
    public static (int q, int r, int s)[] getHexGridDistanceLocs(int lower, int upper) {
        if(hexGridDistanceLocs.TryGetValue((lower,upper), out (int q, int r, int s)[] locsArray)) {
            return locsArray; 
        } else {
            throw new System.ArgumentException("Key ("+ lower + "," + upper + ") not found. Current maximums: ("+distanceLocBoundsLower+","+distanceLocBoundsUpper+")");
        }
    }
// conversions
    public static (int q, int r, int s) XYtoQRS(Vector3Int loc) {
        (int q, int r, int s) newLoc;
        // step 1: convert (x,y) using (0,y) first
        // step 2: then find (x,y) by translating along (-,.,+)--(+,.,-) axis i.e add (x,0,-x)
        newLoc.q = (loc.y + math.abs(loc.y % 2))/2 + loc.x;
        newLoc.r = -loc.y;
        newLoc.s = (loc.y - math.abs(loc.y % 2))/2 - loc.x;

        return newLoc;
    }
    public static Vector3Int QRStoXY((int q, int r, int s) loc) { 
        Vector3Int newLoc = new Vector3Int();
        newLoc.y = -loc.r;
        newLoc.x = loc.q - (-loc.r + math.abs(loc.r % 2))/2;
               
        newLoc.z = 0;
        return newLoc;
    }
}
