namespace QuantumAlgorithms.Models.Get
{
    public class IntegerFactorizationInputDto
    {
        public int Number { get; set; }

        public static IntegerFactorizationInputDto Create(int number) => new IntegerFactorizationInputDto { Number = number };
    }
}
