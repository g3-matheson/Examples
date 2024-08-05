
import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class Solution {
	public static void main(String[] args)
	{
		String[] words = {"i","love","leetcode","i","love","coding"};
		int k = 2;
		topKFrequent(words, k);
	}
	
	private static void topKFrequent(String[] words, int k){
		
		Map<String, Integer> hm = new HashMap<String, Integer>();
	
			for(String word : words){
				if(!hm.containsKey(word)){
					hm.put(word, 1);
				} else {
					hm.put(word, hm.get(word) + 1);
				}
			}
	
			List<String> list = new ArrayList<>(hm.keySet());
	
			Collections.sort(list, new Comparator<String>(){
				@Override
				public int compare(String s1, String s2){
					int freq1 = hm.get(s1);
					int freq2 = hm.get(s2);
					if(freq1 == freq2){
						return s1.compareTo(s2);
					} 
					else return freq2 - freq1;
				}
			});
	
			for(int i = 0; i < k; i++)
			{
				System.out.print("\"" + list.get(i) + "\",");
			}
	}
}
