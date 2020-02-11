namespace DogHouse.ToonWorld.Animation
{
    /// <summary>
    /// IFade is an interface that is implemented
    /// by a component that in some way fades.
    /// </summary>
    public interface IFade
    {
        //Value from 0 to 1
        void SetFadeValue(float value);
    }
}
