namespace GunsNGas
{
    public interface IController
    {
        bool IsSpeederPressed { get; }
        bool AreBrakesPressed { get; }
        bool IsTurningRight{ get; }
        bool IsTurningLeft { get; }
        bool IsFiring { get; }
        bool IsDroppingMine { get; }
        bool IsFiringNitro { get; }
    }
}