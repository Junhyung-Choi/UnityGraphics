using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DVector
{
    public static DVector zero = new DVector(0,0,0);
    public double x,y,z;
    public DVector(double x, double y, double z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static DVector operator *(DVector vector, double value)
    {
        DVector newVector = new DVector(0,0,0);

        newVector.x = vector.x * value;
        newVector.y = vector.y * value;
        newVector.z = vector.z * value;
        
        return newVector;
    }

    public static DVector operator +(DVector vector1, DVector vector2)
    {
        DVector newVector = new DVector(0,0,0);

        newVector.x = vector1.x + vector2.x;
        newVector.y = vector1.y + vector2.y;
        newVector.z = vector1.z + vector2.z;
        
        return newVector;
    }

    public static implicit operator Vector3(DVector dvector)
    {
        Vector3 newVector = new Vector3();

        newVector.x = (float)dvector.x;
        newVector.y = (float)dvector.y;
        newVector.z = (float)dvector.z;

        return newVector;
    }

}
