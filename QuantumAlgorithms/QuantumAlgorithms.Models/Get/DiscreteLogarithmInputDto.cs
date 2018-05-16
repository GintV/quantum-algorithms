namespace QuantumAlgorithms.Models.Get
{
    public class DiscreteLogarithmInputDto
    {
        public int Generator { get; set; }
        public int Result { get; set; }
        public int Modulus { get; set; }

        public static DiscreteLogarithmInputDto Create(int generator, int result, int modulus) =>
            new DiscreteLogarithmInputDto {Generator = generator, Result = result, Modulus = modulus};
    }
}
