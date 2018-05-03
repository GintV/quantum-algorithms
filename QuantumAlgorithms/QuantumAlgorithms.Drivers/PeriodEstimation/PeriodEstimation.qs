namespace QuantumAlgorithms.Drivers.PeriodEstimation
{
    open Microsoft.Quantum.Primitive;
    open Microsoft.Quantum.Extensions.Math;
    open Microsoft.Quantum.Extensions.Convert;
    open Microsoft.Quantum.Canon;

    operation PeriodEstimation (generator : Int, modulus : Int) : Int
    {
        body
        {
            // Here we check that the inputs to the EstimatePeriod operation are valid.
            AssertBoolEqual(IsCoprime(generator,modulus), true, "`generator` and `modulus` must be co-prime" );

            // The variable that stores the divisor of the generator period found so far.
            mutable result = 1;

            // Number of bits in the modulus with respect to which we are estimating the period.
            let bitsize = BitSize( modulus );

            // The EstimatePeriod operation estimates the period r by finding an 
            // approximation k/2^bitsPrecision to a fraction s/r where s is some integer.
            // Note that if s and r have common divisors we will end up recovering a divisor of r
            // and not r itself. However, if we recover big enough number of divisors of r
            // we recover r itself pretty soon.

            // Number of bits of precision with which we need to estimate s/r to recover period r.
            // using continued fractions algorithm. 
            let bitsPrecision = 2*bitsize + 1;

            repeat {
                // The variable that stores numerator of dyadic fraction k/2^bitsPrecision 
                // approximating s/r
                mutable dyadicFractionNum = 0;

                // Allocate qubits for the superposition of eigenstates of 
                // the oracle that is used in period finding
                using(eignestateRegister = Qubit[bitsize]) {

                    // Initialize eignestateRegister to 1 which is a superposition of 
                    // the eigenstates we are estimating the phases of. 
                    // We first interpret the register as encoding unsigned integer
                    // in little endian encoding.
                    let eignestateRegisterLE = LittleEndian(eignestateRegister);
                    InPlaceXorLE(1,eignestateRegisterLE);

                    // An oracle of type Microsoft.Quantum.Canon.DiscreteOracle 
                    // that we are going to use with phase estimation methods below.
                    let oracle = DiscreteOracle(OrderFindingOracle(generator, modulus, _, _));

                    // Find the numerator of a dyadic fraction that approximates 
                    // s/r where r is the multiplicative order ( period ) of g

                    // Use Microsoft.Quantum.Canon.QuantumPhaseEstimation to estimate s/r.
                    // When using QuantumPhaseEstimation we will need extra `bitsPrecision`
                    // qubits
                    using ( dyadicFractionNumerator = Qubit[bitsPrecision] ) {

                        // The register that will contain the numerator k of
                        // dyadic fraction k/2^bitsPrecision. The numerator is unsigned 
                        // integer encoded in big-endian format. This is indicated by 
                        // use of Microsoft.Quantum.Canon.BigEndian type.
                        let dyadicFractionNumeratorBE = BigEndian(dyadicFractionNumerator);

                        QuantumPhaseEstimation(oracle, eignestateRegisterLE, dyadicFractionNumeratorBE);

                        // Directly measure the numerator k of dyadic fraction k/2^bitsPrecision 
                        // approximating s/r. Note that phase estimation project on 
                        // the eigenstate corresponding to random s.
                        set dyadicFractionNum = MeasureIntegerBE(dyadicFractionNumeratorBE);
                        }
                    

                    // Return all the qubits used for oracle's eigenstate back to 0 state
                    // using Microsoft.Quantum.Canon.ResetAll
                    ResetAll(eignestateRegister);
                }

                // Sometimes we might measure all zeros state in Phase Estimation.
                // This is a failure and we need to start all over.
                if (dyadicFractionNum == 0) {
                    //fail "We measured 0 for the numerator";
					fail "Phase estimation failed.";
                }
				 
                // This will print our estimate of s/r to the standard output
                // using Microsoft.Quantum.Primitive.Message
                //...Message($"Estimated eigenvalue is {dyadicFractionNum}/2^{bitsPrecision}.");

                // Now we use Microsoft.Quantum.Canon.ContinuedFractionConvergent
                // function to recover s/r from dyadic fraction k/2^bitsPrecision.
                let (numerator, period) = ContinuedFractionConvergent(Fraction(dyadicFractionNum, 2^(bitsPrecision)), modulus);

                // ContinuedFractionConvergent does not guarantee the signs of the numerator 
                // and denominator. Here we make sure that both are positive using 
                // Microsoft.Quantum.Extensions.MathI
                let (numeratorAbs, periodAbs) = (AbsI(numerator), AbsI(period));

                // Use Microsoft.Quantum.Primitive.Message to output the 
                // period divisor and the eigenstate number
                //...Message($"Estimated divisor of period is {periodAbs}, we have projected on eigenstate marked by {numeratorAbs}.");

                // Update the result variable by including newly found divisor.
                // Uses GCD function from Microsoft.Quantum.Canon. 
                set result = periodAbs * result / GCD(result, periodAbs);
            }
            until(ExpMod(generator, result, modulus) == 1)
            fixup {
                // Above we checked if we have found actual period, or only the divisor of it.
                // If the period was found, loop terminates.

                // If we have not found the period, output message about it to 
                // standard output and try again.
                //...Message($"It looks like the period has divisors and we have found only a divisor of the period. Trying again ...");
            }

            // Return found period.
            return result;
        }
    }

	/// # Summary 
    /// Interprets `target` as encoding unsigned little-endian integer k 
    /// and performs transformation |k⟩ ↦ |gᵖ⋅k mod N ⟩ where 
    /// p is `power`, g is `generator` and N is `modulus`.
    /// 
    /// # Input 
    /// ## generator 
    /// The unsigned integer multiplicative order ( period )
    /// of which is being estimated. Must be co-prime to `modulus`.
    /// ## modulus
    /// The modulus which defines the residue ring Z mod `modulus` 
    /// in which the multiplicative order of `generator` is being estimated.
    /// ## power 
    /// Power of `generator` by which `target` is multiplied.
    /// ## target 
    /// Register interpreted as LittleEndian which is multiplied by 
    /// given power of the generator. The multiplication is performed modulo 
    /// `modulus`.
    operation OrderFindingOracle(generator : Int, modulus : Int, power : Int , target : Qubit[] ) : ()
	{
        body
		{
            // Check that the parameters satisfy the requirements.
            AssertBoolEqual(IsCoprime(generator, modulus), true, "`generator` and `modulus` must be co-prime");

            // The oracle we use for order finding essentially wraps 
            // Microsoft.Quantum.Canon.ModularMultiplyByConstantLE operation
            // that implements |x⟩ ↦ |x⋅a mod N ⟩.
            // We also use Quantum.Canon.ExpMod to compute a by which 
            // x must be multiplied.
            // Also note that we interpret target as unsigned integer 
            // in little-endian encoding by using Microsoft.Quantum.Canon.LittleEndian
            // type.
            ModularMultiplyByConstantLE(ExpMod(generator, power, modulus), modulus, LittleEndian(target));
			//ModularMultiplyByConstantLE()
        }
        adjoint auto

        // Phase estimation routines use controlled version of the oracle
        // and therefore OrderFindingOracle must have a controlled version.
        // In this case compiler can easily figure out the controlled version.
        controlled auto
        adjoint controlled auto
    }
}
