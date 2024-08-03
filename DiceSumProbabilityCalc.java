// COMP 233 Probability
// COMP 249 Recursion
// Find probability that the sum of 20 dice rolls X_i is in [30,40]
public class DiceSumProbabilityCalc {
	public static void main(String[] args) {
		long total = 0;
		for(long i = 30; i <= 40; i++) {
			total += choose(i-1, 19) - countCases(i, 20);
		}
		System.out.println("Total: " + total);
	}

	private static long countCases(long n, long k)
	{	// sum < (k-1) 1s and a 7 --> base case
		if(n < (k-1) + 7) { return choose(n-1, k-1); }
		long sb = choose(n-1, k-1);
		for(long j = 7; j <= n-(k-1); j++) {
		// k-many spots to put X_i = j, i in (1,...,k)
			// * countCases left in k-1 positions that sum to n-j
			// if there is no way to put a 7, the multiplication should return the base number of cases
			// n-j >= k-1 at all times so that base case bottoms out at choose(k-1,k-1) where all positions are 1
			sb += k * countCases(n-j, k-1); 
		}
		return sb;
	}
	
	// https://stackoverflow.com/a/2235523
	// -- tested
	public static long choose(long total, long choose){
	    if(total < choose)
	        return 0;
	    if(choose == 0 || choose == total)
	        return 1;
	    return choose(total-1,choose-1)+choose(total-1,choose);
	}
}
