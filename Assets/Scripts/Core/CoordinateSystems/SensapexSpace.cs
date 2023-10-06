using UnityEngine;

namespace CoordinateSpaces
{
    public sealed class SensapexSpace : CoordinateSpace
    {

        public override string Name => "Sensapex";
        public override Vector3 Dimensions => new(20, 20, 20);
        
        /// <summary>
        /// Convert coordinates from Sensapex space to Unity world space
        /// </summary>
        /// <param name="coord">X, Y, Z coordinate in Sensapex space</param>
        /// <returns>World space coordinates</returns>
        public override Vector3 Atlas2World(Vector3 coord)
        {
            return new Vector3(coord.y, -coord.z, -coord.x);
        }

        /// <summary>
        /// Convert coordinates from world space to Sensapex space
        /// </summary>
        /// <param name="world">X, Y, Z coordinates in World space</param>
        /// <returns>Sensapex Space coordinates</returns>
        public override Vector3 World2Atlas(Vector3 world)
        {
            return new Vector3(-world.z, world.x, -world.y);
        }

        public override Vector3 Atlas2World_Vector(Vector3 coord)
        {
            return Atlas2World(coord);
        }

        public override Vector3 World2Atlas_Vector(Vector3 world)
        {
            return World2Atlas(world);
        }
    }
}
