namespace QuantumAlgorithms.Models.Get
{
    public class DiscreteLogarithmOutputDto
    {
        public int DiscreteLogarithm { get; set; }

        public static DiscreteLogarithmOutputDto Create(int discreteLogarithm) =>
            new DiscreteLogarithmOutputDto {DiscreteLogarithm = discreteLogarithm};
    }
}
